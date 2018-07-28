using GPSS.Entities.General;
using System.Collections.ObjectModel;

namespace GPSS.ReportParts
{
	/// <summary>
	/// CEC transaction simulation data.
	/// </summary>
	public class CurrentEventData
	{
		internal CurrentEventData(Transaction transaction)
		{
			Number = transaction.Number;
			Priority = transaction.Priority;
			MarkTime = transaction.MarkTime;
			Assembly = transaction.Assembly;
			CurrentBlockIndex = transaction.CurrentBlock;
			NextBlockIndex = transaction.NextBlock;
			Parameters = new ReadOnlyDictionary<string, dynamic>(transaction.Parameters);
		}

		/// <summary>
		/// Transcation Number value.
		/// </summary>
		public int Number { get; private set; }

		/// <summary>
		/// Transaction Priority value.
		/// </summary>
		public int Priority { get; private set; }

		/// <summary>
		/// Transaction Mark Time value.
		/// </summary>
		public double MarkTime { get; private set; }

		/// <summary>
		/// Transaction Assembly value.
		/// </summary>
		public int Assembly { get; private set; }

		/// <summary>
		/// Transaction Current Block value.
		/// </summary>
		public int CurrentBlockIndex { get; private set; }

		/// <summary>
		/// Transaction Next Block value.
		/// </summary>
		public int NextBlockIndex { get; private set; }

		/// <summary>
		/// Transaction Parameters collection.
		/// </summary>
		public ReadOnlyDictionary<string, dynamic> Parameters { get; private set; }
	}
}
