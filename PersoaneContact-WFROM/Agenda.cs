using System;
using System.Drawing;
using System.Windows.Forms;
using LibrarieModele;
using NivelAccesDate;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace PersoaneContact_WFROM
{
    public partial class Agenda : Form
    {
        IStocareData adminPersoane;
        public Agenda()
        {
            
            InitializeComponent();
            adminPersoane = StocareFactory.GetAdministratorStocare();
            this.Width = 400;
        }

        private void btnAdauga_Click(object sender, EventArgs e)
        {
            ResetCuloareEtichete();
            PersoaneContact s;
            CodEroare codValidare = Validare(txtNume.Text, txtPrenume.Text, txtNumarTelefon.Text, txtMail.Text);
            if (codValidare != CodEroare.CORECT)
            {
                MarcheazaControaleCuDateIncorecte(codValidare);
            }
            else
            {
                s = new PersoaneContact(txtNume.Text, txtPrenume.Text, txtNumarTelefon.Text, txtMail.Text,Convert.ToInt32(2));
                
                s.GRUP = GetGrupSelectat();
                s.Genul = cmbGenul.Text;
                s.REGIUNE = GetRegiuneSelectata();
                s.DataActualizarii = DateTime.Now;
                s.DataNasterii = dtpNastere.Value;
                adminPersoane.AddPersoana(s);
                lblInfo.Text = "Contactul a fost adaugat";
                ResetControale();
            }


        }
        private CodEroare Validare(string nume, string prenume, string numar, string mail)
        {
            
            if (nume == string.Empty)
            {
                return CodEroare.NUME_INCORECT;
            }
            if (prenume == string.Empty)
            {
                return CodEroare.PRENUME_INCORECT;
            }
            if (numar == string.Empty)
            {        
                    return CodEroare.NUMAR_INCORECTE;
          
            }
            if (mail == string.Empty)
            {
                return CodEroare.MAIL_INCORECT;
            }
            return CodEroare.CORECT;
        }

        private void btnAfisare_Click(object sender, EventArgs e)
        {
            this.Width = 1450; 
            List<PersoaneContact> persoane = adminPersoane.GetPersoane();

            AdaugaPersoaneInControlListbox(persoane);
            AdaugaPersoaneInControlDataGridView(persoane);
            ResetControale();
        }
        private void AdaugaPersoaneInControlListbox(List<PersoaneContact> persoane)
        {
            lstAfisare.Items.Clear();
            //var antetTabel = String.Format("{0,-5}{1,15}{2,20}{3,15}{4,20}\n","ID", "Nume Prenume",  "Telefon", "Mail", "Grupul");
            //lstAfisare.Items.Add(antetTabel);
            //List<PersoaneContact> persoane = adminPersoane.GetPersoane();
            foreach (PersoaneContact s in persoane)
            {
                lstAfisare.Items.Add(s);
            //    var linieTabel = String.Format("{0,-5}{1,15}{2,20}{3,15}{4,20}\n",s.IdContact, s.NumeleComplet, s.NumarTelefon, s.AdresaEmail, s.GRUP);
            //    lstAfisare.Items.Add(linieTabel);
            }
            
        }
        private void AdaugaPersoaneInControlDataGridView(List<PersoaneContact> persoane)
        {
            //reset continut control DataGridView
            dataGridContact.DataSource = null;
            //!!!! Controlul de tip DataGridView are ca sursa de date lista de obiecte de tip Student !!!
            //dataGridStudenti.DataSource = studenti;

            //personalizare sursa de date
            dataGridContact.DataSource = persoane.Select(s => new { s.IdContact,s.Nume, s.Prenume, s.NumarTelefon, s.AdresaEmail, s.GRUP, s.Genul, s.REGIUNE, s.DataNasterii }).ToList();
        }

    



        private void btnCauta_Click(object sender, EventArgs e)
        {
            PersoaneContact s = adminPersoane.GetPersoane(txtNume.Text, txtPrenume.Text);//, txtNumarTelefon.Text, txtMail.Text);
            if (s != null)
            {
                lblInfo.Text = s.ConversieLaSir();
            }
            else
                lblInfo.Text = "Nu s-a gasit Persoana de contact";
            if (txtNume.Enabled == true && txtPrenume.Enabled == true)
            {
                txtNume.Enabled = false;
                txtPrenume.Enabled = false;

            //dezactivare butoane radio
            foreach(var button in gbdGrupContact.Controls)
                {
                    if(button is RadioButton)
                    {
                        var radioButton = button as RadioButton;
                        radioButton.Enabled = false;
                    }
                }
                foreach (var ck in gpbRegiuni.Controls)
                {
                    if (ck is CheckBox)
                    {
                        var radioButton = ck as CheckBox;
                        radioButton.Enabled = false;
                    }
                }
            }
            else
            {
                txtNume.Enabled = true;
                txtPrenume.Enabled = true;
                foreach (var button in gbdGrupContact.Controls)
                {
                    if (button is RadioButton)
                    {
                        var radioButton = button as RadioButton;
                        radioButton.Enabled = true;
                    }
                }
                foreach (var ck in gpbRegiuni.Controls)
                {
                    if (ck is CheckBox)
                    {
                        var radioButton = ck as CheckBox;
                        radioButton.Enabled = true;
                    }
                }
            }

        }

        private void btnModifica_Click(object sender, EventArgs e)
        {
            ResetCuloareEtichete();

            CodEroare codValidare = Validare(txtNume.Text, txtPrenume.Text, txtNumarTelefon.Text, txtMail.Text);
            if (codValidare != CodEroare.CORECT)
            {
                MarcheazaControaleCuDateIncorecte(codValidare);
            }
            else
            {
                PersoaneContact s = new PersoaneContact(txtNume.Text, txtPrenume.Text, txtNumarTelefon.Text, txtMail.Text, Convert.ToInt32(2));
                s.IdContact = Int32.Parse(lblID.Text);

                s.GRUP = GetGrupSelectat();
                s.REGIUNE = GetRegiuneSelectata();

                s.Genul = cmbGenul.Text;

                adminPersoane.UpdatePersoana(s);
                lblInfo.Text = "Contactul a fost actualizat";

                ResetControale();

            }
        }
        private Grup GetGrupSelectat()
        {
            if (rtbFamilie.Checked)
                return Grup.Familie;
            if (rtbPrieteni.Checked)
                return Grup.Prieteni;
            if (rtbServiciu.Checked)
                return Grup.Serviciu;
            if (rtbNecunoscut.Checked)
                return Grup.Necunoscut;
            return Grup.NonGrup;
        }
        private Regiune GetRegiuneSelectata()
        {
            if(chk1.Checked)
            {
                return Regiune.Moldova;
            }
            if (chk2.Checked)
            {
                return Regiune.Transilvania;
            }
            if (chk3.Checked)
            {
                return Regiune.Dobrogea;
            }
            if (chk4.Checked)
            {
                return Regiune.Muntenia;
            }
            else
                return Regiune.NON_REGIUNE;
        }
        private void ResetCuloareEtichete()
        {
            lblNume.ForeColor = Color.Black;
            lblPrenume.ForeColor = Color.Black;
            lblNumarTelefon.ForeColor = Color.Black;
            lblMail.ForeColor = Color.Black;
            lblGrup.ForeColor = Color.Black;
        }
        private void ResetControale()
        {
            txtNume.Text = txtPrenume.Text = txtNumarTelefon.Text = txtMail.Text = string.Empty;
            rtbFamilie.Checked = false;
            rtbNecunoscut.Checked = false;
            rtbPrieteni.Checked = false;
            rtbServiciu.Checked = false;
            cmbGenul.Text = string.Empty;
            lblInfo.Text = string.Empty;
            lblID.Text = String.Empty;
            chk1.Checked = false;
            chk2.Checked = false;
            chk3.Checked = false;
            chk4.Checked = false;














        }
        private void MarcheazaControaleCuDateIncorecte(CodEroare codValidare)
        {
            if((codValidare & CodEroare.NUME_INCORECT) == CodEroare.NUME_INCORECT)
            {
                lblNume.ForeColor = Color.Red;
            }
            if ((codValidare & CodEroare.PRENUME_INCORECT) == CodEroare.PRENUME_INCORECT)
            {
                lblPrenume.ForeColor = Color.Red;
            }
            if ((codValidare & CodEroare.NUMAR_INCORECTE) == CodEroare.NUMAR_INCORECTE)
            {
                lblNumarTelefon.ForeColor = Color.Red;
            }
            if ((codValidare & CodEroare.MAIL_INCORECT) == CodEroare.MAIL_INCORECT)
            {
                lblMail.ForeColor = Color.Red;
            }
        }

        private void btnRetea_Click(object sender, EventArgs e)
        {
            PersoaneContact s = adminPersoane.GetPersoane(txtNume.Text, txtPrenume.Text);
            if (s != null)
            {

                int NumarTelefon = 0;
                int ReteaTelefon = 0;
                int numar = Convert.ToInt32(s.NumarTelefon);
                for (int i = 0; i < Convert.ToInt32(s.NumarTelefon).ToString().Length; i++)
                {
                    if (i < 6 )
                    {
                        NumarTelefon = numar / 10;
                        numar = numar / 10;

                    }

                }
                ReteaTelefon = NumarTelefon % 1000;
                //lblRetea.Text = "" + ReteaTelefon;
                if (ReteaTelefon == 740 || ReteaTelefon == 741 || ReteaTelefon == 742 || ReteaTelefon == 743 || ReteaTelefon == 744 || ReteaTelefon == 745 || ReteaTelefon == 746 || ReteaTelefon == 747 || ReteaTelefon == 748 || ReteaTelefon == 749 ||
                    ReteaTelefon == 750 || ReteaTelefon == 751 || ReteaTelefon == 752 || ReteaTelefon == 753 || ReteaTelefon == 754 || ReteaTelefon == 755 || ReteaTelefon == 756 || ReteaTelefon == 757 || ReteaTelefon == 758 || ReteaTelefon == 759)
                {
                    lblInfo.Text = "Contactul " + s.NumeleComplet + " Este in reteaua Orange";
                }
                if (ReteaTelefon == 760 || ReteaTelefon == 761 || ReteaTelefon == 762 || ReteaTelefon == 763 || ReteaTelefon == 764 || ReteaTelefon == 765 || ReteaTelefon == 766 || ReteaTelefon == 767 || ReteaTelefon == 768 || ReteaTelefon == 769 ||
                    ReteaTelefon == 780 || ReteaTelefon == 711  || ReteaTelefon == 783 || ReteaTelefon == 784 || ReteaTelefon == 785 || ReteaTelefon == 786 || ReteaTelefon == 787 || ReteaTelefon == 788 )
                {
                    lblInfo.Text = "Contactul " + s.NumeleComplet + " Este in reteaua Telekom";
                }
                if (ReteaTelefon == 720 || ReteaTelefon == 721 || ReteaTelefon == 722 || ReteaTelefon == 723 || ReteaTelefon == 724 || ReteaTelefon == 725 || ReteaTelefon == 726 || ReteaTelefon == 727 || ReteaTelefon == 728 || ReteaTelefon == 729 ||
                    ReteaTelefon == 730 || ReteaTelefon == 731 || ReteaTelefon == 732 || ReteaTelefon == 733 || ReteaTelefon == 734 || ReteaTelefon == 735 || ReteaTelefon == 736 || ReteaTelefon == 737 || ReteaTelefon == 738 || ReteaTelefon == 739 || ReteaTelefon == 799)
                {
                    lblInfo.Text = "Contactul " + s.NumeleComplet + " Este in reteaua Vodafone";
                }
                if(ReteaTelefon == 770 || ReteaTelefon == 771 || ReteaTelefon == 772 || ReteaTelefon == 773 || ReteaTelefon == 774 || ReteaTelefon == 775 || ReteaTelefon == 776)
                {
                    lblInfo.Text = "Contactul " + s.NumeleComplet + " Este in reteaua Digi Mobil";

                }
                if (ReteaTelefon == 701 || ReteaTelefon == 702 )
                {
                    lblInfo.Text = "Contactul " + s.NumeleComplet + " Este in reteaua LycaMobile";

                }

            }
            else
            {
                lblInfo.Text = "Contact inexistent";
            }
        }

        private void lstAfisare_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetControale();
            PersoaneContact s = null;
            try
            {
                s = adminPersoane.GetContactByIndex(lstAfisare.SelectedIndex );
               
            }
            catch (Exception ex)
            {
                lblInfo.Text = "Eroare: " + ex.Message;
            }

            if (s != null)
            {
                lblID.Text = s.IdContact.ToString();

                txtNume.Text = s.Nume;
                txtPrenume.Text = s.Prenume;
                txtMail.Text = s.AdresaEmail;
                txtNumarTelefon.Text = s.NumarTelefon;

                foreach (var prgstd in gbdGrupContact.Controls)
                {
                    if (prgstd is RadioButton)
                    {
                        var prgstdBox = prgstd as RadioButton;
                        if (prgstdBox.Text == s.GRUP.ToString())
                        {
                            prgstdBox.Checked = true;
                        }
                    }
                }
                foreach (var ck in gpbRegiuni.Controls)
                {
                    if (ck is CheckBox)
                    {
                        var ch = ck as CheckBox;
                        if (ch.Text == s.REGIUNE.ToString())
                        {
                            ch.Checked = true;
                        }
                    }
                }
                cmbGenul.Text = s.Genul.ToString();




            }
        }

        private void lblID_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void sortareContacte_Click(object sender, EventArgs e)
        {
            List<PersoaneContact> fam = adminPersoane.GetGrupFamilie();
            dataGridContact.DataSource = fam.Select(s => new { s.IdContact, s.Nume, s.Prenume, s.NumarTelefon, s.AdresaEmail, s.GRUP, s.Genul, s.REGIUNE, s.DataNasterii }).ToList();
        }

        private void SalvareFisier_Click(object sender, EventArgs e)
        {
            List<PersoaneContact> conta = adminPersoane.GetGrupFamilie();
            sfd.ShowDialog();
            salvareRaport(conta, sfd.FileName);
        }
        public void salvareRaport(List<PersoaneContact> contacte, string numeFisier)
        {
            try
            {
                //instructiunea 'using' va apela la final swFisierText.Close();
                //al doilea parametru setat la 'true' al constructorului StreamWriter indica modul 'append' de deschidere al fisierului
                using (StreamWriter swFisierText = new StreamWriter(numeFisier, true))
                {
                    foreach (PersoaneContact s in contacte)
                    {
                        swFisierText.WriteLine(s.ConversieLaSir());
                    }
                }
            }
            catch (IOException eIO)
            {
                throw new Exception("Eroare la deschiderea fisierului. Mesaj: " + eIO.Message);
            }
            catch (Exception eGen)
            {
                throw new Exception("Eroare generica. Mesaj: " + eGen.Message);
            }
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Agenda_Load(object sender, EventArgs e)
        {

        }
    }
    
}
