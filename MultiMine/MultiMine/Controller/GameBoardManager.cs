using MultiMine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMine.Controller {
    class GameBoardManager {

        private GameBoard gameBoard;
        private GameBoardListener listener;

        public GameBoardManager(GameBoard gameBoard, GameBoardListener listener)
        {
            this.gameBoard = gameBoard;
            this.listener = listener;
        }

        public void onRightClick(int x, int y)
        {
            if (gameBoard.firstMove)
            {
                gameBoard.FirstMove(x,y);
                listener.gameBoardUpdated();
            } else
            {
                gameBoard.RevealPanel(x, y);
                listener.gameBoardUpdated();
            }
        }

        public void onLeftClick(int x, int y)
        {
            if (gameBoard.firstMove)
            {
                gameBoard.FirstMove(x, y);
                listener.gameBoardUpdated();
            }
            else
            {
                gameBoard.FlagPanel(x, y);
                listener.gameBoardUpdated();
            }
        }

        public GameBoard getGameBoard()
        {
            return this.gameBoard;
        }
    }
}
