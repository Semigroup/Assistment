using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;
using Assistment.form;
using Assistment.Extensions;

namespace Assistment.form
{
    public class SteuerBox : ButtonReihe
    {
        private string Neu, Laden, Speichern, SpeichernUnter;
        private bool speichernNotwendig = false;
        public bool SpeichernNotwendig
        {
            get { return speichernNotwendig; }
            set
            {
                speichernNotwendig = value;
                Enable(speicherort != null && value, Speichern);
            }
        }
        private string speicherort;
        public string Speicherort
        {
            get { return speicherort; }
            set
            {
                speicherort = value;
                OpenFileDialog.FileName = speicherort;
                SaveFileDialog.FileName = speicherort;
                Enable(speicherort != null, Speichern);
            }
        }

        public event EventHandler NeuClicked = delegate { };
        public event EventHandler SpeichernClicked = delegate { };
        public event EventHandler LadenClicked = delegate { };

        private SaveFileDialog SaveFileDialog = new SaveFileDialog();
        private OpenFileDialog OpenFileDialog = new OpenFileDialog();

        public SteuerBox(string Name)
            : base(true,
                "Neues " + Name + " Erstellen",
                Name + " Laden",
                Name + " Speichern",
                Name + " Speichern Unter")
        {
            Neu = buttons[0].Text;
            Laden = buttons[1].Text;
            Speichern = buttons[2].Text;
            SpeichernUnter = buttons[3].Text;

            SaveFileDialog.DefaultExt = OpenFileDialog.DefaultExt = "xml";
            SaveFileDialog.Filter = OpenFileDialog.Filter = "Xml-Dateien(*.xml)|*.xml";
            SpeichernNotwendig = false;
        }

        protected override void OnButtonClicked(object sender, EventArgs e)
        {
            base.OnButtonClicked(sender, e);

            if (Message == Neu && CanExit())
            {
                this.Speicherort = null;
                SpeichernNotwendig = false;
                NeuClicked(this, e);
            }
            else if (Message == Laden && CanExit() && OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.Speicherort = OpenFileDialog.FileName;
                SpeichernNotwendig = false;
                LadenClicked(this, e);
            }
            else if (Message == Speichern)
            {
                SpeichernNotwendig = false;
                SpeichernClicked(this, e);
            }
            else if (Message == SpeichernUnter)
            {
                if (speicherort != null)
                    SaveFileDialog.InitialDirectory = Path.GetDirectoryName(speicherort);
                if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.speicherort = SaveFileDialog.FileName;
                    SpeichernNotwendig = false;
                    SpeichernClicked(this, e);
                }
            }
        }

        public bool CanExit()
        {
            if (SpeichernNotwendig)
                return MessageBox.Show("Wollen Sie wirklich fortfahren ohne vorher abzuspeichern?", "Achtung, Daten können verloren gehen!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    == DialogResult.Yes;
            else
                return true;
        }
    }
}
