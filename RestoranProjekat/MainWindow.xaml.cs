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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using RestoranProjekat.Forme;

namespace RestoranProjekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string ucitanaTabela;
        private Konekcija kon = new Konekcija();
        private SqlConnection konekcija = new SqlConnection();
        bool azuriraj;

        #region select upiti
        static string deoRestoranaSelect = @"select DeoRestoranaID as ID, Naziv as Naziv, Kapacitet as Kapacitet, 
                      DozvoljenoPusenje as 'Dozvojeno Pusenje', VrstaKuhinje as Meni
                      from tblDeoRestorana join tblMenu on tblDeoRestorana.MeniID = tblMenu.MeniID";
        static string jeloSelect = @"select JeloID as ID, Ime + ' ' + Prezime as Kuvar, VrstaKuhinje as Meni,
                        Naziv as Naziv, Vrsta as Vrsta, Vegetarijansko as Vegetarijansko, Cena as Cena
                        from tblJelo join tblKuvar on tblJelo.KuvarID = tblKuvar.KuvarID
                                     join tblMenu on tblJelo.MeniID = tblMenu.MeniID";
        static string konobarSelect = @"select KonobarID as ID, Ime as Ime, Prezime as Prezime, tblDeoRestorana.Naziv as 'Deo restorana'
                        from tblKonobar join tblDeoRestorana on tblKonobar.DeoRestoranaID = tblDeoRestorana.DeoRestoranaID";
        static string kuvarSelect = @"select KuvarID as ID, Ime as Ime, Prezime as Prezime, Poslasticar as Poslasticar from tblKuvar";
        static string meniSelect = @"select MeniID as ID, Alergeni as Alergeni, VrstaKuhinje as 'Vrsta kuhinje' from tblMenu";
        static string piceSelect = @"select PiceID as ID, VrstaKuhinje as Meni, Naziv as Naziv, Cena as Cena, Alkoholno as Alkoholno, Zapremina as Zapremina
                        from tblPice join tblMenu on tblPice.MeniID = tblMenu.MeniID";
        static string specijalnaPonudaSelect = @"select SpecijalnaPonudaID as ID, Naziv as Naziv, VrstaKuhinje as Meni, TrajanjePonude as 'Trajanje ponude'
                        from tblSpecijalnaPonuda join tblMenu on tblSpecijalnaPonuda.MeniID = tblMenu.MeniID";
        static string stoSelect = @"select StoID as ID, Ime + ' ' + Prezime as Konobar, Kapacitet as Kapacitet, BrojStola as BrojStola
                        from tblSto join tblKonobar on tblSto.KonobarID = tblKonobr.KonobarID";
        #endregion

        #region select + uslov
        static string deorestoranaSelectUslov = @"select * from tblDeoRestorana where DeoRestoranaID=";
        static string jeloSelectUslov = @"select * from tblJelo where JeloID=";
        static string konobarSelectUslov = @"select * from tblKonobar where KonobarID=";
        static string kuvarSelectUslov = @"select * from tblKuvar where KuvarID=";
        static string meniSelectUsov = @"select * from tblMenu where MeniID=";
        static string piceSelectUslov = @"select * from tblPice where PiceID=";
        static string specijalnaPonudaSelectUslov = @"select * from tblSpecijalnaPonuda where SpecijalnaPonudaID=";
        static string stoSelectUslov = @"select * from tblSto where StoID=";
        #endregion

        #region delete
        static string deorestoranaDelete = @"delete from tblDeoRestorana where DeoRestoranaID=";
        static string jeloDelete = @"delete from tblJelo where JeloID=";
        static string konobarDelete = @"delete from tblKonobar where KonobarID=";
        static string kuvarDelete = @"delete from tblKuvar where KuvarID=";
        static string meniDelete = @"delete from tblMenu where MeniID=";
        static string piceDelete = @"delete from tblPice where PiceID=";
        static string specijalnaPonudaDelete = @"delete from tblSpecijalnaPonuda where SpecijalnaPonudaID=";
        static string stoDelete = @"delete from tblSto where StoID=";
        #endregion


        public MainWindow()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            UcitajPodatke(dataGridCenralni, meniSelect);
            lblNaslov.Content = "Restoran";
        }

        private void UcitajPodatke(DataGrid grid, string selectUpit)
        {
            try
            {
                konekcija.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectUpit, konekcija);
                DataTable dt = new DataTable();
                sqlDataAdapter.Fill(dt);
                if (grid != null)
                {
                    grid.ItemsSource = dt.DefaultView;
                }
                ucitanaTabela = selectUpit;
                dt.Dispose();
                sqlDataAdapter.Dispose();
            }
            catch (SqlException)
            {
                MessageBox.Show("Neuspesno uneti podaci.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                konekcija.Close();
            }
        }

        //gore
        private void btnDeoRestorana_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCenralni,deoRestoranaSelect);
            lblNaslov.Content = "Deo Restorana";
        }

        private void btnJelo_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCenralni, jeloSelect);
            lblNaslov.Content = "Jelo";
        }

        private void btnKonobar_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCenralni, konobarSelect);
            lblNaslov.Content = "Konobar";
        }

        private void btnKuvar_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCenralni, kuvarSelect);
            lblNaslov.Content = "Kuvar";
        }

        private void btnMeni_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCenralni, meniSelect);
            lblNaslov.Content = "Meni";
        }

        private void btnPice_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCenralni, piceSelect);
            lblNaslov.Content = "Pice";
        }

        private void BtnSpecPonuda_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCenralni, specijalnaPonudaSelect);
            lblNaslov.Content = "Specijalna ponuda";
        }

        private void btnSto_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCenralni, stoSelect);
            lblNaslov.Content = "Sto";
        }

        //dole
        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;

            if (ucitanaTabela.Equals(deoRestoranaSelect))
            {
                prozor = new FrmDeoRestorana();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCenralni,deoRestoranaSelect);
            }
            else if (ucitanaTabela.Equals(jeloSelect))
            {
                prozor = new FrmJelo();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCenralni, jeloSelect);
            }
            else if (ucitanaTabela.Equals(konobarSelect))
            {
                prozor = new FrmKonobar();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCenralni, konobarSelect);
            }
            else if (ucitanaTabela.Equals(kuvarSelect))
            {
                prozor = new FrmKuvar();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCenralni, kuvarSelect);
            }
            else if (ucitanaTabela.Equals(meniSelect))
            {
                prozor = new FrmMeni();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCenralni, meniSelect);
            }
            else if (ucitanaTabela.Equals(piceSelect))
            {
                prozor = new FrmPice();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCenralni, piceSelect);
            }
            else if (ucitanaTabela.Equals(specijalnaPonudaSelect))
            {
                prozor = new FrmSpecijalnaPonuda();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCenralni, specijalnaPonudaSelect);
            }
            else if (ucitanaTabela.Equals(stoSelect))
            {
                prozor = new FrmSto();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCenralni, stoSelect);
            }
        }

        private void Izmena(DataGrid grid, string selectUslov)
        {
            try
            {
                konekcija.Open();
                azuriraj = true;
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                SqlCommand cmd = new SqlCommand { Connection = konekcija };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                cmd.CommandText = selectUslov + "@id";
                SqlDataReader citac = cmd.ExecuteReader();
                cmd.Dispose();
                if (citac.Read())
                {
                    if (ucitanaTabela.Equals(deoRestoranaSelect))
                    {
                        FrmDeoRestorana prozorDeoRestorana = new FrmDeoRestorana(azuriraj, red);
                        prozorDeoRestorana.txtNaziv.Text = citac["Naziv"].ToString();
                        prozorDeoRestorana.txtKapacitet.Text = citac["Kapacitet"].ToString();
                        prozorDeoRestorana.cbPusenje.IsChecked = (Boolean)citac["DozvoljenoPusenje"];
                        prozorDeoRestorana.cmbMeni.SelectedValue = citac["MeniID"].ToString();
                        prozorDeoRestorana.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(jeloSelect))
                    {
                        FrmJelo prozorJelo = new FrmJelo(azuriraj, red);
                        prozorJelo.cmbKuvar.SelectedValue = citac["KuvarID"].ToString();
                        prozorJelo.cmbMeni.SelectedValue = citac["MeniID"].ToString();
                        prozorJelo.txtNaziv.Text = citac["Naziv"].ToString();
                        prozorJelo.txtVrsta.Text = citac["Vrsta"].ToString();
                        prozorJelo.cbVegetarijansko.IsChecked = (Boolean)citac["Vegetarijansko"];
                        prozorJelo.txtCena.Text = citac["Cena"].ToString();
                        prozorJelo.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(konobarSelect))
                    {
                        FrmKonobar prozorKonobar = new FrmKonobar(azuriraj, red);
                        prozorKonobar.txtIme.Text = citac["Ime"].ToString();
                        prozorKonobar.txtPrezime.Text = citac["Prezime"].ToString();
                        prozorKonobar.cmbDeoRestorana.SelectedValue = citac["DeoRestoranaID"].ToString();
                        prozorKonobar.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(kuvarSelect))
                    {
                        FrmKuvar prozorKuvar = new FrmKuvar(azuriraj, red);
                        prozorKuvar.txtIme.Text = citac["Ime"].ToString();
                        prozorKuvar.txtPrezime.Text = citac["Prezime"].ToString();
                        prozorKuvar.cmbPoslasticar.IsChecked = (Boolean)citac["Poslasticar"];
                        prozorKuvar.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(meniSelect))
                    {
                        FrmMeni prozorMeni = new FrmMeni(azuriraj, red);
                        prozorMeni.txtAlergeni.Text = citac["Alergeni"].ToString();
                        prozorMeni.txtVrsta.Text = citac["VrstaKuhine"].ToString();
                        prozorMeni.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(piceSelect))
                    {
                        FrmPice prozorPice = new FrmPice(azuriraj, red);
                        prozorPice.cmbMeni.SelectedValue = citac["MeniID"].ToString();
                        prozorPice.txtNaziv.Text = citac["Naziv"].ToString();
                        prozorPice.txtCena.Text = citac["Cena"].ToString();
                        prozorPice.cbAlkoholno.IsChecked = (Boolean)citac["Alkoholno"];
                        prozorPice.txtZapremina.Text = citac["Zapremina"].ToString();
                        prozorPice.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(specijalnaPonudaSelect))
                    {
                        FrmSpecijalnaPonuda prozorSpecijalnaPonuda = new FrmSpecijalnaPonuda(azuriraj, red);
                        prozorSpecijalnaPonuda.txtNaziv.Text = citac["Naziv"].ToString();
                        prozorSpecijalnaPonuda.cmbMeni.SelectedValue = citac["MeniID"].ToString();
                        prozorSpecijalnaPonuda.dateTrajanje.SelectedDate = (DateTime)citac["TrajanjePonude"];
                        prozorSpecijalnaPonuda.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(stoSelect))
                    {
                        FrmSto prozorSto = new FrmSto(azuriraj, red);
                        prozorSto.cmbKonobar.SelectedValue = citac["KonobarID"];
                        prozorSto.txtKapacitet.Text = citac["Kapacitet"].ToString();
                        prozorSto.txtBrojStola.Text = citac["BrojStola"].ToString();
                        prozorSto.ShowDialog();
                    }
                }
            }
            catch(ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
                azuriraj = false;
            }
            
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if(ucitanaTabela.Equals(deoRestoranaSelect))
            {
                Izmena(dataGridCenralni, deorestoranaSelectUslov);
                UcitajPodatke(dataGridCenralni, deoRestoranaSelect);
            }
            else if (ucitanaTabela.Equals(jeloSelect))
            {
                Izmena(dataGridCenralni, jeloSelectUslov);
                UcitajPodatke(dataGridCenralni, jeloSelect);
            }
            else if (ucitanaTabela.Equals(konobarSelect))
            {
                Izmena(dataGridCenralni, konobarSelectUslov);
                UcitajPodatke(dataGridCenralni, konobarSelect);
            }
            else if (ucitanaTabela.Equals(kuvarSelectUslov))
            {
                Izmena(dataGridCenralni, kuvarSelectUslov);
                UcitajPodatke(dataGridCenralni, kuvarSelect);
            }
            else if (ucitanaTabela.Equals(meniSelect))
            {
                Izmena(dataGridCenralni, meniSelectUsov);
                UcitajPodatke(dataGridCenralni, meniSelect);
            }
            else if (ucitanaTabela.Equals(piceSelect))
            {
                Izmena(dataGridCenralni, piceSelectUslov);
                UcitajPodatke(dataGridCenralni, piceSelect);
            }
            else if (ucitanaTabela.Equals(specijalnaPonudaSelect))
            {
                Izmena(dataGridCenralni, specijalnaPonudaSelectUslov);
                UcitajPodatke(dataGridCenralni, specijalnaPonudaSelect);
            }
            else if (ucitanaTabela.Equals(stoSelect))
            {
                Izmena(dataGridCenralni, stoSelectUslov);
                UcitajPodatke(dataGridCenralni, stoSelect);
            }
        }

        private void Obrisi(DataGrid grid, string selectUslov)
        {
            try
            {
                konekcija.Open();
                var selectedRow = (DataRowView)grid.SelectedItem;
                MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni?", "UPOZORENJE", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(rezultat == MessageBoxResult.Yes)
                {
                    object a = selectedRow.Row.ItemArray[0];
                    int? id = (int?)a;
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = konekcija
                    };
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.CommandText = selectUslov + "@id";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch
            {
                MessageBox.Show("Ne može se obrisati element koji se koristi u drugoj tabeli kao strani kljuc!",
               "GREŠKA", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if(konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if(ucitanaTabela.Equals(deoRestoranaSelect))
            {
                Obrisi(dataGridCenralni, deorestoranaDelete);
                UcitajPodatke(dataGridCenralni, deoRestoranaSelect);
            }
            else if(ucitanaTabela.Equals(jeloSelect))
            {
                Obrisi(dataGridCenralni, jeloDelete);
                UcitajPodatke(dataGridCenralni, jeloSelect);
            }
            else if (ucitanaTabela.Equals(konobarSelect))
            {
                Obrisi(dataGridCenralni, konobarDelete);
                UcitajPodatke(dataGridCenralni, konobarSelect);
            }
            else if (ucitanaTabela.Equals(kuvarSelect))
            {
                Obrisi(dataGridCenralni, kuvarDelete);
                UcitajPodatke(dataGridCenralni, kuvarSelect);
            }
            else if (ucitanaTabela.Equals(meniSelect))
            {
                Obrisi(dataGridCenralni, meniDelete);
                UcitajPodatke(dataGridCenralni, meniSelect);
            }
            else if (ucitanaTabela.Equals(piceSelect))
            {
                Obrisi(dataGridCenralni, piceDelete);
                UcitajPodatke(dataGridCenralni, piceSelect);
            }
            else if (ucitanaTabela.Equals(specijalnaPonudaSelect))
            {
                Obrisi(dataGridCenralni, specijalnaPonudaDelete);
                UcitajPodatke(dataGridCenralni, specijalnaPonudaSelect);
            }
            else if (ucitanaTabela.Equals(stoSelect))
            {
                Obrisi(dataGridCenralni, stoDelete);
                UcitajPodatke(dataGridCenralni, stoSelect);
            }
        }
    }
}
