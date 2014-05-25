using Risk.Core;

namespace Risk.Models.Actions
{
    public class InfoAction : IAction
    {
        public InfoAction()
        {
            ShowOnMap = false;
        }

        public string Message { get; set; }

        public IPlayer Player { get; set; }
        public bool ShowOnMap { get; set; }

        public string GetMessage()
        {
            return Message;
        }
    }
}