using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Risk.Core;

namespace Risk.Models.Actions
{
    public class InfoAction : IAction
    {
        public IPlayer Player { get; set; }
        public bool ShowOnMap { get; set; }

        public string Message { get; set; }

        public InfoAction()
        {
            ShowOnMap = false;
        }

        public string GetMessage()
        {
            return Message;
        }
    }
}