using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace RestoranProjekat.Forme
{
    /// <summary>
    /// Interaction logic for FrmPice.xaml
    /// </summary>
    public partial class FrmPice : Window
    {
        private bool azuriraj;
        private DataRowView pomocniRed;
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        public FrmPice()
        {
            InitializeComponent();
            txtNaziv.Focus();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        public FrmPice(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtNaziv.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        private void btnIzadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();
                string vratiVrstu = @"select MeniID, VrstaKuhinje from tblMenu";
                SqlDataAdapter daVrsta = new SqlDataAdapter(vratiVrstu, konekcija);
                DataTable dtVrsta = new DataTable();
                daVrsta.Fill(dtVrsta);
                cmbMeni.ItemsSource = dtVrsta.DefaultView;
                daVrsta.Dispose();
                dtVrsta.Dispose();
            }
            catch (SqlException)
            {
                MessageBox.Show("Padajuce liste nisu popunjene", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    this.Close();
                }
            }
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@MeniID", SqlDbType.Int).Value = cmbMeni.SelectedValue;
                cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar).Value = txtNaziv.Text;
                cmd.Parameters.Add("@Cena", SqlDbType.Int).Value = Convert.ToInt32(txtCena.Text);
                cmd.Parameters.Add("@Alkoholno", SqlDbType.Bit).Value = cbAlkoholno.IsChecked;
                cmd.Parameters.Add("@Zapremina", SqlDbType.Int).Value = Convert.ToInt32(txtZapremina.Text);

                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@PiceID", SqlDbType.Int).Value = red["PiceID"];
                    cmd.CommandText = @"Update tblPice Set MeniID = @MeniID, Naziv = @Naziv, Cena = @Cena,
                                     Alkoholno = @Alkoholno, Zapremina = @Zapremina , where PiceID = @PiceID";

                }
                else
                {
                    cmd.CommandText = @"insert into 
                        tblPice(MeniID, Naziv, Cena, Alkoholno, Zapremina)
                        values(@MeniID, @Naziv, @Cena, @Alkoholno, @Zapremina)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Unos prosledjenih podataka nije validan!", "Greska", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }

        }

    }
}
