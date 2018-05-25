using GPSS.Attributes;
using System;

namespace GPSS
{
    // Класс для обращения к СЧА при составлении модели (при помощи IFunc<StandardAttributes, double>)
    // http://www.minutemansoftware.com/reference/r3.htm
    // http://www.minutemansoftware.com/reference/r4.htm (4.3)
    // Томашевский - стр. 135
    public class StandardAttributes
    {
        // системные СЧА
        public ISystemAttributes System()
        {
            throw new NotImplementedException();
        }

        // СЧА текущего транзакта
        public ITransactionAttributes Transaction()
        {
            throw new NotImplementedException(); 
        }

        // СЧА блока
        public IBlockAttributes Block(int index)
        {
            throw new NotImplementedException();
        }

        // СЧА устройства
        public IFacilityAttributes Facility(int name)
        {
            throw new NotImplementedException();
        }

        // СЧА функции - FNEntnum - возвращает значение функции
        public IFunctionAttributes Function(string name)
        {
            throw new NotImplementedException();
        }

        // CЧА логического переключателя (?) - возвращает состояние
        public ILogicSwitchAttributes LogicSwitch(string name)
        {
            throw new NotImplementedException();
        }

        // СЧА матрицы - вынесены в отдельный класс для реализации произвольной размерности
        public IMatrixAttributes Matrix(string name)
        {
            throw new NotImplementedException();
        }

        // CЧА очереди
        public IQueueAttributes Queue(string name)
        {
            throw new NotImplementedException();
        }

        // CЧА многоканального устройства
        public IStorageAttributes Storage(string name)
        {
            throw new NotImplementedException();
        }

        // CЧА сохраненного значения - получение этого значения
        public ISaveValueAttributes SaveValue(string name)
        {
            throw new NotImplementedException();
        }

        public ITableAttributes Table(string name)
        {
            throw new NotImplementedException();
        }

        public IUserChainAttributes UserChain(string name)
        {
            throw new NotImplementedException();
        }

        public IVariableAttributes Variable(string name)
        {
            throw new NotImplementedException();
        }

        // GNEntnum - Numeric Group count. GNEntnum returns the membership count of Numeric Group Entity Entnum.
        public INumericGroupAttributes NumericGroup(string name)
        {
            throw new NotImplementedException();
        }

        // GTEntnum - Transaction Group count. GTEntnum returns the membership count of Transaction Group Entnum.
        public ITransactionGroupAttributes TransactionGroup(string name)
        {
            throw new NotImplementedException();
        }

        public IRandomNumberGeneratorAttributes RandomNumberGenerator(string name)
        {
            throw new NotImplementedException();
        }
      
    }
}
