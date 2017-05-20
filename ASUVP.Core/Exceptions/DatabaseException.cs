using System.Data;

namespace ASUVP.Core.Exceptions
{
    public class DatabaseException : DataException
    {
        public override string Message => "Во время выполнения операции с базой данных произошла ошибка";
    }
}