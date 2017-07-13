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
        public pentasCLS pntas = new pentasCLS();
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
                    items = JsonConvert.DeserializeObject<BracoCLS>(json);
                }

                using (StreamReader r = new StreamReader(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Replace("bin\\Debug", "") + "Acordes.json"))
                {
                    string json = r.ReadToEnd();
                    acrds = JsonConvert.DeserializeObject<acordesCLS>(json);
                }

                using (StreamReader r = new StreamReader(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Replace("bin\\Debug", "") + "Penta.json"))
                {
                    string json = r.ReadToEnd();
                    pntas = JsonConvert.DeserializeObject<pentasCLS>(json);
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

        public class Posico
        {
            public int pos_1 { get; set; }
            public int pos_2 { get; set; }
        }

        public class Penta
        {
            public string tonica { get; set; }
            public int desenho { get; set; }
            public string xcorda { get; set; }
            public IList<Posico> posicoes { get; set; }
        }

        public class pentasCLS
        {
            public IList<Penta> penta { get; set; }
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

        private void button2_Click(object sender, EventArgs e)
        {
            LimparTudo();
            string Posicao = null;
            foreach (System.Windows.Forms.Control controle in PoisicaoPenta.Controls)
            {
                if (controle is RadioButton)
                {
                    RadioButton rdoPosicao = controle as RadioButton;
                    if (rdoPosicao.Checked)
                    {
                        Posicao = rdoPosicao.Text.Split(' ')[1];
                    }

                }
            }
            if (Posicao == null)
            {
                MessageBox.Show("Selecione uma posição", "Penta");
                return;
            }
            string Tonica = cboNota.Text.ToUpper();
            decimal dPosicao = Convert.ToDecimal(Posicao);

            foreach (Penta penta in pntas.penta.Where(R => R.desenho == dPosicao && R.tonica== Tonica))
            {
                foreach (System.Windows.Forms.Control Notas in BRACO.Controls)
                {
                    if (Notas is Label)
                    {
                        Label btnNota = Notas as Label;
                        string nm_nota1 = "lbl_" + penta.xcorda + "_" + penta.posicoes[0].pos_1.ToString();
                        string nm_nota2 = "lbl_" + penta.xcorda + "_" + penta.posicoes[0].pos_2.ToString();
                        if (btnNota.Name.Contains(nm_nota1))
                        {
                            btnNota.Visible = true;
                        }
                        if (btnNota.Name.Contains(nm_nota2))
                        {
                            btnNota.Visible = true;
                        }
                    }
                }
            }
        }
    }
}
