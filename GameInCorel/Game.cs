using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;

namespace GameInCorel
{
    public class Game
    {
        private GameSlot[] gameSlots = new GameSlot[9];
        private Player PlayerX;
        private Player PlayerO;
        private Player currentPlayer;
        private Shape endButton;
        int numTurns;
        bool gameRunning = true;
        bool endGame = false;
        bool winner = false;

        public static Layer LayerBackground, LayerScore, LayerPlayed, LayerTurn;

        public Game()
        {
            int index = 0;
            for (int i = 0; i < 3; i++)
			{
                for (int r = 0; r < 3; r++)
			    {
			        gameSlots[index] = new GameSlot(i,r);
                    index++;
			    }
			  
            }
            PlayerX = new Player(PlayerType.X);
            PlayerO = new Player(PlayerType.O);
            currentPlayer = PlayerX;
            numTurns = 0;
        }
        private void drawBackground()
        {
            //DockerUI.corelApp.ActivePage.GetLayerByName("Background").Activate();
            DockerUI.corelApp.Optimization = true;
            Shape q = LayerBackground.CreateRectangle2(10, 220, 110, 20,5,5,5,5);
            Shape q2 = DockerUI.corelApp.ActiveLayer.CreateRectangle2(130, 220, 110, 20, 5, 5, 5, 5);
            Shape q3 = DockerUI.corelApp.ActiveLayer.CreateRectangle2(75, 190, 110, 20, 5, 5, 5, 5);
            endButton = DockerUI.corelApp.ActiveLayer.CreateRectangle2(190, 190, 30, 20, 5, 5, 5, 5);
            q.Outline.SetNoOutline();
            q2.Outline.SetNoOutline();
            q3.Outline.SetNoOutline();
            endButton.Outline.SetNoOutline();
            Color gray = new Color();
            gray.RGBAssign(140, 140, 140);
            q.Fill.UniformColor = gray;
            q2.Fill.UniformColor = gray;
            q3.Fill.UniformColor = gray;
            endButton.Fill.UniformColor = gray;

            LayerBackground.CreateArtisticText(15, 222, "X -",cdrTextLanguage.cdrLanguageNone,cdrTextCharSet.cdrCharSetMixed,null,5);
            LayerBackground.CreateArtisticText(135, 222, "O -", cdrTextLanguage.cdrLanguageNone, cdrTextCharSet.cdrCharSetMixed, null, 5);
            LayerBackground.CreateArtisticText(195, 192, "End", cdrTextLanguage.cdrLanguageNone, cdrTextCharSet.cdrCharSetMixed, null, 3);
           
            LayerBackground.CreateLineSegment(gameSlots[0].Right,gameSlots[0].Bottom,gameSlots[6].Right,gameSlots[6].Top);
            LayerBackground.CreateLineSegment(gameSlots[1].Right, gameSlots[1].Bottom, gameSlots[7].Right, gameSlots[7].Top);
            LayerBackground.CreateLineSegment(gameSlots[3].Left,gameSlots[3].Bottom,gameSlots[5].Right,gameSlots[5].Bottom);
            LayerBackground.CreateLineSegment(gameSlots[6].Left, gameSlots[6].Bottom, gameSlots[8].Right, gameSlots[8].Bottom);
            DockerUI.corelApp.Optimization = false;
            DockerUI.corelApp.Refresh();
        }
        public bool Click(double x, double y,PlayerType playerType)
        {
            for (int i = 0; i < gameSlots.Length; i++)
            {
                if(gameSlots[i].hitTest((int)x, (int)y, playerType))
                    return true;
            }
           
            if(endButton.DisplayCurve.IsPointInside(x,y))
            {
                endGame = true;
            }
            return false;
        }
        public void Start()
        {
            createLayers();
            drawBackground();
            score();
            newGame();
            gameLoop();
        }

        private void createLayers()
        {
            LayerBackground = DockerUI.corelApp.ActiveDocument.ActivePage.CreateLayer("Background");
            LayerBackground.Editable = false;
            LayerScore = DockerUI.corelApp.ActiveDocument.ActivePage.CreateLayer("Score");
            LayerScore.Editable = false;
            LayerPlayed = DockerUI.corelApp.ActiveDocument.ActivePage.CreateLayer("Played");
            LayerPlayed.Editable = false;

            LayerTurn =  DockerUI.corelApp.ActiveDocument.ActivePage.CreateLayer("Turn");
            LayerTurn.Editable = false;
        }

