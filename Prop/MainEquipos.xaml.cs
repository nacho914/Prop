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
    /// Interaction logic for MainEquipos.xaml
    /// </summary>
    public partial class MainEquipos : Window
    {
        public int iIdTorneo;
        public string sNombreTorneo;
        public List<Jugadores> items;


        public MainEquipos()
        {
            InitializeComponent();

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

        private void llenarCombo()
        {

            Dictionary<string, string> listTorneos = new Dictionary<string, string>();

            listTorneos.Add("0", "Agregar nuevo equipo");

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

        private void HabilitaTxt(bool bBabilita)
        {
            txtNombreEquipo.IsEnabled = bBabilita;

            if (bBabilita)
                txtNombreEquipo.Focus();

            txtNombreJugador.IsEnabled = bBabilita;
            txtApellidoMaterno.IsEnabled = bBabilita;
            txApellidoPaterno.IsEnabled = bBabilita;
            txtNumero.IsEnabled = bBabilita;
        }

        private void btnCreaEquipo_Click(object sender, RoutedEventArgs e)
        {
            int idEquipo = 0;
            if (cmbEquipos.SelectedIndex == 0)
            {
                AgregarEquipo();
                idEquipo = obtenerIdEquipo();
                AgregarJugadores(idEquipo);

                MessageBox.Show("El equipo ha sido creado");
            }
            else
            {
                KeyValuePair<string, string> listEquipo = (KeyValuePair<string, string>)cmbEquipos.SelectedItem;
                idEquipo = Int32.Parse(listEquipo.Key);

                AgregarJugadores(idEquipo);
                actualizarNombreEquipo(idEquipo);
                MessageBox.Show("El equipo ha sido modificado");

            }
            cmbEquipos.ItemsSource = null;
            llenarCombo();
        }

        public bool AgregarEquipo()
        {
            bool bRegresa = false;
            int iNumeroEquipo = ObtenerNumeroEquipo();

            using (var ctx = GetInstance())
            {
                string query = string.Format("INSERT INTO equipos(nombre,idTorneo,numeroequipo) VALUES('{0}',{1},{2})", txtNombreEquipo.Text, iIdTorneo, iNumeroEquipo);

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

        public void cargaEquipo(int iIdEquipo)
        {
            using (var ctx = GetInstance())
            {
                var query = string.Format("select id,nombre,paterno,materno,numero from jugadores where idequipo ={0}", iIdEquipo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //iIdEquipo = Int32.Parse(reader["id"].ToString());
                            items.Add(new Jugadores() { nombre = reader["nombre"].ToString(), apellidoPaterno = reader["paterno"].ToString(), apellidoMaterno = reader["materno"].ToString(), numero = Int32.Parse(reader["numero"].ToString()), id = Int32.Parse(reader["id"].ToString()) });
                        }
                    }
                }
            }
            lvUsers.ItemsSource = null;
            lvUsers.ItemsSource = items;

        }

        public int obtenerIdEquipo()
        {
            int iIdEquipo = 0;

            using (var ctx = GetInstance())
            {
                var query = string.Format("select id from equipos where idtorneo ={0} and nombre = '{1}'", this.iIdTorneo, txtNombreEquipo.Text);

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

        public void EliminarJugadores(int iIdEquipo)
        {
            using (var ctx = GetInstance())
            {
                var query = string.Format("DELETE from jugadores where idequipo ={0}", iIdEquipo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    if (command.ExecuteNonQuery() < 0)
                    {
                        MessageBox.Show("Algo salio mal al actualizar al equipo");
                    }
                }
            }
        }

        public void EliminarJugadoresIndividuales(int iIdJugador)
        {
            using (var ctx = GetInstance())
            {
                var query = string.Format("DELETE from jugadores where id ={0}", iIdJugador);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    if (command.ExecuteNonQuery() < 0)
                    {
                        MessageBox.Show("Algo salio mal al actualizar al equipo");
                    }
                }
            }
        }

        public void EliminarEquipo(int iIdEquipo)
        {
            using (var ctx = GetInstance())
            {
                var query = string.Format("DELETE from equipos where id ={0}", iIdEquipo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    if (command.ExecuteNonQuery() < 0)
                    {
                        MessageBox.Show("Algo salio mal al eliminar al equipo");
                    }
                }
            }
        }

        public bool AgregarJugadores(int iIdEquipo)
        {
            bool bRegresa = false;

            using (var ctx = GetInstance())
            {
                foreach (Jugadores jugador in items)
                {
                    //Si el jugador no cuenta con id se le inserta
                    if (jugador.id == 0)
                    {
                        if (jugador.eliminado == false)
                        { 
                            string query = string.Format("INSERT INTO jugadores(nombre,paterno,materno,numero,idEquipo) VALUES" +
                            "('{0}','{1}','{2}',{3},{4})", jugador.nombre, jugador.apellidoPaterno, jugador.apellidoMaterno, jugador.numero, iIdEquipo);

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
                    }
                    else//En caso de que si cuente, lo actualiza
                    {

                        if (jugador.eliminado == false)
                        {
                            string query = string.Format("UPDATE jugadores set nombre = '{0}',paterno ='{1}',materno='{2}',numero ={3}" +
                                " WHERE idequipo = {4} AND id = {5}", jugador.nombre, jugador.apellidoPaterno, jugador.apellidoMaterno, jugador.numero, iIdEquipo, jugador.id);

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
                        else
                        {
                            EliminarJugadoresIndividuales(jugador.id);
                        }
                    }

                }
            }

            return bRegresa;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            llenarCombo();
            //llenarGrid();
        }

        private void btnAgregarJugador_Click(object sender, RoutedEventArgs e)
        {
            int index = lvUsers.SelectedIndex;

            if (index == -1)
            {
                lvUsers.ItemsSource = null;
                items.Add(new Jugadores() { nombre = txtNombreJugador.Text, apellidoPaterno = txApellidoPaterno.Text, apellidoMaterno = txtApellidoMaterno.Text, numero = System.Convert.ToInt32(txtNumero.Text), id = 0 });
                lvUsers.ItemsSource = items;
                LimpiarTxt();

            }
            else
            {
                items[lvUsers.SelectedIndex].nombre = txtNombreJugador.Text;
                items[lvUsers.SelectedIndex].apellidoPaterno = txApellidoPaterno.Text;
                items[lvUsers.SelectedIndex].apellidoMaterno = txtApellidoMaterno.Text;
                items[lvUsers.SelectedIndex].numero = Int32.Parse(txtNumero.Text);
                lvUsers.ItemsSource = null;
                lvUsers.ItemsSource = items;
                lvUsers.SelectedIndex = index;
                LimpiarTxt();

            }

            btnAgregarJugador.Content = "Agregar Jugador";
            lvUsers.SelectedIndex = -1;
        }

        private void cmbEquipos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                KeyValuePair<string, string> listEquipo = (KeyValuePair<string, string>)cmbEquipos.SelectedItem;
                int idEquipo = Int32.Parse(listEquipo.Key);

                lvUsers.ItemsSource = null;
                items.Clear();


                if (idEquipo == 0)
                {
                    btnCreaEquipo.Content = "Crea al equipo";
                    txtNombreEquipo.Text = "";
                    btnEliminaEquipo.IsEnabled = false;
                }
                else
                {
                    btnCreaEquipo.Content = "Modifica el equipo";
                    btnEliminaEquipo.IsEnabled = true;
                    txtNombreEquipo.Text = listEquipo.Value;
                    cargaEquipo(Int32.Parse(listEquipo.Key));
                }
            }
            catch
            {

            }
        }

        private void lvUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Jugadores item = (Jugadores)lvUsers.SelectedItems[0];
            txtNombreJugador.Text = item.nombre;
            txApellidoPaterno.Text = item.apellidoPaterno;
            txtApellidoMaterno.Text = item.apellidoMaterno;
            txtNumero.Text = item.numero.ToString();

            btnAgregarJugador.Content = "Modificar Jugador";
        }

        public void LimpiarTxt()
        {
            txtNombreJugador.Text = "";
            txApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            txtNumero.Text = "";
        }

        private void btnEliminaEquipo_Click(object sender, RoutedEventArgs e)
        {
            int idEquipo = obtenerIdEquipo();
            EliminarEquipo(idEquipo);
            EliminarJugadores(idEquipo);

            llenarCombo();

            MessageBox.Show("El equipo ha sido eliminado");
        }

        private void actualizarNombreEquipo(int idEquipo)
        {

            using (var ctx = GetInstance())
            {
                string query = string.Format("UPDATE Equipos SET nombre = '{0}' WHERE id = {1} AND idTorneo = {2}", txtNombreEquipo.Text, idEquipo, iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {

                    if (command.ExecuteNonQuery() != 1)
                    {
                        MessageBox.Show("Algo salio mal al modificar el torneo");
                    }
                }

            }
        }

        public int ObtenerNumeroEquipo()
        {
            int iMaxId=0;

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

            return iMaxId+1;
        }


    }

    public class Jugadores
    {
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public int numero { get; set; }
        public int id { get; set; }
        public bool eliminado { get; set; }
    }
}
