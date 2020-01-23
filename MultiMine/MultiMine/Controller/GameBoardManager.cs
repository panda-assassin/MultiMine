using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMine.Controller {
    class GameBoardManager {

        private static GameBoardManager instance;

        private GameBoard gameBoard;
        private GameBoardListener listener;

        private GameBoardManager()
        {
            
        }

        public static GameBoardManager GetInstance()
        {
            if (instance == null)
            {
                instance = new GameBoardManager();
            }
            return instance;
        }

        public void setListener(GameBoardListener listener)
        {
            if (instance != null)
            {
                this.listener = listener;
            }
        }

        public void setGameBoard(GameBoard gameBoard)
        {
            if (instance != null)
            {
                this.gameBoard = gameBoard;
                listener.gameBoardUpdated();
            }
        }

        public void onRightClick(int x, int y)
        {
            gameBoard.FlagPanel(x, y);
            Connector.GetInstance().sendGameBoard(gameBoard);

        }

        public void onLeftClick(int x, int y)
        {
            if (gameBoard.firstMove)
            {
                gameBoard.FirstMove(x, y);
                Connector.GetInstance().sendGameBoard(gameBoard);
                
            }
            else
            {
                gameBoard.RevealPanel(x, y);
                Connector.GetInstance().sendGameBoard(gameBoard);
            }
        }

        public GameBoard getGameBoard()
        {
            return this.gameBoard;
        }
    }
}
