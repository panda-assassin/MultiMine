using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMine.Model
{

    public enum GameStatus
    {
        InProgress,
        Failed,
        Completed
    }
    class GameBoard
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int MineCount { get; set; }
        public List<Tile> Tiles { get; set; }
        public GameStatus Status { get; set; }


        public GameBoard(int width, int height, int mines)
        {

            this.Width = width;
            this.Height = height;
            this.MineCount = mines;
            this.Tiles = new List<Tile>();

            int id = 1;
            for (int i = 1; i <= height; i++)
            {
                for (int j = 1; j <= width; j++)
                {
                    Tiles.Add(new Tile(id, j, i));
                    id++;
                }
            }

            Status = GameStatus.InProgress; //Let's start the game!
        }

        public void FirstMove(int x, int y, Random rand)
        {
            //For any board, take the user's first revealed tile + any neighbors of that tile to X depth, and mark them as unavailable for mine placement.
            var depth = 0.125 * Width; //12.5% (1/8th) of the board width becomes the depth of unavailable panels
            var neighbors = GetNeighbors(x, y, (int)depth); //Get all neighbors to specified depth
            neighbors.Add(getTiles(x, y)); //Don't place a mine in the user's first move!

            //Select random tiles from set of panels which are not excluded by the first-move rule
            var mineList = Tiles.Except(neighbors).OrderBy(user => rand.Next());
            var mineSlots = mineList.Take(MineCount).ToList().Select(z => new { z.x, z.y });

            //Place the mines
            foreach (var mineCoord in mineSlots)
            {
                Tiles.Single(tile => tile.x == mineCoord.x && tile.y == mineCoord.y).isMine = true;
            }

            //For every panel which is not a mine, determine and save the adjacent mines.
            foreach (var openTile in Tiles.Where(tile => !tile.isMine))
            {
                var nearbyTiles = GetNeighbors(openTile.x, openTile.y);
                openTile.adjacentMines = nearbyTiles.Count(z => z.isMine);
            }
        }

        public Tile getTiles(int x, int y)
        {
            return Tiles.First(z => z.x == x && z.y == y);
        }

        public List<Tile> GetNeighbors(int x, int y)
        {
            return GetNeighbors(x, y, 1);
        }

        public List<Tile> GetNeighbors(int x, int y, int depth)
        {
            var nearbyPanels = Tiles.Where(tile => tile.x >= (x - depth) && tile.x <= (x + depth)
                                                    && tile.y >= (y - depth) && tile.y <= (y + depth));
            var currentPanel = Tiles.Where(tile => tile.x == x && tile.y == y);
            return nearbyPanels.Except(currentPanel).ToList();
        }

        public void RevealPanel(int x, int y)
        {
            //Step 1: Find the Specified Panel
            var selectedTile = Tiles.First(tile => tile.x == x && tile.y == y);
            selectedTile.isRevealed = true;
            selectedTile.isFlagged = false; //Revealed panels cannot be flagged

            //Step 2: If the panel is a mine, game over!
            if (selectedTile.isMine) Status = GameStatus.Failed;

            //Step 3: If the panel is a zero, cascade reveal neighbors
            if (!selectedTile.isMine && selectedTile.adjacentMines == 0)
            {
                RevealZeros(x, y);
            }

            //Step 4: If this move caused the game to be complete, mark it as such
            if (!selectedTile.isMine)
            {
                CompletionCheck();
            }
        }

        public void RevealZeros(int x, int y)
        {
            var neighbourTiles = GetNeighbors(x, y).Where(tile => !tile.isRevealed);
            foreach (var neighbor in neighbourTiles)
            {
                neighbor.isRevealed = true;
                if (neighbor.adjacentMines == 0)
                {
                    RevealZeros(neighbor.x, neighbor.y);
                }
            }
        }

        public void FlagPanel(int x, int y)
        {
            var tile = Tiles.Where(z => z.x == x && z.y == y).First();
            if (!tile.isRevealed)
            {
                tile.isFlagged = true;
            }
        }

        private void CompletionCheck()
        {
            var hiddenTiles = Tiles.Where(x => !x.isRevealed).Select(x => x.iD);
            var mineTiles = Tiles.Where(x => x.isMine).Select(x => x.iD);
            if (!hiddenTiles.Except(mineTiles).Any())
            {
                Status = GameStatus.Completed;
            }
        }
    }
}
