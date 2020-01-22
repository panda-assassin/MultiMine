using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMine.Model {

    public enum GameStatus {
        InProgress,
        Failed,
        Completed
    }
    class GameBoard {
        public int Width { get; set; }
        public int Height { get; set; }
        public int MineCount { get; set; }
        public List<Tile> Tiles { get; set; }
        public List<Tile> FlagTiles { get; set; }
        public GameStatus Status { get; set; }
        public Boolean firstMove { get; set; }


        public GameBoard(int width, int height, int mines)
        {

            this.Width = width;
            this.Height = height;
            this.MineCount = mines;
            this.Tiles = new List<Tile>();
            this.FlagTiles = new List<Tile>();
            this.firstMove = true;

            int id = 1;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Tiles.Add(new Tile(id, j, i));
                    id++;
                }
            }

            Status = GameStatus.InProgress; //Let's start the game!
        }

        public void FirstMove(int x, int y)
        {

            /*getTile(x, y).isRevealed = true;
            getTile(x, y).imagePath = "../../Resources/EmptyTile_0.png";*/

            List<Tile> mineList = new List<Tile>();
            //Adding Tiles to the minelist so no mines will spawn here
            mineList.Add(getTile(x, y));
            if (getTile(x - 1, y - 1) != null)
            {
                mineList.Add(getTile(x - 1, y - 1));
            }
            if (getTile(x - 1, y) != null)
            {
                mineList.Add(getTile(x - 1, y));
            }
            if (getTile(x - 1, y + 1) != null)
            {
                mineList.Add(getTile(x - 1, y + 1));
            }
            if (getTile(x, y + 1) != null)
            {
                mineList.Add(getTile(x, y + 1));
            }
            if (getTile(x, y - 1) != null)
            {
                mineList.Add(getTile(x, y - 1));
            }
            if (getTile(x + 1, y - 1) != null)
            {
                mineList.Add(getTile(x + 1, y - 1));
            }
            if (getTile(x + 1, y) != null)
            {
                mineList.Add(getTile(x + 1, y));
            }
            if (getTile(x + 1, y + 1) != null)
            {
                mineList.Add(getTile(x + 1, y + 1));
            }
            
            Random rand = new Random();
            for (int i = 0; i < MineCount; i++)
            {

                Tile stubTile = getTile(rand.Next(1, Tiles.Count));
                if (Tiles.Except(mineList).Contains(stubTile))
                {
                    mineList.Add(stubTile);
                    stubTile.isMine = true;
                   // getTile(rand.Next(1, Tiles.Count)).imagePath = "../../Resources/Mine.png"; //uncomment to show all mines
                } else
                {
                    i--;
                }
                
            }

            foreach (Tile tile in Tiles)
            {
                tile.adjacentMines = getNumberOfNeighourMines(tile.x, tile.y);
            }

            RevealPanel(x, y);

            this.firstMove = false;
        }

        public void RevealPanel(int x, int y)
        {
            //step 1: check if Tile is mine
            if (getTile(x, y).isMine)
            {
                Status = GameStatus.Failed;
                showAllTiles();
                getTile(x, y).imagePath = "../../Resources/Mine.png";
                return;
            }

            //step 2: reveal tile
            getTile(x, y).isRevealed = true;
            getTile(x, y).imagePath = "../../Resources/EmptyTile_" + getTile(x, y).adjacentMines + ".png";

            //step 3: reveal neighbouring tiles if clicked tile is empty
            if (getTile(x, y).adjacentMines == 0)
            {
                if (getTile(x - 1, y - 1) != null && !getTile(x - 1, y - 1).isRevealed)
                {
                    RevealPanel(x - 1, y - 1);
                }
                if (getTile(x - 1, y) != null && !getTile(x - 1, y).isRevealed)
                {
                    RevealPanel(x - 1, y);
                }
                if (getTile(x - 1, y + 1) != null && !getTile(x - 1, y + 1).isRevealed)
                {
                    RevealPanel(x - 1, y + 1);
                }
                if (getTile(x, y + 1) != null && !getTile(x, y + 1).isRevealed)
                {
                    RevealPanel(x, y + 1);
                }
                if (getTile(x, y - 1) != null && !getTile(x, y - 1).isRevealed)
                {
                    RevealPanel(x, y - 1);
                }
                if (getTile(x + 1, y - 1) != null && !getTile(x + 1, y - 1).isRevealed)
                {
                    RevealPanel(x + 1, y - 1);
                }
                if (getTile(x + 1, y) != null && !getTile(x + 1, y).isRevealed)
                {
                    RevealPanel(x + 1, y);
                }
                if (getTile(x + 1, y + 1) != null && !getTile(x + 1, y + 1).isRevealed)
                {
                    RevealPanel(x + 1, y + 1);
                }
            }

            //step 4: check if game is completed
            if (Tiles.Except(FlagTiles).All(tile => tile.isRevealed))
            {
                Status = GameStatus.Completed;
            }
        }

        public void FlagPanel(int x, int y)
        {
            
            getTile(x, y).isFlagged = !getTile(x, y).isFlagged;
            if (getTile(x, y).isFlagged && !getTile(x, y).isRevealed)
            {
                FlagTiles.Add(getTile(x, y));
                getTile(x,y).imagePath = "../../Resources/Flag.png";
            } else
            {
                FlagTiles.Remove(getTile(x, y));
                getTile(x, y).imagePath = "../../Resources/Tile.png";
            }
        }

        private Tile getTile(int x, int y)
        {
            int ID = Tiles.FindIndex(tile => tile.x == x && tile.y == y) + 1;
            Tile stubTile = getTile(ID);
            if (Tiles.Contains(stubTile))
            {
                return Tiles.First(tile => tile.x == x && tile.y == y);
            } else
            {
                return null;
            }
        }

        private Tile getTile(int ID)
        {
            if (ID <= Tiles.Count && ID > 0)
            {
                return Tiles.First(tile => tile.iD == ID);
            }
            else return null;
        }

        private int getNumberOfNeighourMines(int x, int y)
        {
            //Setting the neighouring tiles;
            List<Tile> neighbours = new List<Tile>();
            if (getTile(x - 1, y - 1) != null)
            {
                neighbours.Add(getTile(x - 1, y - 1));
            }
            if (getTile(x - 1, y) != null)
            {
                neighbours.Add(getTile(x - 1, y));
            }
            if (getTile(x - 1, y + 1) != null)
            {
                neighbours.Add(getTile(x - 1, y + 1));
            }
            if (getTile(x, y + 1) != null)
            {
                neighbours.Add(getTile(x, y + 1));
            }
            if (getTile(x, y - 1) != null)
            {
                neighbours.Add(getTile(x, y - 1));
            }
            if (getTile(x + 1, y - 1) != null)
            {
                neighbours.Add(getTile(x + 1, y - 1));
            }
            if (getTile(x + 1, y) != null)
            {
                neighbours.Add(getTile(x + 1, y));
            }
            if (getTile(x + 1, y + 1) != null)
            {
                neighbours.Add(getTile(x + 1, y + 1));
            }

            int counter = 0;

            foreach (Tile tile in neighbours)
            {
                if (tile.isMine)
                {
                    counter++;
                }
            }
            return counter;
        }

        private void showAllTiles()
        {
            foreach (Tile tile in Tiles.Except(FlagTiles))
            {
                if (tile.isMine)
                {
                    tile.isRevealed = true;
                    tile.imagePath = "../../Resources/Mine.png";
                } else
                {
                    tile.isRevealed = true;
                    tile.imagePath = "../../Resources/EmptyTile_" +  tile.adjacentMines + ".png";
                }
            }
        }
    }
}
