using System.Web;

namespace Risk.Core
{
    public static class RiskContext
    {
        public static GameManager GetGame()
        {
            if (HttpContext.Current.Application["Game"] == null)
            {
                HttpContext.Current.Application["Game"] = new GameManager(new Settings());
            }

            return (GameManager)HttpContext.Current.Application["Game"];
        }

        public static void ResetGame()
        {
            if (HttpContext.Current.Application["Game"] != null)
            {
                HttpContext.Current.Application.Remove("Game");
            }
        }

        public static Statistics Get()
        {
            if (HttpContext.Current.Application["stats"] == null)
            {
                HttpContext.Current.Application["stats"] = new Statistics();
            }

            return (Statistics)HttpContext.Current.Application["stats"];
        }
    }
}