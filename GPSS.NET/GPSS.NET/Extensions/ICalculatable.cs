namespace GPSS.Extensions
{
	interface ICalculatable<T>
	{
		T Result { get; }

		void Calculate(IStandardAttributes simulation);
	}
}
