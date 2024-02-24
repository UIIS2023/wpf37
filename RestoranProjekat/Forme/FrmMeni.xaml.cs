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
    /// Interaction logic for FrmMeni.xaml
    /// </summary>
    public partial class FrmMeni : Window
    {
        private bool azuriraj;
        private DataRowView pomocniRed;
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        public FrmMeni()
        {
            InitializeComponent();
            txtAlergeni.Focus();
            konekcija = kon.KreirajKonekciju();
        }

        public FrmMeni(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtAlergeni.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
            konekcija = kon.KreirajKonekciju();
        }

        private void btnIzadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                cmd.Parameters.Add("@Alergeni", SqlDbType.NVarChar).Value = txtAlergeni.Text;
                cmd.Parameters.Add("@VrstaKuhinje", SqlDbType.NVarChar).Value = txtVrsta.Text;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@MeniID", SqlDbType.Int).Value = red["MeniID"];
                    cmd.CommandText = @"Update tblMenu Set Alergeni=@Alergeni" +
                         "Vrsta = @VrstaKuhinje  Where MeniID = @MeniID";
                }
                else
                {
                    cmd.CommandText = @"Insert into tblMenu(Alergeni, VrstaKuhinje) values(@Alergeni);";
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
