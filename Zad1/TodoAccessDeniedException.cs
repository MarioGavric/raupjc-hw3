using System;

namespace Zad1
{
    public class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException(string message) : base(message)
        {
            
        }
    }
}