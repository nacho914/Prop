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
    /// Interaction logic for MainJuegos.xaml
    /// </summary>
    public partial class MainJuegos : Window
    {
        public int iIdTorneo;
        public List<partidos> itemsLocal;
        public List<partidos> itemsVisita;
        int idLocal, idVisitante;
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text

        public MainJuegos()
        {
            InitializeComponent();
            itemsLocal = new List<partidos>();
            itemsVisita = new List<partidos>();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            llenarComboJornadas();
                        
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
                var query = string.Format("SELECT a.id AS id,(select nombre from Equipos where numeroequipo = a.idlocal AND idtorneo = a.idtorneo) as local, (select nombre from Equipos where numeroequipo = a.idvisitante AND idtorneo = a.idtorneo) as visitante FROM jornadas AS a where idtorneo = {0} AND NumeroJornada = {1} AND a.idvisitante <> 0 AND a.idlocal <> 0", this.iIdTorneo, idJornada);

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
            try { 
                KeyValuePair<string, string> listEquipo = (KeyValuePair<string, string>)cmbPartidos.SelectedItem;
                int idPartido = Int32.Parse(listEquipo.Key);

                using (var ctx = GetInstance())
                {
                    var query = string.Format("select a.idlocal AS idlocal,a.idvisitante AS idvisitante,(SELECT nombre FROM Equipos WHERE numeroequipo = a.idlocal AND idtorneo=a.idtorneo) as nomlocal, (SELECT nombre FROM Equipos WHERE numeroequipo = a.idvisitante AND idtorneo = a.idtorneo) as nomvisita, guardado FROM jornadas AS a where a.id = {0}", idPartido);

                    using (var command = new SQLiteCommand(query, ctx))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lblNomLocal.Content = reader["nomlocal"].ToString();
                                lblNomVis.Content = reader["nomvisita"].ToString();
                                cargarEquipoListView(int.Parse(reader["idlocal"].ToString()),1);
                                cargarEquipoListView(int.Parse(reader["idvisitante"].ToString()), 2);
                                if (int.Parse(reader["guardado"].ToString()) == 0)
                                {
                                    btnGuardaPartido.Content = "Guardar Partido";
                                    imgCheck.Source = new BitmapImage(
                                    new Uri(@"imagenes/nocheck.png", UriKind.Relative));
                                }
                                else
                                { 
                                    btnGuardaPartido.Content = "Modificar Partido";

                                    imgCheck.Source = new BitmapImage(
                                    new Uri(@"imagenes/check.png", UriKind.Relative));                                    
                                }
                            }
                        }
                    }
 
                }
                actualizaMarcador(1);
                actualizaMarcador(2);
            }
            catch
            { }
        }

        private void cargarEquipoListView(int iEquipo, int iTipo)
        {
            int idJugador = 0,iGoles=0,iAmarillas=0,iRojas=0;
            if (iTipo == 1)
            {
                lvLocales.ItemsSource = null;
                itemsLocal.Clear();
                idLocal = iEquipo;
            }
            else
            { 
                lvVisitas.ItemsSource = null;
                itemsVisita.Clear();
                idVisitante = iEquipo;
            }


            using (var ctx = GetInstance())
            {
                var query = string.Format("SELECT a.id,a.nombre,a.paterno,a.numero FROM jugadores as a inner join Equipos as b on a.idEquipo = b.id where b.numeroequipo = {0} and b.idtorneo = {1}", iEquipo,iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idJugador = int.Parse(reader["id"].ToString());

                            regresarDatosGuardados(idJugador, out iGoles, out iAmarillas, out iRojas);

                            if (iTipo==1)
                                itemsLocal.Add(new partidos() {id= idJugador, idEquipo= iEquipo, numero = reader["numero"].ToString(), nombre = reader["nombre"].ToString(),apellidos= reader["paterno"].ToString(),goles=iGoles,amarillas=iAmarillas,rojas=iRojas });
                            else
                                itemsVisita.Add(new partidos() {id = idJugador, idEquipo = iEquipo, numero = reader["numero"].ToString(), nombre = reader["nombre"].ToString(), apellidos = reader["paterno"].ToString(), goles = iGoles, amarillas = iAmarillas, rojas = iRojas });
                        }
                        
                    }
                }

            }
            if (iTipo == 1)
                lvLocales.ItemsSource = itemsLocal;
            else
                lvVisitas.ItemsSource = itemsVisita;

        }

        private void cmbjornadas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            llenarComboPartidos();
        }

        private void cmbPartidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            itemsLocal.Clear();
            itemsVisita.Clear();
            cargarEquipos();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Validar que solo pongan numeros y no vacios.
            //TextBox valor = (TextBox)sender;            
        }

        private void actualizaMarcador(int iTipo)
        {
            int totalGoles = 0;
            if (iTipo == 1)
            {
                for (int i = 0; i < lvLocales.Items.Count; i++)
                {
                    partidos line = (partidos)lvLocales.Items[i];

                    totalGoles += line.goles;
                }

                lblMarcaLocal.Content = totalGoles.ToString();
            }
            else
            {
                for (int i = 0; i <lvVisitas.Items.Count; i++)
                {
                    partidos line = (partidos)lvVisitas.Items[i];

                    totalGoles += line.goles;
                }

                lblMarcaVisita.Content = totalGoles.ToString();
            }            
        }

        private int guardarDetallesJornadas(int iTipo,int idPartido)
        {

            int totalGoles = 0;

            if (iTipo == 1)
            {
                for (int i = 0; i < lvLocales.Items.Count; i++)
                {
                    partidos jugador = (partidos)lvLocales.Items[i];

                    if(jugador.goles != 0)
                    {
                        guardarDetallesGoles(jugador, idPartido,iTipo);
                        totalGoles += jugador.goles;
                    }
                    if(jugador.amarillas != 0)
                    {
                        guardarDetallesAmarillas(jugador, idPartido);
                    }
                    if(jugador.rojas != 0)
                    {
                        guardarDetallesRojas(jugador, idPartido);
                    }

                }

                lblMarcaLocal.Content = totalGoles.ToString();
            }
            else
            {
                for (int i = 0; i < lvVisitas.Items.Count; i++)
                {
                    partidos jugador = (partidos)lvVisitas.Items[i];

                    if (jugador.goles != 0)
                    {
                        guardarDetallesGoles(jugador, idPartido,iTipo);
                        totalGoles += jugador.goles;
                    }
                    if (jugador.amarillas != 0)
                    {
                        guardarDetallesAmarillas(jugador, idPartido);
                    }
                    if (jugador.rojas != 0)
                    {
                        guardarDetallesRojas(jugador, idPartido);
                    }
                }

                lblMarcaVisita.Content = totalGoles.ToString();
            }

            return totalGoles;
        }

        public void guardarDetallesGoles(partidos par,int iJornada,int iTipo)
        {
            int iEquipoContra;

            if (iTipo == 1)
                iEquipoContra = idVisitante;
            else
                iEquipoContra = idLocal;
            
            using (var ctx = GetInstance())
            {
                string query = string.Format("INSERT INTO DetallesGoles(idjugador,idjornada,idtorneo,idequipo,goles,idEquipoContra) VALUES({0},{1},{2},{3},{4},{5})", par.id, iJornada, iIdTorneo, par.idEquipo, par.goles, iEquipoContra);

                using (var command = new SQLiteCommand(query, ctx))
                {

                    if (!(command.ExecuteNonQuery() == 1))
                    {
                        MessageBox.Show("Algo salio mal al guardar los goles");
                    }

                }
            }
        }

        public void guardarDetallesAmarillas(partidos par, int iJornada)
        {

            using (var ctx = GetInstance())
            {
                string query = string.Format("INSERT INTO DetallesAmarillas(idjugador,idjornada,idtorneo,idequipo,tarjetas) VALUES({0},{1},{2},{3},{4})", par.id, iJornada, iIdTorneo, par.idEquipo, par.amarillas);

                using (var command = new SQLiteCommand(query, ctx))
                {

                    if (!(command.ExecuteNonQuery() == 1))
                    {
                        MessageBox.Show("Algo salio mal al guardar los goles");
                    }

                }
            }
        }

        public void guardarDetallesRojas(partidos par, int iJornada)
        {

            using (var ctx = GetInstance())
            {
                string query = string.Format("INSERT INTO DetallesRojas(idjugador,idjornada,idtorneo,idequipo,tarjetas) VALUES({0},{1},{2},{3},{4})", par.id, iJornada, iIdTorneo, par.idEquipo, par.rojas);

                using (var command = new SQLiteCommand(query, ctx))
                {

                    if (!(command.ExecuteNonQuery() == 1))
                    {
                        MessageBox.Show("Algo salio mal al guardar los goles");
                    }

                }
            }
        }



        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            actualizaMarcador(1);

        }

        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            actualizaMarcador(2);
        }

        private void btnGuardaPartido_Click(object sender, RoutedEventArgs e)
        {
            guardarResultado();
        }

        private void guardarResultado()
        {
            KeyValuePair<string, string> listEquipo = (KeyValuePair<string, string>)cmbPartidos.SelectedItem;
            int idPartido = Int32.Parse(listEquipo.Key);

            if(btnGuardaPartido.Content.Equals("Modificar Partido"))
            {
                eliminarDatosPrevios(idPartido);
            }

            int iGolesLocales = guardarDetallesJornadas(1, idPartido);
            int iGolesVisitantes = guardarDetallesJornadas(2, idPartido);
            int iGanador = 0;

            if (iGolesLocales > iGolesVisitantes)
                iGanador = idLocal;
            else if (iGolesLocales < iGolesVisitantes)
                iGanador = idVisitante;

            using (var ctx = GetInstance())
            {
                string query = string.Format("UPDATE jornadas SET guardado=1,idganador = {0} WHERE id ={1} AND idtorneo= {2}", iGanador, idPartido, iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {

                    if (command.ExecuteNonQuery() != 1)
                    {
                        MessageBox.Show("Algo salio mal al modificar el torneo");
                    }
                    else
                    {
                        imgCheck.Source = new BitmapImage(
                                    new Uri(@"imagenes/check.png", UriKind.Relative));
                    }
                }

            }

            if (btnGuardaPartido.Content.Equals("Modificar Partido"))
            {
                MessageBox.Show("Se modifico el resultado del partido");
            }
        }

        private void eliminarDatosPrevios(int idPartido)
        {
            eliminarGoles(idPartido);
            eliminarAmarillas(idPartido);
            eliminarRojas(idPartido);
        }

        private void eliminarGoles(int idPartido)
        {
            using (var ctx = GetInstance())
            {
                string query = string.Format("DELETE FROM DetallesGoles WHERE idjornada = {0} AND idtorneo = {1}",idPartido, iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void eliminarAmarillas(int idPartido)
        {
            using (var ctx = GetInstance())
            {
                string query = string.Format("DELETE FROM DetallesAmarillas WHERE idjornada = {0} AND idtorneo = {1}", idPartido, iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void eliminarRojas(int idPartido)
        {
            using (var ctx = GetInstance())
            {
                string query = string.Format("DELETE FROM DetallesRojas WHERE idjornada = {0} AND idtorneo = {1}", idPartido, iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text,1);
        }

        private static bool IsTextAllowed(string text, int iTipo)
        {
            bool bRegresa = false;
            if (iTipo == 1)
                return !_regex.IsMatch(text);
            else if (iTipo == 2)
            {
                if (text == "0" ||text == "1" || text == "2")
                {
                    return true;
                }
                else
                    return false;
            }                
            else if (iTipo == 3)
            {
                if (text == "0" || text == "1" )
                {
                    return true;
                }
                else
                    return false;
            }
                

            return bRegresa;
        }

        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text,1);
        }

        private void TextBox_PreviewTextInput_2(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text, 2);
        }

        private void TextBox_PreviewTextInput_3(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text, 3);
        }

        private void TextBox_PreviewTextInput_4(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text, 3);
        }

        private void TextBox_PreviewTextInput_5(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text, 2);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.iIdTorneo = this.iIdTorneo;
            main.Show();
            this.Close();
        }

        private void regresarDatosGuardados(int idJugador,out int goles, out int amarillas,out int rojas)
        {
            goles = 0;
            amarillas = 0;
            rojas = 0;

            KeyValuePair<string, string> listEquipo = (KeyValuePair<string, string>)cmbPartidos.SelectedItem;
            int idPartido = Int32.Parse(listEquipo.Key);

            using (var ctx = GetInstance())
            {
                string query = string.Format("SELECT COALESCE(g.goles, 0) AS goles, COALESCE(a.tarjetas, 0) AS amarillas,  COALESCE(r.tarjetas, 0) AS rojas FROM jugadores AS j LEFT JOIN DetallesGoles AS g ON j.id = g.idjugador LEFT JOIN DetallesAmarillas as a ON j.id = a.idjugador AND g.idjornada = a.idjornada LEFT JOIN DetallesRojas as r ON j.id = r.idjugador and g.idjornada = r.idjornada WHERE j.id = {0} AND g.idjornada = {1} ", idJugador, idPartido);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            goles = int.Parse(reader["goles"].ToString());
                            amarillas = int.Parse(reader["amarillas"].ToString());
                            rojas = int.Parse(reader["rojas"].ToString());
                        }

                    }

                }
            }
        }
    }

    public class partidos
    {
        public int id { get; set; }
        public int idEquipo { get; set; }
        public string numero { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public int goles { get; set; }
        public int amarillas { get; set; }
        public int rojas { get; set; }
    }

}



