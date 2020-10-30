using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace SchetsEditor
{
    public class Schets
    {
        private Bitmap bitmap;
        public List<Shape> vormen = new List<Shape>(); // Lijst om elementen op te slaan.
        public Size Grootte { get; set; } = new Size(1, 1);
        private const int offset = 10;

        public Schets()
        {
            bitmap = new Bitmap(1, 1);
        }

        public Graphics BitmapGraphics
        {
            get { return Graphics.FromImage(bitmap); }
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
            foreach (Shape s in vormen) // Alle elementen aflopen en tekenfunctie oproepen.
            {
                s.teken(gr);
            }

            // TODO: gr.DrawImage... kan weg zodra de functie hierboven is geïmplementeerd.
            // gr.DrawImage(bitmap, 0, 0);
        }

        // Methode voor de gum in tools om te kijken of op punt muis een object is geraakt
        public void VerwijderObject(Point p, SchetsControl s)
        {
            for (int i = 0; i < vormen.Count; i++)
            {
                if (IsGeraakt(vormen[i], p))
                {
                    vormen.RemoveAt(i);
                    // Opnieuw lijst tekenen
                    s.Invalidate();
                }
            }
        }

        public bool IsGeraakt(Shape s, Point p)
        {
            if (s is LijnShape)
            {
                return AfstandTotLijn(s.p1, s.p2, p) < offset;
            }
            else if (s is RechthoekShape)
            {
                return BinnenRechthoek(s, p, offset);
            }
            else if (s is TekstShape)
            {
            }
            else if (s is VolRechthoekShape)
            {
                return BinnenVolRechthoek(s, p);
            }
            else if (s is CirkelShape)
            {
            }
            else if (s is VolCirkelShape)
            {
            }

            return false;
        }


        public void Schoon()
        {
            vormen.Clear(); // Lijst leegmaken na schoon. 
            Graphics gr = Graphics.FromImage(bitmap);
            gr.FillRectangle(Brushes.White, 0, 0, Grootte.Width, Grootte.Height);
        }

        public void Roteer()
        {
            // TODO: Verplaats de items in de TekenObject lijst
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
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

        private bool AfstandVulCirkel(Shape cirkel, Point p)
        {
            return BinnenVolRechthoek(cirkel, p);
        }

        private double AfstandTotLijn(Point eerste, Point tweede, Point p)
        {
            double xx, yy;

            double dx = tweede.X - eerste.X;
            double dy = tweede.Y - eerste.Y;


            double res = ((p.X - eerste.X) * dx + (p.Y - eerste.Y) * dy) / (dx * dx + dy * dy);

            if (res < 0)
            {
                xx = eerste.X;
                yy = eerste.Y;
            }
            else if (res > 1)
            {
                xx = tweede.X;
                yy = tweede.Y;
            }

            else // [0, 1]
            {
                xx = eerste.X + res * dx;
                yy = eerste.Y + res * dy;
            }

            return Math.Sqrt(Math.Pow(p.X - xx, 2) + Math.Pow(p.Y - yy, 2));
        }
    }
}