using System;
using System.Windows.Forms;

namespace Mine
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            var size = 10;
            var mineCount = 10;
            MainWindow game;
            do
            {
                game = new MainWindow(size, mineCount);
                Application.Run(game);
                size = game.GridSize;
                mineCount = game.MineCount;
                if (game.Won == true && size + 5 <= 20)
                {
                    size += 5;
                    mineCount *= 2;
                }

            } while (game.NextGame);
        }
    }
}
