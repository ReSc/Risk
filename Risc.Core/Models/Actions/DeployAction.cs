using Risk.Core;

namespace Risk.Models.Actions
{
    public class DeployAction : IAction
    {
        public DeployAction()
        {
            ShowOnMap = true;
        }

        public Country CountryToDeploy { get; set; }
        public int NumberOfAddedTroops { get; set; }
        public IPlayer Player { get; set; }
        public bool ShowOnMap { get; set; }

        public string GetMessage()
        {
            return string.Format("{0} added {1} troops in {2}", Player.Name, NumberOfAddedTroops,
                                 CountryToDeploy.GetName());
        }
    }
}