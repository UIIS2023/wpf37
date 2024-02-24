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
    /// Interaction logic for FrmSpecijalnaPonuda.xaml
    /// </summary>
    public partial class FrmSpecijalnaPonuda : Window
    {
        private bool azuriraj;
        private DataRowView pomocniRed;
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();

        public FrmSpecijalnaPonuda()
        {
            InitializeComponent();
            txtNaziv.Focus();
            konekcija = kon.KreirajKonekciju();
        }

        public FrmSpecijalnaPonuda(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtNaziv.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
            konekcija = kon.KreirajKonekciju();
        }

        private void btnIzadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PopuniPadajuceListe()
        {
            try
            {
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
                DateTime date = (DateTime)dateTrajanje.SelectedDate;
                string datum = date.ToString("yyyy-MM-dd");
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@MeniID", SqlDbType.Int).Value = cmbMeni.SelectedValue;
                cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar).Value = txtNaziv.Text;
                cmd.Parameters.Add("@TrajanjePonude", SqlDbType.DateTime).Value = datum;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@DeoRestoranaIDID", SqlDbType.Int).Value = red["DeoRestoranaID"];
                    cmd.CommandText = @"Update tblSpecijalnaPonuda Set MeniID=@MeniID, Naziv = @Naziv,
                                       TrajanjePonude = @TrajanjePonude  Where DeoRestoranaID = @DeoRestoranaID";
                }
                else
                {
                    cmd.CommandText = @"Insert into tblDeoRestorana(MeniID, Naziv, TrajanjePonude) 
                                values(@Meni, @Naziv, @TrajanjePonude);";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose(); 
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Doslo je do greske priliom konverzije podataka", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
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
