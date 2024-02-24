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
    /// Interaction logic for FrmSto.xaml
    /// </summary>
    public partial class FrmSto : Window
    {
        private bool azuriraj;
        private DataRowView pomocniRed;
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        public FrmSto()
        {
            InitializeComponent();
            txtKapacitet.Focus();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        public FrmSto(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtKapacitet.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        private void btnIzadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();
                string vratiKonobar = @"select KonobarID, Ime, Prezime from tblKonobar";
                SqlDataAdapter daKonobar = new SqlDataAdapter(vratiKonobar, konekcija);
                DataTable dtKonobar = new DataTable();
                daKonobar.Fill(dtKonobar);
                cmbKonobar.ItemsSource = dtKonobar.DefaultView;
                daKonobar.Dispose();
                dtKonobar.Dispose();
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
                cmd.Parameters.Add("@KonobarID", SqlDbType.Int).Value = cmbKonobar.SelectedItem;
                cmd.Parameters.Add("@Kapacitet", SqlDbType.Int).Value = Convert.ToInt32(txtKapacitet.Text);
                cmd.Parameters.Add("@BrojStola", SqlDbType.Int).Value = Convert.ToInt32(txtBrojStola.Text);
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@StoID", SqlDbType.Int).Value = red["StoID"];
                    cmd.CommandText = "@Update tblSto Set KonobarID = @KonobarID, Kapacitet = @Kapacitet," +
                        " BrojStola = @BrojStola, where StoID = @StoID";

                }
                else
                {
                    cmd.CommandText = @"insert into 
                        tblSto(KonobarID, Kapacitet, BrojStola)
                        values(@KonobarID, @Kapacitet, @Brojstola)";
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
