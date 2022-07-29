using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RASDK.Basic;

namespace PerlerBeadsHandler
{
    public static class PegBoardFactory
    {
        public static Pegboard Make(string csvFile, out string info)
        {
            var board = new Pegboard();
            int b = 0;
            int w = 0;
            int g = 0;
            int y = 0;
            int o = 0;
            int n = 0;

            var csv = Csv.Read(csvFile);
            for (int row = 0; row < csv.Count; row++)
            {
                for (int col = 0; col < csv[0].Count; col++)
                {
                    var value = csv[row][col];
                    var grid = new Point(row, col);

                    switch (value)
                    {
                        case "B":
                            board.PutBead(grid, Color.Black, true);
                            b++;
                            break;

                        case "W":
                            board.PutBead(grid, Color.White, true);
                            w++;
                            break;

                        case "G":
                            board.PutBead(grid, Color.Green, true);
                            g++;
                            break;

                        case "Y":
                            board.PutBead(grid, Color.Yellow, true);
                            y++;
                            break;

                        case "O":
                            board.PutBead(grid, Color.Orange, true);
                            o++;
                            break;

                        default:
                            board.RemoveBead(grid);
                            n++;
                            break;
                    }
                }
            }

            info = $"[Pegboard Beads Count]\r\n" +
                   $" Black: {b}\r\n" +
                   $" White: {w}\r\n" +
                   $" Green: {g}\r\n" +
                   $"Yellow: {y}\r\n" +
                   $"Orange: {o}\r\n" +
                   $" Empty: {n}";
            return board;
        }
    }
}