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
    /// Lógica de interacción para MainImportar.xaml
    /// </summary>
    public partial class MainImportar : Window
    {

        public List<Jugadores> items;
        public int iIdTorneo;

        public MainImportar()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            llenarComboTorneos();
            items = new List<Jugadores>();
        }

        public static SQLiteConnection GetInstance()
        {
            var db = new SQLiteConnection(
                string.Format("Data Source={0};Version=3;", "Prop.db")
            );

            db.Open();

            return db;
        }

        private void llenarComboTorneos()
        {

            Dictionary<string, string> listTorneos = new Dictionary<string, string>();

            listTorneos.Add("0", "Selecciona un torneo");

            using (var ctx = GetInstance())
            {
                var query = "SELECT id, nombre FROM torneos where id <>"+iIdTorneo;

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
                cmbTorneo.ItemsSource = listTorneos;
                cmbTorneo.SelectedValue = "key";
                cmbTorneo.DisplayMemberPath = "Value";
                cmbTorneo.SelectedIndex = 0;
            }
        }

        private void cmbTorneo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // KeyValuePair<string, string> listTorneo = (KeyValuePair<string, string>)cmbTorneo.SelectedIndex;

            if (cmbTorneo.SelectedIndex != 0)
            {
                cmbEquipo.IsEnabled = true;
                llenarComboEquipos();
            }
            else
            {
                cmbEquipo.IsEnabled = false;
                //btnImportar.IsEnabled = false;
                cmbEquipo.SelectedIndex = 0;
            }
        }

        private void llenarComboEquipos()
        {

            Dictionary<string, string> listTorneos = new Dictionary<string, string>();

            listTorneos.Add("0", "Selecciona un equipo");
            KeyValuePair<string, string> listTorneo = (KeyValuePair<string, string>)cmbTorneo.SelectedItem;
            int idTorSeleccionado = Int32.Parse(listTorneo.Key);

            using (var ctx = GetInstance())
            {
                var query = string.Format("SELECT id, nombre FROM equipos where idtorneo ={0} AND nombre NOT IN(SELECT nombre FROM Equipos WHERE idtorneo = {1})", idTorSeleccionado, iIdTorneo);

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
                cmbEquipo.ItemsSource = listTorneos;
                cmbEquipo.SelectedValue = "key";
                cmbEquipo.DisplayMemberPath = "Value";
                cmbEquipo.SelectedIndex = 0;
            }
        }

        private void cmbEquipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbEquipo.SelectedIndex != 0)
            { 
                btnImportar.IsEnabled = true;
            }
            else
            {
                btnImportar.IsEnabled = false;
            }
        }

        private void importarEquipo()
        {
            //Dictionary<string, string> listEquipo = new Dictionary<string, string>();

            //listTorneos.Add("0", "Agregar nuevo equipo");
            KeyValuePair<string, string> listEquipo = (KeyValuePair<string, string>)cmbEquipo.SelectedItem;
            using (var ctx = GetInstance())
            {
                var query = string.Format("select id,nombre,paterno,materno,numero from jugadores where idequipo ={0}", listEquipo.Key);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new Jugadores() { nombre = reader["nombre"].ToString(), apellidoPaterno = reader["paterno"].ToString(), apellidoMaterno = reader["materno"].ToString(), numero = Int32.Parse(reader["numero"].ToString()), id = Int32.Parse(reader["id"].ToString()) });
                            //agregarJugadores(reader["nombre"].ToString(), reader["paterno"].ToString(), reader["materno"].ToString(), Int32.Parse(reader["id"].ToString()));

                        }
                    }
                }
            }
        }

        private void agregarJugadores(string nombre, string apellidoP, string apellidoM, int iNumero)
        {

            using (var ctx = GetInstance())
            {
                string query = string.Format("INSERT INTO jugadores(nombre,paterno,materno,numero,idEquipo) VALUES" +
                "('{0}','{1}','{2}',{3},{4})", nombre, apellidoP, apellidoM, iNumero, 99999);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    command.ExecuteNonQuery();
                }
            }
            
        }

        private void cargarJugadores(int iIdEquipo)
        {
            using (var ctx = GetInstance())
            { 
                foreach (Jugadores jugador in items)
                {
                    //Si el jugador no cuenta con id se le inserta

                    string query = string.Format("INSERT INTO jugadores(nombre,paterno,materno,numero,idEquipo) VALUES" +
                    "('{0}','{1}','{2}',{3},{4})", jugador.nombre, jugador.apellidoPaterno, jugador.apellidoMaterno, jugador.numero, iIdEquipo);

                    using (var command = new SQLiteCommand(query, ctx))
                    {

                        command.ExecuteNonQuery();

                    }

                }
            }
        }

        private void btnImportar_Click(object sender, RoutedEventArgs e)
        {
            int idEquipo = 0;

            AgregarEquipo();
            idEquipo = obtenerIdEquipo();
            importarEquipo();
            cargarJugadores(idEquipo);
            MessageBox.Show("El equipo ha sido importado");
            cmbTorneo.SelectedIndex = 0;
        }

        public bool AgregarEquipo()
        {
            bool bRegresa = false;
            int iNumeroEquipo = ObtenerNumeroEquipo();
            KeyValuePair<string, string> listEquipo = (KeyValuePair<string, string>)cmbEquipo.SelectedItem;

            using (var ctx = GetInstance())
            {
                string query = string.Format("INSERT INTO equipos(nombre,idTorneo,numeroequipo) VALUES('{0}',{1},{2})", listEquipo.Value, iIdTorneo, iNumeroEquipo);

                using (var command = new SQLiteCommand(query, ctx))
                {

                    if (command.ExecuteNonQuery() == 1)
                    {
                        bRegresa = true;

                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal al crear el nuevo torneo");
                    }
                }
            }

            return bRegresa;
        }

        public int ObtenerNumeroEquipo()
        {
            int iMaxId = 0;

            using (var ctx = GetInstance())
            {
                var query = string.Format("select count(*) AS total FROM Equipos WHERE idtorneo ={0}", iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //iIdEquipo = Int32.Parse(reader["id"].ToString());
                            iMaxId = Int32.Parse(reader["total"].ToString());

                        }
                    }
                }
            }

            return iMaxId + 1;
        }

        public int obtenerIdEquipo()
        {
            int iIdEquipo = 0;
            KeyValuePair<string, string> listEquipo = (KeyValuePair<string, string>)cmbEquipo.SelectedItem;

            using (var ctx = GetInstance())
            {
                var query = string.Format("select id from equipos where idtorneo ={0} and nombre = '{1}'", this.iIdTorneo, listEquipo.Value);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            iIdEquipo = Int32.Parse(reader["id"].ToString());
                        }
                    }
                }
            }

            return iIdEquipo;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainEquipos main = new MainEquipos();
            main.iIdTorneo = this.iIdTorneo;
            main.Show();
            this.Close();
        }
    }
}
