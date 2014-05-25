using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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