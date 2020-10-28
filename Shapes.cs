using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SchetsEditor
{
    public abstract class Shapes
    {
    
        public Color c;

        public Point p1;
        public Point p2;

        public abstract void teken(Graphics g);
    
    }

    public class Rechthoek : Shapes
    {
        
        public Rechthoek(Color c, Point p1, Point p2)
        {
            this.c = c;
            this.p1 = p1;
            this.p2 = p2;
        }

        public override void teken(Graphics g)
        {
            Pen pen = new Pen(c, 4);
            g.DrawRectangle(pen, p1.X, p1.Y, p2.X, p2.Y);
        }
    }

    public class Cirkel : Shapes
    {
        public Cirkel(Color c, Point p1, Point p2)
        {
            this.c = c;
            this.p1 = p1;
            this.p2 = p2;
        }
        public override void teken(Graphics g)
        {
            Pen pen = new Pen(c, 4);
            g.DrawEllipse(pen, p1.X, p1.Y, p2.X, p2.Y);
        }
    }

    public class VolRechthoek : Shapes
    {

        
        public override void teken(Graphics g)
        {

        }
    }

    public class VolCirkel : Shapes
    {
        public VolCirkel(Color c, Point p1, Point p2)
        {
            this.c = c;
            this.p1 = p1;
            this.p2 = p2;
        }
        public override void teken(Graphics g)
        {
            
        }
    }
}

