﻿using System;
using System.Threading;
using System.Web;

namespace Risk.Core
{
    // dubious thread safety here...
    public static class RiskContext
    {
        public static GameManager GetGame()
        {
            
            if (HttpContext.Current.Application["Game"] == null)
            {
                HttpContext.Current.Application["Game"] = new GameManager(GetStats(), new Settings());
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

        public static Statistics GetStats()
        {
            if (HttpContext.Current.Application["stats"] == null)
            {
                HttpContext.Current.Application["stats"] = new Statistics();
            }

            return (Statistics)HttpContext.Current.Application["stats"];
        }
    }
}