using System.Linq;
using Risk.Core;
using Risk.Models;
using System.Web.Mvc;

namespace Risk.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var board = new Board { Countries = RiskContext.GetGame().Countries };
            return View(board);
        }

        public ActionResult NextTurn()
        {
            var gameManager = RiskContext.GetGame();
            gameManager.DoNextTurn();
            return View("Index", CreateBoard(gameManager));
        }

        public ActionResult NextPhase()
        {
            var gameManager = RiskContext.GetGame();
            gameManager.DoNextPhase();
            return View("Index", CreateBoard(gameManager));
        }

        public ActionResult SkipStartUpPhase()
        {
            var gameManager = RiskContext.GetGame();
            gameManager.DoStartupPhase();
            return View("Index", CreateBoard(gameManager));
        }

        public ActionResult NewGame()
        {
            RiskContext.ResetGame();
            return View("Index", CreateBoard(RiskContext.GetGame()));
        }

        public ActionResult EndGame()
        {
            var gameManager = RiskContext.GetGame();

            do
            {
                gameManager.DoNextTurn();
            } while (!gameManager.GameEnded);

            return View("Index", CreateBoard(RiskContext.GetGame()));
        }

        private Board CreateBoard(GameManager gameManager)
        {
            return new Board
            {
                Countries = gameManager.Countries,
                Actions = gameManager.Actions,
                NextPhase = gameManager.CurrentPhase,
                ActivePlayer = gameManager.CurrentPlayer,
                Players = gameManager.Players.ToList(),
                GameEnded = gameManager.GameEnded,
                TimesWonByPlayers = RiskContext.GetStats().TimesWonByPlayer,
                Turn = gameManager.Turn,
                LastPhase = gameManager.LastPhase,
                LastPlayer = gameManager.LastPhase == EPhase.Move ? gameManager.LastPlayer : gameManager.CurrentPlayer 
            };
        }


    }
}
