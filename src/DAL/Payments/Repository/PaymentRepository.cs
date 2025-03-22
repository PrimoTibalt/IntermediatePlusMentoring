using DAL.Abstraction;
using Entities.Events;
using Entities.Payments;
using Microsoft.EntityFrameworkCore;
using PaymentApplication.Repository;

namespace DAL.Payments.Repository;

internal class PaymentRepository(PaymentContext context) :
	GenericRepository<Payment, long, PaymentContext>(context),
	IPaymentRepository
{
	public static IQueryable<Payment> IncludeRelatedInfo(DbSet<Payment> payment)
	{
		return payment
			.Include(p => p.Cart.User)
			.Include(p => p.Cart.CartItems)
				.ThenInclude(ci => ci.EventSeat.Event)
			.Include(p => p.Cart.CartItems)
				.ThenInclude(ci => ci.Price);
	}

	public async Task<Payment> GetPaymentWithRelatedInfo(long id)
	{
		var payment = await IncludeRelatedInfo(_collection)
			.FirstOrDefaultAsync(p => p.Id == id);

		return payment;
	}

	public async Task<bool> CompletePayment(long id)
	{
		using var transaction = await _context.Database.BeginTransactionAsync();
		var payment = await _collection
			.Include(p => p.Cart.CartItems)
			.ThenInclude(ci => ci.EventSeat)
			.FirstOrDefaultAsync(p => p.Id == id);
		var seats = payment.Cart.CartItems.Select(ci => ci.EventSeat).ToList();
		var notBookedSeatsMessages = new List<string>();
		foreach (var seat in seats)
		{
			if (seat.Status == (int)SeatStatus.Booked)
				seat.Status = (int)SeatStatus.Sold;
			else
				notBookedSeatsMessages.Add($"Seat with id {seat.Id} has status other than {nameof(SeatStatus.Booked)}");
		}

		if (notBookedSeatsMessages.Count != 0)
		{
			await transaction.RollbackAsync();
			throw new DbUpdateException(string.Join("\n", notBookedSeatsMessages));
		}

		var result = await _context.SaveChangesAsync();
		if (result < seats.Count)
			throw new DbUpdateException("Some seats were already sold. Check your cart and delete them before finishing a payment");

		payment.Status = (int)PaymentStatus.Completed;
		await _context.SaveChangesAsync();
		await transaction.CommitAsync();
		return true;
	}

	public async Task<bool> FailPayment(long id)
	{
		using var transaction = await _context.Database.BeginTransactionAsync();
		var payment = await _collection
			.Include(p => p.Cart.CartItems)
			.ThenInclude(ci => ci.EventSeat)
			.FirstOrDefaultAsync(p => p.Id == id);
		var seats = payment.Cart.CartItems.Select(ci => ci.EventSeat).ToList();
		var notBookedOrAvailableSeatsMessages = new List<string>();
		foreach (var seat in seats)
		{
			if (seat.Status == (int)SeatStatus.Booked || seat.Status == (int)SeatStatus.Available)
				seat.Status = (int)SeatStatus.Available;
			else
				notBookedOrAvailableSeatsMessages.Add(
					$"Seat with id {seat.Id} has status other than {nameof(SeatStatus.Booked)} or {nameof(SeatStatus.Available)}");
		}

		if (notBookedOrAvailableSeatsMessages.Count != 0)
		{
			await transaction.RollbackAsync();
			throw new DbUpdateException(string.Join("\n", notBookedOrAvailableSeatsMessages));
		}

		var result = await _context.SaveChangesAsync();
		payment.Status = (int)PaymentStatus.Failed;
		await _context.SaveChangesAsync();
		await transaction.CommitAsync();
		return true;
	}
}