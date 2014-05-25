using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Risk.Core;

namespace Risk.Players
{
    public class player33 : Player3
    {
        public new void Hallo()
        {
            base.Hallo();
        }
    }

    public abstract class Player3 : IPlayer
    {

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string Color
        {
            get { throw new NotImplementedException(); }
        }

        public void Deploy(int numberOfTroops)
        {
            //deploy
        }

        public T Hallo<T>() where T : IPlayer
        {
            
        }

        public void Hallo()
        {
            
        }

        public void Attack()
        {

        }

        public void Move()
        {
            throw new NotImplementedException();
        }
    }
}