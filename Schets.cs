using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace SchetsEditor
{
    public class Schets
    {
        public List<Shape> Vormen = new List<Shape>(); // Lijst om elementen op te slaan.
        public Stack<Shape> Redos = new Stack<Shape>();
        public Size Grootte { get; set; } = new Size(1, 1);
        private const int Offset = 10;
        private int _count = 0;

        public Schets()
        {
        }

        public void VeranderAfmeting(Size sz)
        {
            Grootte = sz;
            // if (sz.Width > bitmap.Size.Width || sz.Height > bitmap.Size.Height)
            // {
            //     Bitmap nieuw = new Bitmap( Math.Max(sz.Width,  bitmap.Size.Width)
            //                              , Math.Max(sz.Height, bitmap.Size.Height)
            //                              );
            //     Graphics gr = Graphics.FromImage(nieuw);
            //     gr.FillRectangle(Brushes.White, 0, 0, sz.Width, sz.Height);
            //     gr.DrawImage(bitmap, 0, 0);
            //     bitmap = nieuw;
            // }
        }

        public void Teken(Graphics gr)
        {
            gr.FillRectangle(Brushes.White, 0, 0, Grootte.Width, Grootte.Height);
            foreach (Shape s in Vormen) // Alle elementen aflopen en tekenfunctie oproepen.
            {
                s.teken(gr);
            }
        }

        // Methode voor de gum in tools om te kijken of op punt muis een object is geraakt
        public void VerwijderObject(Point p, SchetsControl s)
        {
            for (int i = 0; i < Vormen.Count; i++)
            {
                if (IsGeraakt(Vormen[i], p))
                {
                    Vormen.RemoveAt(i);
                    // Opnieuw lijst tekenen
                    s.Invalidate();
                }
            }
        }

        public bool IsGeraakt(Shape s, Point p)
        {
            switch (s)
            {
                case LijnShape _:
                    return AfstandTotLijn(s.p1, s.p2, p) < Offset;
                case RechthoekShape _:
                    return BinnenRechthoek(s, p, Offset);
                case TekstShape ts:
                    return BinnenFont(p, ts);
                case VolRechthoekShape _:
                    return BinnenVolRechthoek(s, p);
                case CirkelShape _:
                    return BinnenCirkel(p, s);
                case VolCirkelShape _:
                    return BinnenGevuldeCirkel(p, s);
                default:
                    return false;
            }
        }


        public void Schoon()
        {
            Vormen.Clear(); // Lijst leegmaken na schoon.
        }

        public void Roteer()
        {
            // Werkt nog niet
            foreach (var shape in Vormen)
            {
                if (shape is TekstShape ts)
                {
                    if (ts.Hoek == 3)
                        ts.Hoek = 0;
                    else
                        ts.Hoek++;
                }

                //Code werkt wel, maar er komt een zwarte balk
                int x1 = shape.p1.X - Grootte.Width / 2;
                int y1 = shape.p1.Y - Grootte.Height / 2;

                int x2 = -y1 + Grootte.Width / 2;
                int y2 = x1 + Grootte.Height / 2;

                shape.p1.X = x2;
                shape.p1.Y = y2;
            }

            // TODO: Verplaats de items in de TekenObject lijst
            // bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        private bool BinnenVolRechthoek(Shape s, Point p)
        {
            // P1 is de locatie, en P2 is de breedte & hoogte
            return p.X > s.p1.X &&
                   p.X < s.p1.X + s.p2.X &&
                   p.Y > s.p1.Y &&
                   p.Y < s.p1.Y + s.p2.Y;
        }

        private bool BinnenRechthoek(Shape s, Point p, int offset)
        {
            var x = s.p1.X;
            var y = s.p1.Y;
            return
                // Check de linker rand
                (p.X > x - offset &&
                 p.X < x + offset) ||

                // Check de rechter rand
                (p.X > x + s.p2.X - offset &&
                 p.X < x + s.p2.X + offset) ||

                // Check de bovenrand
                (p.Y > y - offset &&
                 p.Y < y + offset) ||

                // Check de onderrand
                (p.Y > y + s.p2.Y - offset &&
                 p.Y < y + s.p2.Y + offset)
                ;
        }

        private double AfstandTotLijn(Point eerste, Point tweede, Point p)
        {
            double resx, resy;
            double dx = tweede.X - eerste.X;
            double dy = tweede.Y - eerste.Y;
            double res = ((p.X - eerste.X) * dx + (p.Y - eerste.Y) * dy) / (dx * dx + dy * dy);

            if (res < 0)
            {
                resx = eerste.X;
                resy = eerste.Y;
            }
            else if (res > 1)
            {
                resx = tweede.X;
                resy = tweede.Y;
            }
            else // [0, 1]
            {
                resx = eerste.X + res * dx;
                resy = eerste.Y + res * dy;
            }

            return Math.Sqrt(Math.Pow(p.X - resx, 2) + Math.Pow(p.Y - resy, 2));
        }

        private bool BinnenGevuldeCirkel(Point p, Shape s)
        {
            double straalx = (double) s.p2.X / 2.0;
            double straaly = (double) s.p2.Y / 2.0;
            double m1 = (double) s.p1.X + straalx;
            double m2 = (double) s.p1.Y + straaly;

            double res = ((Math.Pow(((double) p.X - m1), 2.0) / Math.Pow(straalx, 2.0))
                          + (Math.Pow(((double) p.Y - m2), 2.0) / Math.Pow(straaly, 2.0)));

            return res <= 1;
        }

        private bool BinnenCirkel(Point p, Shape s)
        {
            double straalx = (double) s.p2.X / 2.0;
            double straaly = (double) s.p2.Y / 2.0;
            double m1 = (double) s.p1.X + straalx;
            double m2 = (double) s.p1.Y + straaly;

            double res = ((Math.Pow(((double) p.X - m1), 2.0) / Math.Pow(straalx, 2.0))
                          + (Math.Pow(((double) p.Y - m2), 2.0) / Math.Pow(straaly, 2.0)));

            return res < 1.2 && res > 0.9;
        }

        private bool BinnenFont(Point p, TekstShape ts)
        {
            // Gebruik Math.Ceiling om de kleinst mogelijke int groter dan ts.Size.Width/Height te verkrijgen:
            // Maak een bitmap object waar de string precies inpast.
            var bmp = new Bitmap((int) Math.Ceiling(ts.Size.Width), (int) Math.Ceiling(ts.Size.Height));
            var gr = Graphics.FromImage(bmp);
            gr.DrawString(ts.Tekst, ts.Font, Brushes.Black, Point.Empty);
            int x = p.X - ts.p1.X,
                y = p.Y - ts.p1.Y;
            if ((x > 0 && x < bmp.Width && y > 0 && y < bmp.Height))
            {
                Color pixel = bmp.GetPixel(x, y);
                return pixel.A == 255 && pixel.R == 0 && pixel.G == 0 && pixel.B == 0;
            }

            return false;
        }
    }
}