using System.Collections.Generic;
using Risk.Core;
using Risk.Models.Actions;

namespace Risk.Models
{
    public class Board
    {
        public Board()
        {
            Countries = new List<Country>();
            Actions = new List<IAction>();
            Players = new List<IPlayer>();
            TimesWonByPlayers = new Dictionary<string, int>();
        }

        public List<Country> Countries { get; set; }
        public List<IAction> Actions { get; set; }
        public List<IPlayer> Players { get; set; }
        public IPlayer ActivePlayer { get; set; }

        public bool GameEnded { get; set; }
        public int Turn { get; set; }
        public EPhase NextPhase { get; set; }

        public IPlayer LastPlayer { get; set; }
        public EPhase LastPhase { get; set; }

        public Dictionary<string, int> TimesWonByPlayers { get; set; }
    }
}