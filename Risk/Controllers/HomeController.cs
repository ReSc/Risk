using Risk.Core;
using Risk.Models;
using System.Web.Mvc;

namespace Risk.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var board = new Board { Countries = GameManager.Get().Countries };
            return View(board);
        }

        public ActionResult NextTurn()
        {
            var gameManager = GameManager.Get();
            gameManager.DoNextTurn();
            return View("Index", CreateBoard(gameManager));
        }

        public ActionResult NextPhase()
        {
            var gameManager = GameManager.Get();
            gameManager.DoNextPhase();
            return View("Index", CreateBoard(gameManager));
        }

        public ActionResult SkipStartUpPhase()
        {
            var gameManager = GameManager.Get();
            gameManager.DoStartupPhase();
            return View("Index", CreateBoard(gameManager));
        }

        public ActionResult NewGame()
        {
            GameManager.Reset();
            return View("Index", CreateBoard(GameManager.Get()));
        }

        public ActionResult EndGame()
        {
            var gameManager = GameManager.Get();

            do
            {
                gameManager.DoNextTurn();
            } while (!gameManager.GameEnded);

            return View("Index", CreateBoard(GameManager.Get()));
        }

        private Board CreateBoard(GameManager gameManager)
        {
            return new Board
            {
                Countries = gameManager.Countries,
                Actions = gameManager.Actions,
                NextPhase = gameManager.CurrentPhase,
                ActivePlayer = gameManager.CurrentPlayer,
                Players = Settings.Players,
                GameEnded = gameManager.GameEnded,
                TimesWonByPlayers = Statistics.Get().TimesWonByPlayer,
                Turn = gameManager.Turn,
                LastPhase = gameManager.LastPhase,
                LastPlayer = gameManager.LastPhase == EPhase.Move ? gameManager.LastPlayer : gameManager.CurrentPlayer 
            };
        }


    }
}
