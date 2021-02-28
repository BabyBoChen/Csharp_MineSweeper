using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mine
{
    class MainWindow : Form
    {
        private delegate void Del(object sender, EventArgs e);  //not quite understand why this delegate is necessary to avoid thread conflicting

        public int GridSize { get; set; }  //size of the grid
        public int MineCount { get; set; }  //mine count
        private bool win = false;  //will be true if you win the game
        public bool Won { get {return this.win;}}
        private List<Tile> tiles = new List<Tile>();  //all tile will be added to this list of tiles right after they are created.
        public bool AutoSweep { get; set; }  //if true, neighbor tiles will be swept automatically if they are not near by any mine
        public bool CheatMode { get; set; }  // if true, reveal all the mines
        private bool nextGame;
        public bool NextGame { get { return this.nextGame; } }
        private StopWatch sw;  //a stopwatch measuring the time elapsed during each game
        private Label LblTime; // the string telling the time on the right side of the MainWindow
        public MainWindow(int gridSize, int mineCount)
        {

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            this.Text = "Mine Sweeper";
            this.GridSize = gridSize;
            this.MineCount = mineCount;
            this.AutoSweep = false;
            this.CheatMode = false;

            TableLayoutPanel grid = new TableLayoutPanel();
            grid.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            grid.Dock = DockStyle.Fill;
            grid.AutoSize = true;
            grid.AutoSizeMode = AutoSizeMode.GrowOnly;
            grid.ColumnCount = this.GridSize + 1;  // "+1" for the right-side panel (autosweep button, cheatmode button and time label)
            grid.RowCount = this.GridSize;

            // create tiles according to the given gridSize
            for (int r = 0; r < grid.RowCount; r++)
            {
                for (int c = 0; c < grid.ColumnCount-1; c++)
                {
                    var tile = new Tile(c, r);
                    grid.Controls.Add(tile, c, r); // add each tile at the designated coordination of the grid
                    
                    // add eventhandler to each tile
                    tile.Click += (a, b) =>
                    {
                        this.OnTileClick(tile); // This method was defined down below.
                    };
                    this.tiles.Add(tile); // add each tile to List<Tile> tiles
                }
            }
            Tile.GroupNeighbors(this.tiles);  //calculate each tile's neighbor tiles (Tile.cs)
            MineGen.Generate(tiles,this.MineCount);  //bury mines at randomly selected tiles (MineGen.cs)

            //right side panel (autosweep button, cheatmode button and time label)
            var menuPanel1 = new Panel();
            var lblAutoSweep = new Label();
            lblAutoSweep.Text = "Auto Sweep:";
            lblAutoSweep.Width = 70;
            lblAutoSweep.Dock = DockStyle.Left;
            var btnAutoSweep = new Button();
            btnAutoSweep.Text = $"{this.AutoSweep}";
            btnAutoSweep.Width = 50;
            btnAutoSweep.Click += (a, b) =>
            {
                this.AutoSweep = (!this.AutoSweep);
                btnAutoSweep.Text = $"{this.AutoSweep}";
            };
            btnAutoSweep.Dock = DockStyle.Left;
            menuPanel1.Controls.Add(btnAutoSweep);
            menuPanel1.Controls.Add(lblAutoSweep);
            menuPanel1.Height = 20;
            menuPanel1.Width = 130;

            grid.Controls.Add(menuPanel1, grid.ColumnCount, 0);

            var menuPanel2 = new Panel();
            var lblCheatMode = new Label();
            lblCheatMode.Text = "Cheat Mode: ";
            lblCheatMode.Width = 70;
            lblCheatMode.Dock = DockStyle.Left;
            var btnCheatMode = new Button();
            btnCheatMode.Text = $"{this.CheatMode}";
            btnCheatMode.Width = 50;
            btnCheatMode.Click += (a, b) =>
            {
                this.CheatMode = (!this.CheatMode);
                btnCheatMode.Text = $"{this.CheatMode}";
                foreach (Tile tile in this.tiles)
                {
                    if (tile.IsMine == true)
                    {
                        if (this.CheatMode)
                        {
                            tile.Text = "X";
                        }
                        else
                        {
                            tile.Text = " ";
                        }
                    }
                }
            };
            btnCheatMode.Dock = DockStyle.Left;
            menuPanel2.Controls.Add(btnCheatMode);
            menuPanel2.Controls.Add(lblCheatMode);
            menuPanel2.Height = 20;
            menuPanel2.Width = 130;

            grid.Controls.Add(menuPanel2, grid.ColumnCount, 1);

            this.sw = new StopWatch();
            this.sw.SendTime += this.GetTime;
            this.LblTime = new Label();
            this.LblTime.Text = "00:00:00";

            grid.Controls.Add(this.LblTime, grid.ColumnCount, 2);

            this.Controls.Add(grid); //add the grid to the MainWindow

            this.nextGame = false;

        }

        public void OnTileClick(Tile tile)
        {
            if (Lose(tile))
            {
                this.win = false;
                this.sw.Ticking = false;
                var rePlay = MessageBox.Show("Play Again?", "You Lose!",MessageBoxButtons.YesNo);
                this.Close();
                if (rePlay.ToString() == "Yes") this.nextGame = true;
                return;
            }
            this.Sweep(tile);
            if (Win())
            {
                this.win = true;
                this.sw.Ticking = false;
                var rePlay = MessageBox.Show("Play Again?", "You Win!", MessageBoxButtons.YesNo);
                this.Close();
                if (rePlay.ToString() == "Yes") this.nextGame = true;
                return;
            }
        }

        private void Sweep(Tile tile)
        {
            tile.Text = $"{tile.Hint}";
            tile.Enabled = false;
            if (this.AutoSweep == true)
            {
                if (tile.Hint == 0)
                {
                    foreach (Tile t in tile.Neighbors)
                    {
                        if (t.Enabled == true)
                        {
                            this.Sweep(t);
                        }
                    }
                }
            }

        }
        public void GetTime(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                Del d = new Del(GetTime);
                this.Invoke(d,new Object[] {sender,e});
            }
            else
            {
                var a = (StopWatch)sender;
                this.LblTime.Text = a.Ts.ToString(@"hh\:mm\:ss");
            }
            
        }

        public bool Lose(Tile tile)
        {
            if (tile.IsMine == true)
            {
                foreach (Tile t in this.tiles)
                {
                    t.Enabled = false;
                    if (t.IsMine == true)
                    {
                        t.Text = "X";
                    }
                    else
                    {
                        t.Text = $"{t.Hint}";
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Win()
        {
            var win = true;
            foreach (Tile t in this.tiles)
            {
                if (t.Enabled == true && t.IsMine == false)
                {
                    win = false;
                    break;
                }
            }
            if(win)
            foreach (Tile t in this.tiles)
            {
                t.Enabled = false;
                if (t.IsMine == true)
                {
                    t.Text = "X";
                }
                else
                {
                    t.Text = $"{t.Hint}";
                }
            }
            return win;
        }
    }
}
