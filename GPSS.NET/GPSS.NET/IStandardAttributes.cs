using GPSS.StandardAttributes;

namespace GPSS
{
	// Класс для обращения к СЧА при составлении модели (при помощи IFunc<StandardAttributes, double>)
	// http://www.minutemansoftware.com/reference/r3.htm
	// http://www.minutemansoftware.com/reference/r4.htm (4.3)
	// Томашевский - стр. 135

	/// <summary>
	/// Model entities attributes (GPSS World SNAs) interaction interface.
	/// </summary>
	public interface IStandardAttributes
	{
		/// <summary>
		/// Transaction attributes interaction.
		/// </summary>
		/// <returns>
		/// Transaction attributes interface.
		/// </returns>
		ITransactionAttributes Transaction();

		/// <summary>
		/// Block attributes interaction.
		/// </summary>
		/// <param name="blockName">Name of Block to interact with.</param>
		/// <returns>
		/// Block attributes interface.
		/// </returns>
		IBlockAttributes Block(string blockName);

		/// <summary>
		/// Facility attributes interaction.
		/// </summary>
		/// <param name="facilityName">Name of the Facility.</param>
		/// <returns>
		/// Facility attributes interface.
		/// </returns>
		IFacilityAttributes Facility(string facilityName);

		/// <summary>
		/// Function attributes interaction.
		/// </summary>
		/// <param name="functionName">Name of the Function.</param>
		/// <returns>Function attributes interface.</returns>
		IFunctionAttributes Function(string functionName);

		/// <summary>
		/// Logic Switch attributes interaction.
		/// </summary>
		/// <param name="logicSwitchName">Name of the Logic Switch.</param>
		/// <returns>
		/// Logic Switch attributes interface.
		/// </returns>
		ILogicswitchAttributes Logicswitch(string logicSwitchName);

		/// <summary>
		/// Matrix attributes interaction.
		/// </summary>
		/// <param name="matrixName">Name of the Matrix.</param>
		/// <returns>
		/// Matrix attributes interface.
		/// </returns>
		IMatrixAttributes Matrix(string matrixName);

		/// <summary>
		/// Queue attributes interaction.
		/// </summary>
		/// <param name="queueName">Name of the Queue.</param>
		/// <returns>
		/// Queue attributes interface.
		/// </returns>
		IQueueAttributes Queue(string queueName);

		/// <summary>
		/// Storage attributes interaction.
		/// </summary>
		/// <param name="storageName">Name of the Storage.</param>
		/// <returns>
		/// Storage attributes interface.
		/// </returns>
		IStorageAttributes Storage(string storageName);

		/// <summary>
		/// Savevalue attributes interaction.
		/// </summary>
		/// <param name="saveValueName">Name of the Savevalue.</param>
		/// <returns>
		/// Savevalue attributes interface.
		/// </returns>
		ISavevalueAttributes Savevalue(string saveValueName);

		/// <summary>
		/// Table attributes interaction.
		/// </summary>
		/// <param name="tableName">Name of the Table.</param>
		/// <returns>
		/// Table attributes interface.
		/// </returns>
		ITableAttributes Table(string tableName);

		/// <summary>
		/// User Chain attributes interaction.
		/// </summary>
		/// <param name="userChainName">Name of the User Chain.</param>
		/// <returns>
		/// User Chain attributes interface.
		/// </returns>
		IUserchainAttributes Userchain(string userChainName);

		/// <summary>
		/// Boolean variable attributes interaction.
		/// </summary>
		/// <param name="variableName">Name of the Variable.</param>
		/// <returns>
		/// Variable attributes interface.
		/// </returns>
		IVariableAttributes<bool> BoolVariable(string variableName);

		/// <summary>
		/// Integer variable attributes interaction.
		/// </summary>
		/// <param name="variableName">Name of the Variable.</param>
		/// <returns>
		/// Variable attributes interface.
		/// </returns>
		IVariableAttributes<int> Variable(string variableName);

		/// <summary>
		/// Floating point variable attributes interaction.
		/// </summary>
		/// <param name="variableName">Name of the Variable.</param>
		/// <returns>
		/// Variable attributes interface.
		/// </returns>
		IVariableAttributes<double> FloatVariable(string variableName);

		/// <summary>
		/// Numeric Group attributes interaction.
		/// </summary>
		/// <param name="numericGroupName">Name of the Numeric Group.</param>
		/// <returns>
		/// Numeric Group attributes interface.
		/// </returns>
		INumericGroupAttributes NumericGroup(string numericGroupName);

		/// <summary>
		/// Transaction Group attributes interaction.
		/// </summary>
		/// <param name="transactionGroupName">Name of the Transaction Group.</param>
		/// <returns>
		/// TransactionGroup attributes interface.
		/// </returns>
		ITransactionGroupAttributes TransactionGroup(string transactionGroupName);

		/// <summary>
		/// Default Random Number Generator attributes interaction.
		/// </summary>
		/// <returns>
		/// Random Number Generator attributes interface.
		/// </returns>
		IRandomNumberGeneratorAttributes RandomNumberGenerator();

		/// <summary>
		/// Random Number Generator attributes interaction.
		/// </summary>
		/// <param name="seed">Seed of the Random Number Generator.</param>
		/// <returns>
		/// Random Number Generator attributes interface.
		/// </returns>
		IRandomNumberGeneratorAttributes RandomNumberGenerator(int seed);

		/// <summary>
		/// System attributes interaction.
		/// </summary>
		/// <returns>
		/// System attributes interface.
		/// </returns>
		ISystemAttributes System();
	}
}
