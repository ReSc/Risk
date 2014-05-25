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