using DAL;
using DAL.Events;
using DAL.Payments;
using DAL.Payments.Repository;
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
			Assert.Equal(PaymentStatus.Failed.ToString().ToLowerInvariant(), payment.Status);
			Assert.All(payment.Cart.CartItems, item =>
			{
				Assert.Equal(SeatStatus.Available.ToString().ToLowerInvariant(), item.EventSeat.Status);
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
			Assert.Equal(PaymentStatus.Completed.ToString().ToLowerInvariant(), payment.Status);
			Assert.All(payment.Cart.CartItems, item =>
			{
				Assert.Equal(SeatStatus.Sold.ToString().ToLowerInvariant(), item.EventSeat.Status);
			});
		}

		[Theory]
		[InlineData(GetValuesSuites.ManyValues, null)]
		[InlineData(GetValuesSuites.OneValue, SeatStatus.Sold)]
		[InlineData(GetValuesSuites.ManyValues, SeatStatus.Sold)]
		[InlineData(GetValuesSuites.OneValue, SeatStatus.Available)]
		[InlineData(GetValuesSuites.ManyValues, SeatStatus.Available)]
		// Functionality is not implemented for validating status.
		public async Task FailPayment_ExistingValuesWithIncorrectStatus_BookedToAvailable(GetValuesSuites suite, SeatStatus? status)
		{
			var payment = Suites.GetPayment(suite, status);
			Dictionary<long, string> seatIdStatusMap = new();
			var paymentId = long.MaxValue;
			payment.Id = paymentId;
			var repository = serviceProvider.GetService<IPaymentRepository>();
			await repository.Create(payment);
			await repository.Save();
			foreach (var item in payment.Cart.CartItems)
			{
				seatIdStatusMap.Add(item.EventSeat.Id, item.EventSeat.Status);
			}

			var result = await repository.FailPayment(paymentId);

			Assert.True(result);
			Assert.All(payment.Cart.CartItems, item =>
			{
				var oldStatus = seatIdStatusMap[item.EventSeat.Id];
				if (oldStatus == SeatStatus.Sold.ToString().ToLowerInvariant())
					Assert.Equal(oldStatus, item.EventSeat.Status);
				else
					Assert.Equal(SeatStatus.Available.ToString().ToLowerInvariant(), item.EventSeat.Status);
			});
		}

		[Theory]
		[InlineData(GetValuesSuites.ManyValues, null)]
		[InlineData(GetValuesSuites.OneValue, SeatStatus.Sold)]
		[InlineData(GetValuesSuites.ManyValues, SeatStatus.Sold)]
		[InlineData(GetValuesSuites.OneValue, SeatStatus.Available)]
		[InlineData(GetValuesSuites.ManyValues, SeatStatus.Available)]
		// Functionality is not implemented for validating status.
		public async Task CompletePayment_ExistingValuesWithIncorrectStatus_ResultFalse_StatusRemains(GetValuesSuites suite, SeatStatus? status)
		{
			var payment = Suites.GetPayment(suite, status);
			Dictionary<long, string> seatIdStatusMap = new();
			var paymentId = 1;
			payment.Id = paymentId;
			var repository = serviceProvider.GetService<IPaymentRepository>();
			await repository.Create(payment);
			await repository.Save();
			foreach (var item in payment.Cart.CartItems)
			{
				seatIdStatusMap.Add(item.EventSeat.Id, item.EventSeat.Status);
			}

			var result = await repository.CompletePayment(paymentId);

			Assert.False(result);
			Assert.All(payment.Cart.CartItems, item =>
			{
				Assert.Equal(seatIdStatusMap[item.EventSeat.Id], item.EventSeat.Status);
			});
		}
	}
}
