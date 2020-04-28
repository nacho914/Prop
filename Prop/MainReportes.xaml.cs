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
    /// Interaction logic for MainReportes.xaml
    /// </summary>
    public partial class MainReportes : Window
    {
        public int iIdTorneo;
        public List<TablaEquipos> itemsTabla;
        public List<goleadores> itemsGoleo;
        public List<lvJornadas> listJornadas;
        public List<tablaEquiposjugadores> lEquiJug;
        public List<equipos> equipo;

        public MainReportes()
        {
            listJornadas = new List<lvJornadas>();
            lEquiJug = new List<tablaEquiposjugadores>();
            InitializeComponent();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var rpt = new IListPdfReport().CreatePdfReport(iIdTorneo, itemsTabla);
            var Repo = string.Format("El reporte de la liga se ha generado correctamente en la siguiente ruta: \n {0}", System.AppDomain.CurrentDomain.BaseDirectory + "\\Pdf");
            MessageBox.Show(Repo);            
        }

        private void imgRepoGoleo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var rpt = new CReporteGoleo().CreatePdfReport(iIdTorneo, itemsGoleo);
            var Repo = string.Format("El reporte de goleo se ha generado correctamente en la siguiente ruta: \n {0}", System.AppDomain.CurrentDomain.BaseDirectory + "\\Pdf");
            MessageBox.Show(Repo);
        }

        private void imgRegresa_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.iIdTorneo = this.iIdTorneo;
            main.Show();
            this.Close();
        }

        public static SQLiteConnection GetInstance()
        {
            var db = new SQLiteConnection(
                string.Format("Data Source={0};Version=3;", "Prop.db")
            );

            db.Open();

            return db;
        }

        private void llenarComboJornadas()
        {
            Dictionary<string, string> listJornadas = new Dictionary<string, string>();
            string txt;            

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
                            listJornadas.Add(reader["jornada"].ToString(), txt);
                        }
                    }
                }
                cmbJornada.ItemsSource = listJornadas;
                cmbJornada.SelectedValue = "key";
                cmbJornada.DisplayMemberPath = "Value";
                cmbJornada.SelectedIndex = 0;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            llenarComboJornadas();
            llenarComboEquipos();
        }

        public void cargarJornada()
        {

            int iJornada = cmbJornada.SelectedIndex + 1;
            string local = "", visitante = "", sTexto = "";

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
            }

        }

        public void cargarJugadores(int iIdEquipo)
        {                     
            using (var ctx = GetInstance())
            {
                var query = string.Format("SELECT a.nombre || \" \" || a.paterno || \" \" || a.materno as nombre ,coalesce((select sum(goles) from DetallesGoles  WHERE idjugador = a.id),0) AS goles, coalesce((select sum(tarjetas) from DetallesAmarillas WHERE idjugador = a.id),0) AS amarillas, coalesce((select sum(tarjetas) from DetallesRojas WHERE idjugador = a.id),0) AS rojas FROM jugadores AS a WHERE a.idequipo = {0} ORDER BY goles DESC, amarillas DESC, rojas DESC", iIdEquipo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {      
                            lEquiJug.Add(new tablaEquiposjugadores() { sNombre = reader["nombre"].ToString(),iGoles = Int32.Parse(reader["goles"].ToString()),iAmarillas = Int32.Parse(reader["amarillas"].ToString()), iRojas = Int32.Parse(reader["rojas"].ToString())});
                        }
                    }
                }
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
                        if (reader.Read())
                        {
                            nombre = reader["nombre"].ToString();
                        }
                    }
                }

            }

            return nombre;
        }

        private void imgRepoGoleoJornada_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int iJornada = cmbJornada.SelectedIndex + 1;

            var rpt = new CReporteJornadas().CreatePdfReport(iIdTorneo, iJornada, listJornadas);
            var Repo = string.Format("El reporte de Jornada se ha generado correctamente en la siguiente ruta: \n {0}", System.AppDomain.CurrentDomain.BaseDirectory + "\\Pdf");
            MessageBox.Show(Repo);            
        }

        private void cmbJornada_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listJornadas.Clear();
            cargarJornada();
        }

        private void llenarComboEquipos()
        {

            Dictionary<string, string> listTorneos = new Dictionary<string, string>();

            using (var ctx = GetInstance())
            {
                var query = string.Format("SELECT id, nombre FROM equipos where idtorneo ={0}", this.iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listTorneos.Add(reader["id"].ToString(), reader["nombre"].ToString());
                        }
                    }
                }
                cmbEquipos.ItemsSource = listTorneos;
                cmbEquipos.SelectedValue = "key";
                cmbEquipos.DisplayMemberPath = "Value";
                cmbEquipos.SelectedIndex = 0;
            }
        }

        private void imgRepoEquipos_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyValuePair<string, string> listEquipo = (KeyValuePair<string, string>)cmbEquipos.SelectedItem;
            int idEquipo = Int32.Parse(listEquipo.Key);
            lEquiJug.Clear();
            cargarJugadores(idEquipo);


            var rpt = new CReporteEquipos().CreatePdfReport(iIdTorneo, listEquipo.Value, lEquiJug);
            var Repo = string.Format("El reporte de Equipo se ha generado correctamente en la siguiente ruta: \n {0}", System.AppDomain.CurrentDomain.BaseDirectory + "\\Pdf");
            MessageBox.Show(Repo);            
        }
    }

    public class tablaEquiposjugadores
    {        
        public string sNombre { get; set; }
        public int iGoles { get; set; }
        public int iAmarillas { get; set; }
        public int iRojas { get; set; }

    }

}
