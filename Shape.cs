using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SchetsEditor
{
    [Serializable]
    public abstract class Shape
    {
        public static int PenWidth = 4;
        public Color c;

        public Point p1;
        public Point p2;
        public abstract void teken(Graphics g);
    
    }
    [Serializable]
    public class TweePuntsShape : Shape
    {
        public TweePuntsShape(Color c, Point p1, Point p2)
        {
            this.c = c;
            this.p1 = p1;
            this.p2 = p2;
        }

        public override void teken(Graphics g) { }
    }
    [Serializable]
    public class RechthoekShape : TweePuntsShape
    {

        public RechthoekShape(Color c, Rectangle rect) : base(c, rect.Location, new Point(rect.Width, rect.Height)){}
        public override void teken(Graphics g)
        {
            Pen pen = new Pen(c, PenWidth);
            g.DrawRectangle(pen, p1.X, p1.Y, p2.X, p2.Y);
        }

        public override string ToString()
        {
            string s = "rechthoek";
            return s;
        }
    }
    [Serializable]
    public class CirkelShape : TweePuntsShape
    {
        public CirkelShape(Color c, Rectangle rect) : base(c, rect.Location, new Point(rect.Width, rect.Height)) { }

        public override void teken(Graphics g)
        {
            Pen pen = new Pen(c, PenWidth);
            g.DrawEllipse(pen, p1.X, p1.Y, p2.X, p2.Y);
        }

        public override string ToString()
        {
            string s = "cirkel";
            return s;
        }
    }

    [Serializable]
    public class VolRechthoekShape : TweePuntsShape
    {
        public VolRechthoekShape(Color c, Rectangle rect) : base(c, rect.Location, new Point(rect.Width, rect.Height)) { }

        public override void teken(Graphics g)
        {
            SolidBrush brush = new SolidBrush(c);
            g.FillRectangle(brush, p1.X, p1.Y, p2.X, p2.Y);
        }

        public override string ToString()
        {
            string s = "volrechthoek";
            return s;
        }
    }

    [Serializable]
    public class VolCirkelShape : TweePuntsShape
    {
        public VolCirkelShape(Color c, Rectangle rect) : base(c, rect.Location, new Point(rect.Width, rect.Height)) { }

        public override void teken(Graphics g)
        {
            SolidBrush brush = new SolidBrush(c);
            g.FillEllipse(brush, p1.X, p1.Y, p2.X, p2.Y);

        }

        public override string ToString()
        {
            string s = "volcirkel";
            return s;
        }
    }

    [Serializable]
    public class TekstShape : Shape
    {
        public Font Font { get; }
        public string Tekst { get; set; }
        public SizeF Size { get; private set; }
        public TekstShape(Color c, Point p1, string _tekst)
        {
            Font = new Font("Tahoma", 40);
            this.c = c;
            this.p1 = p1;
            this.Tekst = _tekst;
        }

        public override void teken(Graphics g)
        {
            Size = g.MeasureString(Tekst, Font, p1, StringFormat.GenericTypographic);
            g.DrawString(Tekst, Font, new SolidBrush(c), p1.X, p1.Y);

        }

        public override string ToString()
        {
            string s = "tekst";
            return s;
        }
    }
    //werkt niet
    [Serializable]
    public class LijnShape : TweePuntsShape
    {
        public LijnShape(Color c, Point p1, Point p2) : base(c, p1, p2) { }

        public override void teken(Graphics g)
        {
            Pen pen = new Pen(c, PenWidth);
            g.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);
        }

        public override string ToString()
        {
            string s = "lijn";
            return s;
        }
    }
}