        private void gameLoop()
        {
            double x = 0;
            double y = 0;
            int shift = 0;
            int timeOut = 0;
            if (!endGame)
            {
                DockerUI.corelApp.ActiveDocument.GetUserClick(out x, out y, out shift, timeOut, false, cdrCursorShape.cdrCursorSmallcrosshair);
            }
            else
            {
                DockerUI.corelApp.ActiveDocument.Close();
                return;
            }
            if (Click(x, y, currentPlayer._PlayerType))
            {
                
                if (gameRunning)
                {
                    changeTurn();
                    
                }
               
            }
            if(!gameRunning)
                newGame();
            gameLoop();
                
        }
        private void changeTurn()
        {
            if (endGame || winner)
                return;
            if (numTurns > 3)
                checkWinner();
            if (currentPlayer == PlayerX)
                currentPlayer = PlayerO;
            else
                currentPlayer = PlayerX;
            if (DockerUI.corelApp.ActiveDocument == null)
                return;
            LayerTurn.Shapes.All().Delete();
            LayerTurn.CreateArtisticText(80, 192,string.Format("{0} Turn", currentPlayer._PlayerType), cdrTextLanguage.cdrLanguageNone, cdrTextCharSet.cdrCharSetMixed, null, 5);
            numTurns++;

            if (numTurns > 8)
            {
                gameRunning = false;
                LayerTurn.Shapes.All().Delete();
                LayerTurn.CreateArtisticText(80, 192, "Draw", cdrTextLanguage.cdrLanguageNone, cdrTextCharSet.cdrCharSetMixed, null, 5);
                double x = 0;
                double y = 0;
                int shift = 0;
                int timeOut = 0;
                DockerUI.corelApp.ActiveDocument.GetUserClick(out x, out y, out shift, timeOut, false, cdrCursorShape.cdrCursorSmallcrosshair);
                //gameLoop();
            }
           
        }
        private void checkWinner()
        {
            winner = false;
            if (gameSlots[0].Slot == gameSlots[1].Slot && gameSlots[2].Slot == gameSlots[0].Slot && gameSlots[0].Slot != SlotType.EMPTY)
            {
                if (gameSlots[0].Slot == SlotType.CIRCLE)
                    PlayerO.Score++;
                if (gameSlots[0].Slot == SlotType.CROSS)
                    PlayerX.Score++;
                LayerPlayed.CreateLineSegment(gameSlots[0].Left, gameSlots[0].CenterY, gameSlots[2].Right, gameSlots[0].CenterY);
                winner = true;
            }
            if (gameSlots[3].Slot == gameSlots[4].Slot && gameSlots[5].Slot == gameSlots[3].Slot && gameSlots[3].Slot != SlotType.EMPTY)
            {
                if (gameSlots[3].Slot == SlotType.CIRCLE)
                    PlayerO.Score++;
                if (gameSlots[3].Slot == SlotType.CROSS)
                    PlayerX.Score++;
                LayerPlayed.CreateLineSegment(gameSlots[3].Left, gameSlots[3].CenterY, gameSlots[5].Right, gameSlots[3].CenterY);
                winner = true;
            }
            if (gameSlots[6].Slot == gameSlots[7].Slot && gameSlots[8].Slot == gameSlots[6].Slot && gameSlots[6].Slot != SlotType.EMPTY)
            {
                if (gameSlots[6].Slot == SlotType.CIRCLE)
                    PlayerO.Score++;
                if (gameSlots[6].Slot == SlotType.CROSS)
                    PlayerX.Score++;
                LayerPlayed.CreateLineSegment(gameSlots[6].Left, gameSlots[6].CenterY, gameSlots[8].Right, gameSlots[6].CenterY);
                winner = true;
            }
            if (gameSlots[0].Slot == gameSlots[3].Slot && gameSlots[6].Slot == gameSlots[0].Slot && gameSlots[0].Slot != SlotType.EMPTY)
            {
                if (gameSlots[0].Slot == SlotType.CIRCLE)
                    PlayerO.Score++;
                if (gameSlots[0].Slot == SlotType.CROSS)
                    PlayerX.Score++;
                LayerPlayed.CreateLineSegment(gameSlots[0].CenterX, gameSlots[0].Bottom, gameSlots[6].CenterX, gameSlots[6].Top);
                winner = true;
            }
            if (gameSlots[1].Slot == gameSlots[4].Slot && gameSlots[7].Slot == gameSlots[1].Slot && gameSlots[1].Slot != SlotType.EMPTY)
            {
                if (gameSlots[1].Slot == SlotType.CIRCLE)
                    PlayerO.Score++;
                if (gameSlots[1].Slot == SlotType.CROSS)
                    PlayerX.Score++;
                LayerPlayed.CreateLineSegment(gameSlots[1].CenterX, gameSlots[1].Bottom, gameSlots[7].CenterX, gameSlots[7].Top);
                winner = true;
            }
            if (gameSlots[2].Slot == gameSlots[5].Slot && gameSlots[5].Slot == gameSlots[8].Slot && gameSlots[2].Slot != SlotType.EMPTY)
            {
                if (gameSlots[2].Slot == SlotType.CIRCLE)
                    PlayerO.Score++;
                if (gameSlots[2].Slot == SlotType.CROSS)
                    PlayerX.Score++;
                LayerPlayed.CreateLineSegment(gameSlots[2].CenterX, gameSlots[2].Bottom, gameSlots[8].CenterX, gameSlots[8].Top);
                winner = true;
            }
            if (gameSlots[0].Slot == gameSlots[4].Slot && gameSlots[8].Slot == gameSlots[0].Slot && gameSlots[0].Slot != SlotType.EMPTY)
            {
                if (gameSlots[0].Slot == SlotType.CIRCLE)
                    PlayerO.Score++;
                if (gameSlots[0].Slot == SlotType.CROSS)
                    PlayerX.Score++;
                LayerPlayed.CreateLineSegment(gameSlots[0].Left, gameSlots[0].Bottom, gameSlots[8].Right, gameSlots[8].Top);
                winner = true;
            }
            if (gameSlots[2].Slot == gameSlots[4].Slot && gameSlots[4].Slot == gameSlots[6].Slot && gameSlots[2].Slot != SlotType.EMPTY)
            {
                if (gameSlots[2].Slot == SlotType.CIRCLE)
                    PlayerO.Score++;
                if (gameSlots[2].Slot == SlotType.CROSS)
                    PlayerX.Score++;
                LayerPlayed.CreateLineSegment(gameSlots[2].Right, gameSlots[2].Bottom, gameSlots[6].Left, gameSlots[6].Top);
                winner = true;
            }

            if (winner)
            {
                score();
                LayerTurn.Shapes.All().Delete();
                LayerTurn.CreateArtisticText(80, 192, string.Format("{0} Winner", currentPlayer._PlayerType), cdrTextLanguage.cdrLanguageNone, cdrTextCharSet.cdrCharSetMixed, null, 5);
                double x = 0;
                double y = 0;
                int shift = 0;
                int timeOut = 0;
                DockerUI.corelApp.ActiveDocument.GetUserClick(out x, out y, out shift, timeOut, false, cdrCursorShape.cdrCursorSmallcrosshair);
                gameRunning = false;
                //gameLoop();
            }
        }
        private void newGame()
        {
            if (endGame)
                return;
            numTurns = 0;
            winner = false;
            LayerPlayed.Shapes.All().Delete();
            LayerTurn.Shapes.All().Delete();
            LayerTurn.CreateArtisticText(80, 192, string.Format("{0} Turn", currentPlayer._PlayerType), cdrTextLanguage.cdrLanguageNone, cdrTextCharSet.cdrCharSetMixed, null, 5);
            for (int i = 0; i < gameSlots.Length; i++)
            {
                gameSlots[i].Slot = SlotType.EMPTY;
            }
            gameRunning = true;
            //gameLoop();
        }
        private void score()
        {
            LayerScore.Shapes.All().Delete();
            LayerScore.CreateArtisticText(50, 222, PlayerX.Score.ToString(), cdrTextLanguage.cdrLanguageNone, cdrTextCharSet.cdrCharSetMixed, null, 5);
            LayerScore.CreateArtisticText(170, 222, PlayerO.Score.ToString(), cdrTextLanguage.cdrLanguageNone, cdrTextCharSet.cdrCharSetMixed, null, 5);
        }
    }
}
