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
    /// Interaction logic for FrmKonobar.xaml
    /// </summary>
    public partial class FrmKonobar : Window
    {
        private bool azuriraj;
        private DataRowView pomocniRed;
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        public FrmKonobar()
        {
            InitializeComponent();
            txtIme.Focus();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        public FrmKonobar(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtIme.Focus();
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
                string vratiDeo = @"Select DeoRestoranaID, Naziv from tblDeoRestorana";
                SqlDataAdapter daDeo = new SqlDataAdapter(vratiDeo, konekcija);
                DataTable dtDeo = new DataTable();
                daDeo.Fill(dtDeo);
                cmbDeoRestorana.ItemsSource = dtDeo.DefaultView;
                daDeo.Dispose();
                dtDeo.Dispose();
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
                cmd.Parameters.Add("@Ime", SqlDbType.NVarChar).Value = txtIme.Text;
                cmd.Parameters.Add("@Prezime", SqlDbType.NVarChar).Value = txtPrezime.Text;
                cmd.Parameters.Add("@DeoRestoranaID", SqlDbType.Int).Value = cmbDeoRestorana.SelectedItem;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@KonobarID", SqlDbType.Int).Value = red["KonobarID"];
                    cmd.CommandText = @"Update tblKonobar Set Ime = @Ime, Prezime = @Prezime, " +
                                    " DeoRestoranaID = @DeoRestoranaID, where KonobarID = @KonobarID";
                }//gledaj da li je @ ispred ili iza navodnika
                else
                {
                    cmd.CommandText = @"insert into 
                        tblKonobar(Ime, Prezime, DeoRestoranaID)
                        values(@Ime, @Prezime, @DeoRestoranID)";
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
