using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Risk.Core;

namespace Risk.Models.Actions
{
    public interface IAction
    {
        IPlayer Player { get; set; }
        bool ShowOnMap { get; set; }
        string GetMessage();
    }
}
