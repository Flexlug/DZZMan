
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZZMan.API.Exceptions
{
    internal class AlreadyExistsException : Exception
    {
        public AlreadyExistsException()
            : base("This satellite is already in database")
        { }
    }
}
