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
using System.Text.RegularExpressions;

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
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        bool bJornadasGeneradas = false;

        public List<equipos> equipo;
        public List<Partidos> Partido;

        public MainEquipos()
        {
            InitializeComponent();

            equipo = new List<equipos>();
            items = new List<Jugadores>();
            Partido = new List<Partidos>();

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

        public int ActualizaNumeros()
        {
            int iIdEquipo = 0;
            int iNumeroEquipo = 1;


            foreach (KeyValuePair<string, string> dic in cmbEquipos.ItemsSource)
            {
                if(int.Parse(dic.Key) !=0)
                { 
                    actualizarNumeroEquipo(int.Parse(dic.Key), iNumeroEquipo);
                    iNumeroEquipo++;
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
            bJornadasGeneradas = VerificarJornadasGeneradas();

            if (bJornadasGeneradas)
            { 
                btnEliminaEquipo.IsEnabled = false;
                btnCreaEquipo.IsEnabled = false;
                btnGenerarJornadas.Visibility = Visibility.Hidden;
            }
            else
                btnGenerarJornadas.Visibility = Visibility.Visible;
        }

        private void btnAgregarJugador_Click(object sender, RoutedEventArgs e)
        {
            int index = lvUsers.SelectedIndex;

            try
            {

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
            catch
            {
                MessageBox.Show("Favor de validar que lleves todos los datos");
                lvUsers.ItemsSource = items;
            }
        }

        private void cmbEquipos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                KeyValuePair<string, string> listEquipo = (KeyValuePair<string, string>)cmbEquipos.SelectedItem;
                int idEquipo = Int32.Parse(listEquipo.Key);

                lvUsers.ItemsSource = null;
                items.Clear();


                if (bJornadasGeneradas)
                {
                    btnEliminaEquipo.IsEnabled = false;
                    if (idEquipo == 0)
                    {
                        btnCreaEquipo.Content = "Crea al equipo";
                        btnCreaEquipo.IsEnabled = false;
                        txtNombreEquipo.Text = "";

                    }
                    else
                    {
                        btnCreaEquipo.Content = "Modifica el equipo";
                        btnCreaEquipo.IsEnabled = true;
                        btnEliminaEquipo.IsEnabled = false;
                        txtNombreEquipo.Text = listEquipo.Value;
                        cargaEquipo(Int32.Parse(listEquipo.Key));
                    }
                }
                else
                {
                    if (idEquipo == 0)
                    {
                        btnCreaEquipo.Content = "Crea al equipo";
                        txtNombreEquipo.Text = "";
                        
                    }
                    else
                    {
                        btnCreaEquipo.Content = "Modifica el equipo";
                        btnEliminaEquipo.IsEnabled = true;
                        txtNombreEquipo.Text = listEquipo.Value;
                        cargaEquipo(Int32.Parse(listEquipo.Key));
                    }
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
            ActualizaNumeros();
            

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

        private void actualizarNumeroEquipo(int idEquipo,int iNumeroEquipo)
        {

            using (var ctx = GetInstance())
            {
                string query = string.Format("UPDATE Equipos SET numeroequipo = {0} WHERE id = {1} AND idTorneo = {2}", iNumeroEquipo, idEquipo, iIdTorneo);

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

        private void txtNumero_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.iIdTorneo = this.iIdTorneo;
            main.Show();
            this.Close();
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
                        if (reader.Read())
                        {
                            if (Int32.Parse(reader["jornadas"].ToString()) != 0)
                            {
                                bRegresar = true;
                            }
                        }
                    }
                }
            }

            return bRegresar;
        }

        private void btnGenerarJornadas_Click(object sender, RoutedEventArgs e)
        {
            if (!VerificarJornadasGeneradas())
            {
                MessageBoxResult result = MessageBox.Show("¿Estas seguro que deseas generar las jornadas? Después  de esto no podrás eliminar ni generar nuevos equipos",
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    obtenerEquipos();

                    if (!VerificarEquiposPares())
                        generarJornadasImpares();
                    else
                        generarJornadasPares();

                    guardarJornadas();
                    marcarJornadasTorneo();
                }

               
            }
            
        }

        public void obtenerEquipos()
        {
            int iContador = 0;
            using (var ctx = GetInstance())
            {
                var query = string.Format("select nombre from Equipos where idtorneo = {0} order by id", iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipo.Add(new equipos { nombre = reader["nombre"].ToString(), iEquipo = iContador });
                            iContador++;
                        }

                    }
                }
            }

        }

        public void generarJornadasPares()
        {
            int iRondas = equipo.Count() / 2;
            int iJornadas = equipo.Count() - 1;

            int iJuegosTotales = iRondas * iJornadas;
            int iJor = 1;
            int iJue = 1;

            for (int i = 1; i <= iJuegosTotales; i++)
            {
                Partido.Add((new Partidos { jornada = iJor, local = iJue, visitante = 0 }));
                iJue++;
                if (iJue > iJornadas)
                    iJue = 1;
                if (i % iRondas == 0)
                    iJor++;
            }

            int iAyudante = 0;

            foreach (Partidos x in Partido)
            {
                if (x.jornada != iAyudante)
                {
                    if (x.jornada % 2 == 0)
                    {
                        x.visitante = x.local;
                        x.local = equipo.Count();
                    }
                    else
                    {
                        x.visitante = equipo.Count();
                    }
                    iAyudante = x.jornada;
                }
            }

            int iMax = iJornadas;
            foreach (Partidos x in Partido)
            {
                if (x.local == 0 || x.visitante == 0)
                {
                    x.visitante = iMax;
                    iMax--;

                    if (iMax == 0)
                        iMax = iJornadas;
                }
            }
        }

        public void generarJornadasImpares()
        {
            int iRondas = ((equipo.Count() + 1) / 2);//(equipo.Count() * (equipo.Count() - 1)) / 2;
            int iJornadas = equipo.Count();

            int iJuegosTotales = iRondas * iJornadas;
            int iJor = 1;
            int iJue = 1;

            for (int i = 1; i <= iJuegosTotales; i++)
            {
                Partido.Add((new Partidos { jornada = iJor, local = iJue, visitante = -1 }));
                iJue++;
                if (iJue > (iJornadas))
                    iJue = 1;
                if (i % (iRondas) == 0)
                    iJor++;
            }

            int iAyudante = 0;

            foreach (Partidos x in Partido)
            {
                if (x.jornada != iAyudante)
                {
                    if (x.jornada % 2 == 0)
                    {
                        x.visitante = x.local;
                        x.local = 0;
                    }
                    else
                    {
                        x.visitante = 0;
                    }
                    iAyudante = x.jornada;
                }
            }

            int iMax = equipo.Count(); ;
            foreach (Partidos x in Partido)
            {
                if (x.local == -1 || x.visitante == -1)
                {
                    x.visitante = iMax;
                    iMax--;

                    if (iMax == 0)
                        iMax = iJornadas;
                }
            }
        }

        public void guardarJornadas()
        {
            using (var ctx = GetInstance())
            {

                foreach (Partidos par in Partido)
                {
                    string query = string.Format("INSERT INTO jornadas(numerojornada,idlocal,idvisitante,idtorneo) VALUES({0},{1},{2},{3})", par.jornada, par.local, par.visitante, iIdTorneo);

                    using (var command = new SQLiteCommand(query, ctx))
                    {

                        command.ExecuteNonQuery();

                    }
                }
            }
        }

        public void marcarJornadasTorneo()
        {
            using (var ctx = GetInstance())
            {
                string query = string.Format("UPDATE torneos SET jornadas = 1 WHERE id = {0}", iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    command.ExecuteNonQuery();
                }
            }

        }

        public bool VerificarEquiposPares()
        {
            bool bRegresar = false;
            int iTotal = 0;

            using (var ctx = GetInstance())
            {
                var query = string.Format("select count(*) as total from Equipos WHERE idTorneo = {0}", iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            iTotal = Int32.Parse(reader["total"].ToString());

                            if ((iTotal % 2) == 0 && iTotal != 0)
                            {
                                bRegresar = true;
                            }

                        }
                    }
                }
            }

            return bRegresar;
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

    public class equipos
    {
        public string nombre { get; set; }
        public int iEquipo { get; set; }
    }

    public class Partidos
    {
        public int jornada { get; set; }
        public int local { get; set; }
        public int visitante { get; set; }
    }
}
