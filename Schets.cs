using System;
using System.Collections.Generic;
using System.Drawing;

namespace SchetsEditor
{
    public class Schets
    {
        private Bitmap bitmap;
        private Size size = new Size(1,1);
        public List<Shape> vormen = new List<Shape>(); // Lijst om elementen op te slaan.

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
            size = sz;
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
            gr.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);
            foreach (Shape s in vormen) // Alle elementen aflopen en tekenfunctie oproepen.
            {
                s.teken(gr);
            }


            // TODO: gr.DrawImage... kan weg zodra de functie hierboven is geïmplementeerd.
            // gr.DrawImage(bitmap, 0, 0);
        }
        public void Schoon()
        {
            vormen.Clear(); // Lijst leegmaken na schoon. 
            Graphics gr = Graphics.FromImage(bitmap);
            gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        }
        public void Roteer()
        {
            // TODO: Verplaats de items in de TekenObject lijst
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }
    }
}
