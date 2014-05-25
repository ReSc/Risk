using System;

namespace Risk.Core
{
    public class RiskException : Exception
    {
        public RiskException(string message)
            : base(message)
        {
        }
    }
}