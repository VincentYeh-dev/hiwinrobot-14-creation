using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerlerBeadsHandler
{
    public class Bead
    {
        public Color Color;

        public Bead()
        {
            Color = Color.Black;
        }

        public Bead(Color color)
        {
            Color = color;
        }
    }
}