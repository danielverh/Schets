using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace SchetsEditor
{
    public interface ISchetsTool
    {
        void MuisVast(SchetsControl s, Point p);
        void MuisDrag(SchetsControl s, Point p);
        void MuisLos(SchetsControl s, Point p);
        void Letter(SchetsControl s, char c);
    }

    public abstract class StartpuntTool : ISchetsTool
    {
        protected Point startpunt;
        protected Brush kwast;

        public virtual void MuisVast(SchetsControl s, Point p)
        {
            startpunt = p;
        }
        public virtual void MuisLos(SchetsControl s, Point p)
        {
            kwast = new SolidBrush(s.PenKleur);
        }
        public abstract void MuisDrag(SchetsControl s, Point p);
        public abstract void Letter(SchetsControl s, char c);
    }

    public class TekstTool : StartpuntTool
    {
        private string tekst = "";
        public override string ToString() { return "tekst"; }

        public override void MuisDrag(SchetsControl s, Point p) { }
        public override void MuisVast(SchetsControl s, Point p)
        {
            base.MuisVast(s, p);
            s.AddShape(new TekstShape(s.PenKleur, startpunt, tekst));
        }

        public override void Letter(SchetsControl s, char c)
        {
            if (c >= 32)
            {
                // Als er een geldig tekstShape object boven aan de vormen lijst staat, bewerk dan de tekst.
                if (s.Schets.vormen.Last() is TekstShape tekstShape)
                {
                    tekstShape.Tekst += c;
                    s.Invalidate();
                }

                // tekst += c;
                //
                // Graphics gr = s.MaakBitmapGraphics();
                // Font font = new Font("Tahoma", 40);
                // string _tekst = c.ToString();
                // SizeF sz = gr.MeasureString(tekst, font, this.startpunt, StringFormat.GenericTypographic);
                // gr.DrawString(tekst, font, kwast, this.startpunt, StringFormat.GenericTypographic);
                // // gr.DrawRectangle(Pens.Black, startpunt.X, startpunt.Y, sz.Width, sz.Height);
                // startpunt.X += (int)sz.Width;
                // s.Invalidate();
            }
        }
    }

    public abstract class TweepuntTool : StartpuntTool
    {
        public static Rectangle Punten2Rechthoek(Point p1, Point p2)
        {
            return new Rectangle(new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y))
                                , new Size(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y))
                                );
        }
        public static Pen MaakPen(Brush b, int dikte)
        {
            Pen pen = new Pen(b, dikte);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            return pen;
        }
        public override void MuisVast(SchetsControl s, Point p)
        {
            base.MuisVast(s, p);
            kwast = Brushes.Gray;
        }
        public override void MuisDrag(SchetsControl s, Point p)
        {
            s.Refresh();
            this.Bezig(s.CreateGraphics(), this.startpunt, p);
        }
        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);

            this.Compleet(s, startpunt, p);
            s.Invalidate();
        }
        public override void Letter(SchetsControl s, char c)
        {
        }
        public abstract void Bezig(Graphics g, Point p1, Point p2);

        public abstract void Compleet(SchetsControl s, Point p1, Point p2);
    }

    public class RechthoekTool : TweepuntTool
    {
        public override string ToString() { return "kader"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawRectangle(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1, p2));
        }

        public override void Compleet(SchetsControl s, Point p1, Point p2)
        {
            RechthoekShape r = new RechthoekShape(s.PenKleur, Punten2Rechthoek(p1, p2));
            s.AddShape(r);
        }
    }

    public class VolRechthoekTool : RechthoekTool
    {
        public override string ToString() { return "vlak"; }

        public override void Compleet(SchetsControl s, Point p1, Point p2)
        {
            Point start = p1, size = p2;

            // Zorg dat p1 het punt is met de laagste
            if (p2.X < p1.X)
            {
                start = p2;
                start.Y -= p2.Y;
                size.Y += p2.Y;
            }

            size = new Point(size.X - start.X, size.Y - start.Y);
            var rechthoek = new VolRechthoekShape(s.PenKleur, Punten2Rechthoek(p1, p2));
            s.AddShape(rechthoek);
        }
    }

    public class CirkelTool : TweepuntTool
    {
        public override string ToString() { return "cirkel"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawEllipse(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1, p2));
        }

        public override void Compleet(SchetsControl s, Point p1, Point p2)
        {
            var c = new CirkelShape(s.PenKleur, Punten2Rechthoek(p1, p2));
            s.AddShape(c);
        }
    }

    public class GevuldeCirkelTool : CirkelTool
    {
        public override string ToString() { return "gevuldecirkel"; }
        public override void Compleet(SchetsControl s, Point p1, Point p2)
        {
            VolCirkelShape c = new VolCirkelShape(s.PenKleur, Punten2Rechthoek(p1, p2));
            s.AddShape(c);
        }
    }

    public class LijnTool : TweepuntTool
    {
        public override string ToString() { return "lijn"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawLine(MaakPen(this.kwast, 3), p1, p2);
        }

        public override void Compleet(SchetsControl s, Point p1, Point p2)
        {
            s.AddShape(new LijnShape(s.PenKleur, p1, p2));
        }
    }
    public class PenTool : LijnTool
    {
        public override string ToString() { return "pen"; }

        public override void MuisDrag(SchetsControl s, Point p)
        {
            this.MuisLos(s, p);
            this.MuisVast(s, p);
        }
    }

    public class GumTool : StartpuntTool
    {
        public override string ToString() { return "gum"; }

        //public override void Bezig(Graphics g, Point p1, Point p2)
        //{
        //    g.DrawLine(MaakPen(Brushes.White, 7), p1, p2);
        //}

        public override void MuisLos(SchetsControl s, Point p)
        {
            s.Schets.VerwijderObject(p, s);
        }

        public override void MuisDrag(SchetsControl s, Point p)
        {
        }

        public override void Letter(SchetsControl s, char c)
        {
        }
    }
}
