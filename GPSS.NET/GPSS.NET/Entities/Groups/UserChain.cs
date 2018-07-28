using GPSS.Entities.General;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;

namespace GPSS.Entities.Groups
{
	internal class Userchain : ICloneable, IUserchainAttributes
	{
		public LinkedList<Transaction> Chain { get; private set; } = new LinkedList<Transaction>();

		public double AverageContent => throw new NotImplementedException();

		public int TotalEntries => throw new NotImplementedException();

		public int CurrentContent => throw new NotImplementedException();

		public int MaximumContent => throw new NotImplementedException();

		public double AverageResidenceTime => throw new NotImplementedException();

		public Userchain Clone()
		{
			throw new NotImplementedException();
		}

		object ICloneable.Clone() => Clone();
	}
}
