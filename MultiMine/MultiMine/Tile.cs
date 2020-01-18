using MultiMine.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MultiMine
{
    class Tile
    {
        private Grid grid;
        public System.Drawing.Image image { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        private static readonly int[][] adjacentCoords =
                {
                    new[] {-1, -1}, new[] {0, -1}, new[] {1, -1}, new[] {1, 0}, new[] {1, 1}, new[] {0, 1},
                    new[] {-1, 1}, new[] {-1, 0}
                };

        private bool flagged;
        public bool Flagged
        {
            get => this.flagged;
            set
            {
                this.flagged = value;
                this.image = byteArrayToImage(value ? Resources.Resource.Flag : Resources.Resource.Tile);
            }
        }

        public Tile[] AdjacentTiles { get; private set; }
        public System.Drawing.Point GridPosition { get; }
        public bool Opened { get; private set; }
        public bool Mined { get; private set; }
        private int AdjacentMines => this.AdjacentTiles.Count(tile => tile.Mined);

        public Tile(Grid grid)
        {
            this.grid = grid;
        }

        internal void SetAdjacentTiles()
        {
            List<Tile> adjacentTiles = new List<Tile>(8);
            foreach (int[] adjacentCoord in adjacentCoords)
            {
                //Tile tile = this.grid[new System.Drawing.Point(this.GridPosition.X + adjacentCoord[0], this.GridPosition.Y + adjacentCoord[1]);
                
                if (tile != null)
                {
                    adjacentTiles.Add(tile);
                }
            }
           
            this.AdjacentTiles = adjacentTiles.ToArray();
        }

        public System.Drawing.Image byteArrayToImage(byte[] bytesArr)
        {
            using (MemoryStream memstr = new MemoryStream(bytesArr))
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(memstr);
                return img;
            }
        }
    }
}
