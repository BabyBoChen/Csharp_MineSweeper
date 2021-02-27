using System;
using System.Collections.Generic;

namespace Mine
{
    class MineGen
    {
        public static void Generate(List<Tile> tiles, int mine_count)
        {
            var RN = new Random();
            var drawLots = new List<int>();
            for (int i = 0; i < tiles.Count; i++)
            {
                drawLots.Add(i);
            }
            for (int j = 0; j < mine_count; j++)
            {
                var rn = RN.Next(0, drawLots.Count);
                tiles[rn].IsMine = true;
                //tiles[rn].Text = "X";
                drawLots.RemoveAt(rn);
            }
            foreach (Tile tile in tiles)
            {
                var h = 0;
                foreach (Tile t in tile.Neighbors)
                {
                    if (t.IsMine == true) h++;
                }
                tile.Hint = h;
            }
        }
    }
}
