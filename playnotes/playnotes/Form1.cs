using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace playnotes
{
    public partial class Form1 : Form
    {
        public acordesCLS acrds = new acordesCLS();
        public Form1()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void LimparTudo() {
            foreach (System.Windows.Forms.Control Notas in BRACO.Controls)
            {
                if (Notas is Label)
                {
                    Label btnNota = Notas as Label;
                    if (btnNota.Name.Contains("lbl_"))
                    {
                        btnNota.Visible = false;
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                BracoCLS items = new BracoCLS();
                using (StreamReader r = new StreamReader(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Replace("bin\\Debug","") + "Braco.json"))
                {
                    string json = r.ReadToEnd();
                    //var items = JsonConvert.DeserializeObject(json);
                    items = JsonConvert.DeserializeObject<BracoCLS>(json);
                }

                
                using (StreamReader r = new StreamReader(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Replace("bin\\Debug", "") + "Acordes.json"))
                {
                    string json = r.ReadToEnd();
                    //var items = JsonConvert.DeserializeObject(json);
                    acrds = JsonConvert.DeserializeObject<acordesCLS>(json);
                }

                foreach (Corda CRD in items.cordas)
                {
                    foreach (Nota NTA in CRD.notas)
                    {
                        Label LBL = new Label();
                        LBL.Text = NTA.nota;
                        LBL.BackColor = Color.FromName(CRD.cor);
                        LBL.Name = "lbl_" + CRD.corda + "_" + NTA.posicao + "_" + NTA.nota;
                        LBL.AutoSize = true;
                        LBL.TextAlign = ContentAlignment.MiddleCenter;
                        LBL.Font=new Font("Microsoft Sans Serif", 12);
                        LBL.Margin = new Padding(5, 5, 0, 0);
                        int rw = 0;
                        switch (CRD.corda)
                        {
                            case "e":
                                {
                                    rw = 0;
                                    break;
                                }
                            case "B":
                                {
                                    rw = 1;
                                    break;
                                }
                            case "D":
                                {
                                    rw = 2;
                                    break;
                                }
                            case "G":
                                {
                                    rw = 3;
                                    break;
                                }
                            case "A":
                                {
                                    rw = 4;
                                    break;
                                }
                            case "E":
                                {
                                    rw = 5;
                                    break;
                                }
                            default:
                                break;
                        }
                        BRACO.Controls.Add(LBL, NTA.posicao, rw);
                    }
                }
                foreach (Acorde acr in acrds.acordes)
                {
                    cboAcordes.Items.Add(acr.acorde);
                }                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        public class CordaBracos
        {
            public string corda_braco { get; set; }
            public int posicao { get; set; }
        }

        public class Acorde
        {
            public string acorde { get; set; }
            public IList<CordaBracos> corda_braco { get; set; }
        }

        public class acordesCLS
        {
            public IList<Acorde> acordes { get; set; }
        }

        public class Nota
        {
            public int posicao { get; set; }
            public string nota { get; set; }
        }

        public class Corda
        {
            public string corda { get; set; }
            public string cor { get; set; }
            public List<Nota> notas { get; set; }
        }

        public class BracoCLS
        {
            public List<Corda> cordas { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboAcordes.Text != "")
                {
                    LimparTudo();
                    Acorde CRD = acrds.acordes.FirstOrDefault(r => r.acorde == cboAcordes.Text);
                    foreach (CordaBracos CrdBra in CRD.corda_braco)
                    {
                        string nm_nota = "lbl_" + CrdBra.corda_braco + "_" + CrdBra.posicao + "_";
                        foreach (System.Windows.Forms.Control Notas in BRACO.Controls)
                        {
                            if (Notas is Label)
                            {
                                Label btnNota = Notas as Label;
                                if (btnNota.Name.Contains(nm_nota))
                                {
                                    btnNota.Visible = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
