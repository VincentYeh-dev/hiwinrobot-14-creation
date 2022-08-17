using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerlerBeads
{
    public class Pegboard
    {
        public const float X_GridLength = 5f;
        public const float Y_GridLength = 5f;

        public readonly Size Size;

        public PointF Position = new PointF(0, 0);

        private readonly Bead[,] _beads;

        public Pegboard()
        {
            // Default size of pegboard is 29*29.
            Size = new Size(29, 29);
            _beads = new Bead[Size.Width, Size.Height];
        }

        public Pegboard(Size size)
        {
            Size = size;
            _beads = new Bead[Size.Width, Size.Height];
        }

        public Bead[,] Beads => _beads;

        public void PutBead(Point grid, Bead bead, bool forceReplaceBead = false)
        {
            if (grid.X > _beads.GetLength(0) || grid.Y > _beads.GetLength(1))
            {
                throw new IndexOutOfRangeException();
            }

            if (IsAlreadyExistsBead(grid) && !forceReplaceBead)
            {
                throw new Exception("此位置已經有東西了。");
            }

            _beads[grid.X, grid.Y] = bead;
        }

        public bool SearchBy(Color color, out Point grid)
        {
            for (int x = 0; x < Size.Width; x++)
            {
                for (int y = 0; y < Size.Height; y++)
                {
                    var bead = _beads[x, y];
                    if (bead == null)
                    {
                        continue;
                    }

                    if (bead.Color == color)
                    {
                        grid = new Point(x, y);
                        return true;
                    }
                }
            }

            // Not find.
            grid = new Point(-1, -1);
            return false;
        }

        public void PutBead(Point grid, Color color, bool forceReplaceBead = false)
        {
            var bead = new Bead(color);
            PutBead(grid, bead, forceReplaceBead);
        }

        public void RemoveBead(Point grid)
        {
            _beads[grid.X, grid.Y] = null;
        }

        public Bead GetBead(Point grid)
        {
            return _beads[grid.X, grid.Y];
        }

        public Bead GetBead(int x, int y)
        {
            return _beads[x, y];
        }

        public PointF GetRealPosition(Point grid)
        {
            return GetRealPosition(grid, out _);
        }

        public bool HasBead(Point grid)
        {
            return GetBead(grid) != null;
        }

        public PointF GetRealPosition(Point grid, out Bead bead)
        {
            var realGrid = new PointF(X_GridLength* grid.X, Y_GridLength * grid.Y);
            var realPosition = Position;
            realPosition.X += realGrid.X;
            realPosition.Y -= realGrid.Y;

            bead = GetBead(grid);
            return realPosition;
        }

        public bool IsAlreadyExistsBead(Point grid)
        {
            return GetBead(grid) != null;
        }
    }
}