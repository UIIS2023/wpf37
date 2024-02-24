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
    /// Interaction logic for FrmDeoRestorana.xaml
    /// </summary>
    public partial class FrmDeoRestorana : Window
    {
        private bool azuriraj;
        private DataRowView pomocniRed;
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();

        public FrmDeoRestorana()
        {
            InitializeComponent();
            txtNaziv.Focus();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        public FrmDeoRestorana(bool azuriraj, DataRowView pomocniRed)
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
                cmd.Parameters.Add("@Kapacitet", SqlDbType.Int).Value = txtKapacitet.Text;
                cmd.Parameters.Add("@DozvoljenoPusenje", SqlDbType.Bit).Value = cbPusenje.IsChecked;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@DeoRestoranaIDID", SqlDbType.Int).Value = red["DeoRestoranaID"];
                    cmd.CommandText = @"Update tblMenu Set MeniID = @MeniID, Naziv=@Naziv, Kapacitet = @Kapacitet,
                                   DozvoljenoPusenje = @DozvoljenoPusenje Where DeoRestoranaID = @DeoRestoranaID";
                }
                else
                {
                    cmd.CommandText = @"Insert into tblDeoRestorana(MeniID, Naziv, Kapacitet, DozvoljenoPusenje) 
                        values(@MeniID, @Naziv, @Kapacitet, @DozvoljenoPusenje);";
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
