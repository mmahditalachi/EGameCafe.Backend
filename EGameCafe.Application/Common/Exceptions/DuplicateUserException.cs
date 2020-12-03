using System;


namespace EGameCafe.Application.Common.Exceptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException()
            : base()
        {
        }

        public DuplicateUserException(string message)
            : base(message)
        {
        }
    }
}
