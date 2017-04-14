using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;

namespace GameInCorel
{
    public enum SlotType
    {
        EMPTY,
        CROSS,
        CIRCLE
    }

    public class GameSlot
    {
        private int height = 55;
        private int width = 55;
        private int x;
        private int y;
        private int index;

        private int startX = 42;
        private int startY = 5;
        public SlotType Slot
        {
            get;
            set;
        }
        public int Top { get { return this.y + height; } }
        public int Left { get { return this.x; } }
        public int Right { get { return this.x + width; } }
        public int Bottom { get { return this.y ; } }

        public int CenterX { get { return this.x + width / 2; } }
        public int CenterY { get { return this.y + height / 2; } }
        public GameSlot(int line,int column)
        {
            this.x = startX + column * width;
            this.y = startY + line * height;
        }
        public bool hitTest(int x, int y,PlayerType playerType)
        {
            if (this.Slot == SlotType.EMPTY)
            {
                if (x > this.x && x < this.Right && y > this.y && y < this.Top)
                {
                    
                    if (playerType == PlayerType.X)
                    {
                        drawX(x, y);
                        this.Slot = SlotType.CROSS;
                    }
                    if (playerType == PlayerType.O)
                    {
                        drawO(x, y);
                        this.Slot = SlotType.CIRCLE;
                    }
                    return true;
                }
            }
            return false;
        }
        private void drawX(int x, int y)
        {
            Game.LayerPlayed.CreateLineSegment(this.Left + 7, this.Top - 7, this.Right - 7, this.Bottom + 7);
            Game.LayerPlayed.CreateLineSegment(this.Right - 7, this.Top - 7, this.Left + 7, this.Bottom + 7);
           
        }
        private void drawO(int x, int y)
        {
            Game.LayerPlayed.CreateEllipse2(this.x + width / 2, this.y + height / 2, 20);
        }


    }
}
