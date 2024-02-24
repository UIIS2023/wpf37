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
    /// Interaction logic for FrmJelo.xaml
    /// </summary>
    public partial class FrmJelo : Window
    {
        private bool azuriraj;
        private DataRowView pomocniRed;
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        public FrmJelo()
        {
            InitializeComponent();
            txtNaziv.Focus();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        public FrmJelo(bool azuriraj, DataRowView pomocniRed) 
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

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();
                string vratiKuvara = @"select KuvarID, Ime, Prezime from tblKuvar";
                SqlDataAdapter daKuvar = new SqlDataAdapter(vratiKuvara, konekcija);
                DataTable dtKuvar = new DataTable();
                daKuvar.Fill(dtKuvar);
                cmbKuvar.ItemsSource = dtKuvar.DefaultView;
                daKuvar.Dispose();
                dtKuvar.Dispose();

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
                cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar).Value = txtNaziv.Text;
                cmd.Parameters.Add("@Vrsta", SqlDbType.NVarChar).Value = txtVrsta.Text;
                cmd.Parameters.Add("@Cena", SqlDbType.Int).Value = Convert.ToInt32(txtCena.Text);
                cmd.Parameters.Add("@KuvarID", SqlDbType.Int).Value = cmbKuvar.SelectedValue;
                cmd.Parameters.Add("@MeniID", SqlDbType.Int).Value = cmbMeni.SelectedValue;
                cmd.Parameters.Add("@Vegetarijansko", SqlDbType.Bit).Value = cbVegetarijansko.IsChecked;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@JeloID", SqlDbType.Int).Value = red["JeloID"];
                    cmd.CommandText = @"Update tblJelo Set Naziv = @Naziv, Vrsta = @Vrsta,
                            Cena = @Cena, KuvarID = @KuvarID, MeniID = @MeniID, Vegetarijansko = @Vegetarijansko
                            Where JeloID = @JeloID";

                }
                else
                {
                    cmd.CommandText = @"Insert into 
                                        tblJelo(KuvarID, MeniID, Naziv, Vrsta, Vegetarijansko, Cena)
                                        values (@KuvarID , @MeniID , @Naziv , @Vrsta , @Vegetarijansko, @Cena)";
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
            catch (FormatException)
            {
                MessageBox.Show("Doslo je do greske priliom konverzije podataka", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }
    }
}
