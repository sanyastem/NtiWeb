using System;

namespace ASUVP.Core.Exceptions
{
    public class EntityAlreadyExistsException : ApplicationException
    {
        public override string Message => "Объект c таким именем уже существует в базе данных";
    }
}