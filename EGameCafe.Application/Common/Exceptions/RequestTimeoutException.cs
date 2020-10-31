using System;
using System.Collections.Generic;
using System.Text;

namespace EGameCafe.Application.Common.Exceptions
{
    public class RequestTimeoutException : Exception
    {
        public RequestTimeoutException(string name, object error)
            : base($"{name} : {error}")
        {

        }
    }
}
