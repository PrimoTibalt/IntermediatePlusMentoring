using DAL;
using DAL.Events;
using DAL.Payments;
using DAL.Payments.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestsCore;
using TestsCore.Providers;

namespace PaymentTests.DALTests
{
	public class PaymentRepositoryTests
	{
		private IServiceProvider serviceProvider =>
			ServiceConfigurationProvider.Get<PaymentContext>(services => services.AddPaymentRepositories());

		[Theory]
		[InlineData(GetValuesSuites.Empty)]
		[InlineData(GetValuesSuites.OneValue)]
		[InlineData(GetValuesSuites.ManyValues)]
		public async Task FailPayment_ExistingValues_SeatsAvailable(GetValuesSuites suite)
		{
			var payment = Suites.GetPayment(suite, SeatStatus.Booked);
			var paymentId = 1;
			payment.Id = paymentId;
			var repository = serviceProvider.GetService<IPaymentRepository>();
			await repository.Create(payment);
			await repository.Save();

			var result = await repository.FailPayment(paymentId);

			Assert.True(result);
			Assert.Equal((int)PaymentStatus.Failed, payment.Status);
			Assert.All(payment.Cart.CartItems, item =>
			{
				Assert.Equal((int)SeatStatus.Available, item.EventSeat.Status);
			});
		}

		[Theory]
		[InlineData(GetValuesSuites.Empty)]
		[InlineData(GetValuesSuites.OneValue)]
		[InlineData(GetValuesSuites.ManyValues)]
		public async Task CompletePayment_ExistingValues_SeatsSold(GetValuesSuites suite)
		{
			var payment = Suites.GetPayment(suite, SeatStatus.Booked);
			var paymentId = 1;
			payment.Id = paymentId;
			var repository = serviceProvider.GetService<IPaymentRepository>();
			await repository.Create(payment);
			await repository.Save();

			var result = await repository.CompletePayment(paymentId);

			Assert.True(result);
			Assert.Equal((int)PaymentStatus.Completed, payment.Status);
			Assert.All(payment.Cart.CartItems, item =>
			{
				Assert.Equal((int)SeatStatus.Sold, item.EventSeat.Status);
			});
		}

		[Theory]
		[InlineData(GetValuesSuites.ManyValues, null)]
		[InlineData(GetValuesSuites.OneValue, SeatStatus.Sold)]
		[InlineData(GetValuesSuites.ManyValues, SeatStatus.Sold)]
		public async Task FailPayment_ExistingValuesWithIncorrectStatus_Throws(GetValuesSuites suite, SeatStatus? status)
		{
			var payment = Suites.GetPayment(suite, status);
			Dictionary<long, int> seatIdStatusMap = new();
			var paymentId = long.MaxValue;
			payment.Id = paymentId;
			var repository = serviceProvider.GetService<IPaymentRepository>();
			await repository.Create(payment);
			await repository.Save();
			foreach (var item in payment.Cart.CartItems)
			{
				seatIdStatusMap.Add(item.EventSeat.Id, item.EventSeat.Status);
			}

			await Assert.ThrowsAsync<DbUpdateException>(async () =>
			{
				await repository.FailPayment(paymentId);
			});
		}

		[Theory]
		[InlineData(GetValuesSuites.ManyValues, null)]
		[InlineData(GetValuesSuites.OneValue, SeatStatus.Sold)]
		[InlineData(GetValuesSuites.ManyValues, SeatStatus.Sold)]
		[InlineData(GetValuesSuites.OneValue, SeatStatus.Available)]
		[InlineData(GetValuesSuites.ManyValues, SeatStatus.Available)]
		public async Task CompletePayment_ExistingValuesWithIncorrectStatus_Throws(GetValuesSuites suite, SeatStatus? status)
		{
			var payment = Suites.GetPayment(suite, status);
			Dictionary<long, int> seatIdStatusMap = new();
			var paymentId = 1;
			payment.Id = paymentId;
			var repository = serviceProvider.GetService<IPaymentRepository>();
			await repository.Create(payment);
			await repository.Save();
			foreach (var item in payment.Cart.CartItems)
			{
				seatIdStatusMap.Add(item.EventSeat.Id, item.EventSeat.Status);
			}

			await Assert.ThrowsAsync<DbUpdateException>(async () =>
			{
				await repository.CompletePayment(paymentId);
			});
		}
	}
}
