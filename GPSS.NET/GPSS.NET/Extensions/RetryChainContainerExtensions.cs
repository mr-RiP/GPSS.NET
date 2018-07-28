using GPSS.SimulationParts;

namespace GPSS.Extensions
{
	internal static class RetryChainContainerExtensions
	{
		public static void TestRetryChain<T>(this T container, TransactionScheduler scheduler) where T : IRetryChainContainer
		{
			var node = container.RetryChain.First;
			while (node != null)
			{
				var retry = node.Value;
				if (retry.Test())
				{
					retry.ReturnToCurrentEvents(scheduler);
					node = container.RetryChain.First;
				}
			}
		}
	}
}
