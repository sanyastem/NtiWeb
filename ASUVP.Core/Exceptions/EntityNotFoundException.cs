using System.Collections.Generic;

namespace ASUVP.Core.Exceptions
{
    public class EntityNotFoundException : KeyNotFoundException
    {
        public override string Message => "Объект не найден в базе данных";
    }
}