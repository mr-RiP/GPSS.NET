namespace GPSS.Enums
{
	public enum EntityType
	{
		None = 0,

		// General
		Block,
		Transaction,

		// Calculations
		Function,
		Matrix,
		RandomGenerator,
		Savevalue,
		BoolVariable,
		FloatVariable,
		Variable,

		// Groups
		NumericGroup,
		TransactionGroup,
		Userchain,

		// Resources
		Facility,
		Logicswitch,
		Storage,

		// Statistics
		Queue,
		Table
	}
}
