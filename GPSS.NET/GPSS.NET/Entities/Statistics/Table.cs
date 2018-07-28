using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Statistics
{
	internal class Table : ICloneable, ITableAttributes
	{
		public double Average => throw new NotImplementedException();

		public int Count => throw new NotImplementedException();

		public double StandardDeviation => throw new NotImplementedException();

		public Table Clone()
		{
			throw new NotImplementedException();
		}

		object ICloneable.Clone() => Clone();
	}
}
