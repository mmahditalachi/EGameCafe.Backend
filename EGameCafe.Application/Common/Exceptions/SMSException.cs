using System;
using System.Collections.Generic;
using System.Text;

namespace EGameCafe.Application.Common.Exceptions
{
    public class SMSException : Exception
    {
        public SMSException(string name, object error)
            : base($"Error at \"{name}\" ({error}) was found.")
        {

        }
    }
}
