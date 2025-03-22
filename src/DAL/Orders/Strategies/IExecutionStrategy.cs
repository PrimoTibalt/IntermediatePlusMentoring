namespace DAL.Orders.Strategies;

public interface IExecutionStrategy<TKey>
{
	Task<bool> Execute(TKey id);
}
