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
using System.Data.SQLite;


namespace Prop
{
    /// <summary>
    /// Interaction logic for MainJornadas.xaml
    /// </summary>
    public partial class MainJornadas : Window
    {
        public int iIdTorneo;        
        
        public List<lvJornadas> listJornadas;
        int iCargo = 0;

        public MainJornadas()
        {
            InitializeComponent();            
            
            listJornadas = new List<lvJornadas>();            
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Si las jornadas aun no se han generado, se tienen que realizar.

            
            llenarComboJornadas();
            cargarJornada();
            iCargo = 1;

        }


        public bool VerificarJornadasGeneradas()
        {
            bool bRegresar = false;

            using (var ctx = GetInstance())
            {
                var query = string.Format("SELECT jornadas FROM torneos WHERE id = {0}", iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                           if( Int32.Parse(reader["jornadas"].ToString()) != 0)
                           {
                                bRegresar = true;
                           }
                        }
                    }
                }               
            }

            return bRegresar;
        }


        public static SQLiteConnection GetInstance()
        {
            var db = new SQLiteConnection(
                string.Format("Data Source={0};Version=3;", "Prop.db")
            );

            db.Open();

            return db;
        }

        private void btn_Jornadas_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void llenarComboJornadas()
        {
            Dictionary<string, string> listJornadas = new Dictionary<string, string>();
            string txt;
            //listTorneos.Add("0", "Agregar nuevo equipo");

            using (var ctx = GetInstance())
            {
                var query = string.Format("SELECT DISTINCT(NumeroJornada) as jornada FROM jornadas WHERE idtorneo ={0} ORDER BY NumeroJornada", this.iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            txt = string.Format("Jornada {0}", reader["jornada"].ToString());
                            listJornadas.Add(reader["jornada"].ToString(),txt);
                        }
                    }
                }
                cmb_jornadas.ItemsSource = listJornadas;
                cmb_jornadas.SelectedValue = "key";
                cmb_jornadas.DisplayMemberPath = "Value";
                cmb_jornadas.SelectedIndex = 0;
            }
        }

        public void cargarJornada()
        {

            int iJornada = cmb_jornadas.SelectedIndex+1;
            string local="", visitante="", sTexto="";

            using (var ctx = GetInstance())
            {

                var query = string.Format("SELECT a.idlocal as idlocal ,a.idvisitante as idvisitante, COALESCE((SELECT sum(goles) FROM DetallesGoles WHERE idequipo = a.idlocal AND idjornada = a.id), 0) AS Goleslocal, COALESCE((SELECT sum(goles) FROM DetallesGoles WHERE idequipo = a.idvisitante AND idjornada = a.id),0) AS Golesvisita, guardado FROM jornadas as a WHERE idtorneo ={0} AND NumeroJornada = {1}", this.iIdTorneo, iJornada);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            local = obtenerNombreEquipo(Int32.Parse(reader["idlocal"].ToString()));
                            visitante = obtenerNombreEquipo(Int32.Parse(reader["idvisitante"].ToString()));

                            if (Int32.Parse(reader["guardado"].ToString()) == 1)
                                sTexto = "VS";
                            else
                                sTexto = "-";

                            listJornadas.Add(new lvJornadas() { local = local, visitante = visitante, marlocal = reader["Goleslocal"].ToString(), marvisitante = reader["Golesvisita"].ToString(), vs = sTexto });

                        }   
                    }
                }
                lvJornadasPrincipal.ItemsSource = null;
                lvJornadasPrincipal.ItemsSource = listJornadas;                

            }
              
        }

        public string obtenerNombreEquipo(int idEquipo)
        {
            string nombre = "";

            using (var ctx = GetInstance())
            {
                
                var query = string.Format("SELECT nombre FROM Equipos WHERE idtorneo ={0} AND numeroequipo = {1}", this.iIdTorneo, idEquipo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            nombre = reader["nombre"].ToString();
                        }
                    }
                }

            }

            return nombre;
        }

        private void cmb_jornadas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(iCargo == 1)
            { 
                listJornadas.Clear();
                cargarJornada();
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.iIdTorneo = this.iIdTorneo;
            main.Show();
            this.Close();
        }
    }

    public class lvJornadas
    {
        public string local { get; set; }
        public string marlocal { get; set; }
        public string marvisitante { get; set;}
        public string visitante { get; set; }
        public string vs { get; set; }

    }
}
