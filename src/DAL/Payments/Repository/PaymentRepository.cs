using DAL.Events;
using Microsoft.EntityFrameworkCore;

namespace DAL.Payments.Repository
{
	internal class PaymentRepository : GenericRepository<Payment, long, PaymentContext>, IPaymentRepository
	{
		public PaymentRepository(PaymentContext context) : base(context) { }

		public async Task<bool> CompletePayment(long id)
		{
			using var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
				var payment = await _collection.Include(p => p.Cart)
						.ThenInclude(c => c.CartItems)
						.ThenInclude(ci => ci.EventSeat)
						.FirstOrDefaultAsync(p => p.Id == id);
				var seats = payment.Cart.CartItems.Select(ci => ci.EventSeat).ToList();
				foreach (var seat in seats)
				{
					seat.Status = SeatStatusStrings.Sold;
				}

				var result = await _context.SaveChangesAsync();
				if (result != seats.Count)
					throw new DbUpdateException("Some seats were already sold. Check your cart and delete them before finishing a payment");

				payment.Status = PaymentStatus.Completed.ToString().ToLowerInvariant();
				await _context.SaveChangesAsync();
				await transaction.CommitAsync();
				return true;
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				throw;
			}
		}

		public async Task<bool> FailPayment(long id)
		{
			using var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
				var payment = await _collection.Include(p => p.Cart)
						.ThenInclude(c => c.CartItems)
						.ThenInclude(ci => ci.EventSeat)
						.FirstOrDefaultAsync(p => p.Id == id);
				var seats = payment.Cart.CartItems.Select(ci => ci.EventSeat).ToList();
				foreach (var seat in seats)
				{
					seat.Status = SeatStatusStrings.Available;
				}

				var result = await _context.SaveChangesAsync();
				payment.Status = PaymentStatus.Failed.ToString().ToLowerInvariant();
				await _context.SaveChangesAsync();
				await transaction.CommitAsync();
				return true;
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				throw;
			}
		}
	}
}