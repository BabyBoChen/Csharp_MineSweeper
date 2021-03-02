using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mine
{
    class MainWindow : Form
    {
        public int grid_size = 5;
        public List<Tile> tiles = new List<Tile>();
        public MainWindow()
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            TableLayoutPanel grid = new TableLayoutPanel();
            grid.AutoSize = true;
            grid.AutoSizeMode = AutoSizeMode.GrowOnly;
            grid.ColumnCount = grid_size;
            grid.RowCount = grid_size;
            for (int r = 0; r < grid.RowCount; r++)
            {
                for (int c = 0; c < grid.ColumnCount;c++)
                {
                    var tile = new Tile(c,r);
                    grid.Controls.Add(tile,c,r);
                    tile.Click += (a, b) =>
                    {
                        this.OnTileClick(tile);
                    };
                    this.tiles.Add(tile);
                }
            }
            grid.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            grid.Dock = DockStyle.Fill;
            this.Controls.Add(grid);
        }

        public void OnTileClick(Tile tile)
        {
            tile.Enabled = false;
            if (GameFinished())
            {
                Console.WriteLine("Game Finished!");
            }
        }

        public bool GameFinished()
        {
            var gameover = true;
            foreach (Tile t in tiles)
            {
                if (t.Enabled == true)
                {
                    gameover = false;
                    break;
                }
            }
            return gameover;
        }
    }
}
