using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Mine
{
    class Tile : Button
    {
        public int X;
        public int Y;
        public string Id;
        public bool IsMine;
        public int Hint = 0;
        public List<Tile> Neighbors = new List<Tile>();
        public Tile(int x, int y) : base()
        {
            this.X = x;
            this.Y = y;
            this.Id = $"({x},{y})";
            this.TabStop = false;
            this.Width = 20;
            this.Height = 20;
        }

        public static void GroupNeighbors(List<Tile> tiles)
        {
            foreach (Tile t in tiles)
            {
                for (int i = 0; i <= 7; i++)
                {
                    var nt = t.FindNeighbor(tiles, (TileNeighbor)i);
                    if (nt != null)
                    {
                        t.Neighbors.Add(nt);
                    }
                }    
            }
            
        }

        private Tile FindNeighbor(List<Tile> tiles, TileNeighbor tileNeighbor)
        {
            switch (tileNeighbor)
            {
                case TileNeighbor.TL:
                    return tiles.Find(t => t.X == this.X - 1 && t.Y == this.Y - 1);
                case TileNeighbor.TC:
                    return tiles.Find(t => t.X == this.X && t.Y == this.Y - 1);
                case TileNeighbor.TR:
                    return tiles.Find(t => t.X == this.X + 1 && t.Y == this.Y - 1);
                case TileNeighbor.L:
                    return tiles.Find(t => t.X == this.X - 1 && t.Y == this.Y);
                case TileNeighbor.R:
                    return tiles.Find(t => t.X == this.X + 1 && t.Y == this.Y);
                case TileNeighbor.BL:
                    return tiles.Find(t => t.X == this.X - 1 && t.Y == this.Y + 1);
                case TileNeighbor.BC:
                    return tiles.Find(t => t.X == this.X && t.Y == this.Y + 1);
                case TileNeighbor.BR:
                    return tiles.Find(t => t.X == this.X + 1 && t.Y == this.Y + 1);
            }
            return null;
        }
    }

    enum TileNeighbor
    {
        TL,
        TC,
        TR,
        L,
        R,
        BL,
        BC,
        BR
    }
}
