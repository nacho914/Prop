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
    /// Interaction logic for MainJuegos.xaml
    /// </summary>
    public partial class MainJuegos : Window
    {
        public int iIdTorneo;
        public List<partidos> items;

        public MainJuegos()
        {
            InitializeComponent();
            items = new List<partidos>();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            llenarComboJornadas();
            llenarComboPartidos();
            cargarEquipos();
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
                cmbjornadas.ItemsSource = listJornadas;
                cmbjornadas.SelectedValue = "key";
                cmbjornadas.DisplayMemberPath = "Value";
                cmbjornadas.SelectedIndex = 0;
            }
        }

        private void llenarComboPartidos()
        {
            Dictionary<string, string> listPartidos = new Dictionary<string, string>();
            string txt;

            KeyValuePair<string, string> listEquipo = (KeyValuePair<string, string>)cmbjornadas.SelectedItem;
            int idJornada = Int32.Parse(listEquipo.Key);

            using (var ctx = GetInstance())
            {
                var query = string.Format(" select id,(select nombre from Equipos where numeroequipo = idlocal) as local, (select nombre from Equipos where numeroequipo = idvisitante) as visitante from jornadas where idtorneo = {0} AND NumeroJornada = {1}", this.iIdTorneo, idJornada);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            txt = string.Format("{0} VS {1}", reader["local"].ToString(), reader["visitante"].ToString());
                            listPartidos.Add(reader["id"].ToString(), txt);                            
                        }
                    }
                }
                cmbPartidos.ItemsSource = listPartidos;
                cmbPartidos.SelectedValue = "key";
                cmbPartidos.DisplayMemberPath = "Value";
                cmbPartidos.SelectedIndex = 0;
            }
        }

        private void cargarEquipos()
        {

            KeyValuePair<string, string> listEquipo = (KeyValuePair<string, string>)cmbPartidos.SelectedItem;
            int idPartido = Int32.Parse(listEquipo.Key);

            using (var ctx = GetInstance())
            {
                var query = string.Format(" select idlocal,idvisitante FROM jornadas where id = {0}", idPartido);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cargarEquipoListView(int.Parse(reader["idlocal"].ToString()),1);

                        }
                    }
                }
 
            }
        }

        private void cargarEquipoListView(int iEquipo, int iTipo)
        {

            lvLocales.ItemsSource = null;
            
            using (var ctx = GetInstance())
            {
                var query = string.Format("SELECT a.id,a.nombre,a.paterno,a.numero FROM jugadores as a inner join Equipos as b on a.idEquipo = b.id where b.numeroequipo = {0} and b.idtorneo = {1}", iEquipo,iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new partidos() { numero = reader["numero"].ToString(), nombre = reader["nombre"].ToString(),apellidos= reader["paterno"].ToString(),goles=0,amarillas=0,rojas=0 });

                        }
                    }
                }

            }
            lvLocales.ItemsSource = items;

        }

    }

    public class partidos
    {
        public string numero { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public int goles { get; set; }
        public int amarillas { get; set; }
        public int rojas { get; set; }
    }

}



