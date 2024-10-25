using MediatR;
using Moq;

namespace TestsCore
{
    public class MediatorMockObjectBuilder
    {
        private Mock<IMediator> mediator;

        public MediatorMockObjectBuilder()
        {
            mediator = new Mock<IMediator>();
        }

        public MediatorMockObjectBuilder AppendSetup<TInput, TOutput>(TOutput output) where TInput : IRequest<TOutput>
        {
            mediator.Setup(m => m.Send(It.IsAny<TInput>(), CancellationToken.None)).ReturnsAsync(output);
            return this;
        }

        public IMediator GetObject() => mediator.Object;

        public static IMediator Get<TInput, TOutput>(TOutput output) where TInput : IRequest<TOutput>
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<TInput>(), CancellationToken.None)).ReturnsAsync(output);
            return mediator.Object;
        }
    }
}
