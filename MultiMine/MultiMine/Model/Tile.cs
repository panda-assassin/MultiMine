using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMine
{
    class Tile
    {
        public int iD { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public bool isMine { get; set; }
        public int adjacentMines { get; set; }
        public bool isRevealed { get; set; }
        public bool isFlagged { get; set; }
        public String imagePath { get; set; }

        public Tile(int iD, int x, int y)
        {
            this.iD = iD;
            this.x = x;
            this.y = y;
            this.imagePath = "../../Resources/Tile.png";
        }
    }
}
