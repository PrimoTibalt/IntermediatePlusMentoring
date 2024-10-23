using DAL;
using DAL.Events;
using DAL.Payments;
using DAL.Payments.Repository;
using Microsoft.Extensions.DependencyInjection;
using TestsCore;

namespace PaymentTests.DALTests
{
	public class PaymentRepositoryTests
	{
		[Theory]
		[InlineData(GetValuesSuites.Empty)]
		[InlineData(GetValuesSuites.OneValue)]
		[InlineData(GetValuesSuites.ThreeValues)]
		[InlineData(GetValuesSuites.ManyValues)]
		public async Task FailPayment_ExistingValues_SeatsAvailable(GetValuesSuites suite)
		{
			await ProcessPayment_ExistingValues_Status(suite,
				complete: false,
				PaymentStatus.Failed.ToString().ToLowerInvariant(),
				SeatStatus.Available.ToString().ToLowerInvariant());
		}

		[Theory]
		[InlineData(GetValuesSuites.Empty)]
		[InlineData(GetValuesSuites.OneValue)]
		[InlineData(GetValuesSuites.ThreeValues)]
		[InlineData(GetValuesSuites.ManyValues)]
		public async Task CompletePayment_ExistingValues_SeatsSold(GetValuesSuites suite)
		{
			await ProcessPayment_ExistingValues_Status(suite,
				complete: true,
				PaymentStatus.Completed.ToString().ToLowerInvariant(),
				SeatStatus.Sold.ToString().ToLowerInvariant());
		}

		[Theory]
		[InlineData(GetValuesSuites.ManyValues, null)]
		[InlineData(GetValuesSuites.OneValue, SeatStatus.Sold)]
		[InlineData(GetValuesSuites.ThreeValues, SeatStatus.Sold)]
		[InlineData(GetValuesSuites.ManyValues, SeatStatus.Sold)]
		[InlineData(GetValuesSuites.OneValue, SeatStatus.Available)]
		[InlineData(GetValuesSuites.ThreeValues, SeatStatus.Available)]
		[InlineData(GetValuesSuites.ManyValues, SeatStatus.Available)]
		// Functionality is not implemented for validating status.
		public async Task FailPayment_ExistingValuesWithIncorrectStatus_BookedToAvailable(GetValuesSuites suite, SeatStatus? status)
		{
			var payment = Suites.GetPayment(suite, status);
			Dictionary<long, string> seatIdStatusMap = new();
			var paymentId = long.MaxValue;
			payment.Id = paymentId;
			var serviceProvider = ServiceConfigurationProvider.Get<PaymentContext>(services => services.AddPaymentRepositories());
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
		[InlineData(GetValuesSuites.ThreeValues, SeatStatus.Sold)]
		[InlineData(GetValuesSuites.ManyValues, SeatStatus.Sold)]
		[InlineData(GetValuesSuites.OneValue, SeatStatus.Available)]
		[InlineData(GetValuesSuites.ThreeValues, SeatStatus.Available)]
		[InlineData(GetValuesSuites.ManyValues, SeatStatus.Available)]
		// Functionality is not implemented for validating status.
		public async Task CompletePayment_ExistingValuesWithIncorrectStatus_ResultFalse_StatusRemains(GetValuesSuites suite, SeatStatus? status)
		{
			var payment = Suites.GetPayment(suite, status);
			Dictionary<long, string> seatIdStatusMap = new();
			var paymentId = long.MaxValue;
			payment.Id = paymentId;
			var serviceProvider = ServiceConfigurationProvider.Get<PaymentContext>(services => services.AddPaymentRepositories());
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

		private static async Task ProcessPayment_ExistingValues_Status(GetValuesSuites suite, bool complete, string paymentStatus, string seatStatus)
		{
			var payment = Suites.GetPayment(suite, SeatStatus.Booked);
			var paymentId = 6645234535;
			payment.Id = paymentId;
			var serviceProvider = ServiceConfigurationProvider.Get<PaymentContext>(services => services.AddPaymentRepositories());
			var repository = serviceProvider.GetService<IPaymentRepository>();
			await repository.Create(payment);
			await repository.Save();

			var result = complete ? await repository.CompletePayment(paymentId)
				: await repository.FailPayment(paymentId);

			Assert.True(result);
			Assert.Equal(paymentStatus, payment.Status);
			Assert.All(payment.Cart.CartItems, item =>
			{
				Assert.Equal(seatStatus, item.EventSeat.Status);
			});
		}
	}
}
