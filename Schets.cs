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
        public void VerwijderObject(Point p)
        {
            for(int t = (vormen.Count - 1); t >= 0; t++)
            {
                if (IsGeraakt(vormen[t], p))
                {
                    vormen.RemoveAt(t);
                    // Opnieuw lijst tekenen
                }
            }
        }

        public static bool IsGeraakt(Shape s, Point p)
        {
                string res = s.ToString();
                if (res == "lijn")
                {

                }
                if (res == "rechthoek")
                {
                return BinnenRechthoek(s, p, 0);
                }
                if (res == "tekst")
                {

                }
                if (res == "volcrirkel")
                {

                }
                if (res == "volrechthoek")
                {

                }
                if (res == "cirkel")
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

        public static bool BinnenRechthoek(Shape s, Point p, int offset)
        {
            return (p.X > s.p1.X + offset && p.X < s.p2.X - offset && p.Y > s.p1.Y + offset && p.Y < s.p2.Y - offset);
        }
    }
}
