using GPSS.Entities.Calculations;

namespace GPSS.ReportParts
{
	/// <summary>
	/// Savevalue simulation data class.
	/// </summary>
	public class SavevalueData
	{
		internal SavevalueData(string name, Savevalue savevalue)
		{
			Name = name;
			Value = savevalue.Value;
			RetryChainCount = savevalue.RetryChain.Count;
		}

		/// <summary>
		/// Savevalue Entity name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Savevalue value.
		/// </summary>
		public dynamic Value { get; private set; }

		/// <summary>
		/// Savevalue's Retry Chain content count.
		/// </summary>
		public int RetryChainCount { get; private set; }
	}
}
