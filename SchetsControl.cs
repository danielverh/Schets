using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SchetsEditor
{   public class SchetsControl : UserControl
    {   private Schets schets;
        private Color penkleur;

        public Color PenKleur
        { get { return penkleur; }
        }
        public Schets Schets
        { get { return schets;   }
        }
        public SchetsControl()
        {   this.BorderStyle = BorderStyle.Fixed3D;
            this.schets = new Schets();
            this.Paint += this.teken;
            this.Resize += this.veranderAfmeting;
            this.DoubleBuffered = true;
            this.veranderAfmeting(null, null);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
        private void teken(object o, PaintEventArgs pea)
        {   schets.Teken(pea.Graphics);
        }
        private void veranderAfmeting(object o, EventArgs ea)
        {   schets.VeranderAfmeting(this.ClientSize);
            this.Invalidate();
        }
        public void Schoon(object o, EventArgs ea)
        {   schets.Schoon();
            this.Invalidate();
        }
        public void Roteer(object o, EventArgs ea)
        {   schets.VeranderAfmeting(new Size(this.ClientSize.Height, this.ClientSize.Width));
            schets.Roteer();
            this.Invalidate();
        }
        public void VeranderKleur(object obj, EventArgs ea)
        {   string kleurNaam = ((ComboBox)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }
        public void VeranderKleurViaMenu(object obj, EventArgs ea)
        {   string kleurNaam = ((ToolStripMenuItem)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }

        public void AddShape(Shape s)
        {
            schets.vormen.Add(s);
        }

        public void Opslaan()
        {
            if (schets.vormen.Count == 0)
                return; // Niets doen, de schets is leeg.
            // Maak een windows explorer 'save' dialoog
            var dialog = new SaveFileDialog();
            // Schets bestandstype:
            dialog.Filter = "Schets bestand (*.sch) | *.sch";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                BestandLader.SchetsOpslaan(dialog.FileName, schets.Grootte, schets.vormen);
            }
        }

        public void Undo(object sender, EventArgs e)
        {
            if (schets.vormen.Count > 0)
                schets.vormen.RemoveAt(schets.vormen.Count - 1);
            this.Invalidate();
        }

        
    }
}
