using System;
using System.Windows.Forms;

namespace Mine
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            MainWindow game = new MainWindow(10,10);
            Application.EnableVisualStyles();
            Application.Run(game);
            while (game.NextGame)
            {
                var size = game.GridSize;
                var mineCount = game.MineCount;
                if (game.Won == true && size + 5 <= 20)
                {
                    size += 5;
                    mineCount *= 2;
                }
                game = new MainWindow(size, mineCount);
                Application.Run(game);
            }
        }
    }
}
