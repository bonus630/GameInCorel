using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameInCorel
{
    public enum PlayerType
    {
        X,
        O
    }
    public class Player
    {
        private PlayerType playerType;
        public int Score { get; set; }
        public string Name { get; set; }
        public PlayerType _PlayerType { get {return this.playerType; } }
        public Player(PlayerType playerType)
        {
            this.playerType = playerType;
        }
    }
}
