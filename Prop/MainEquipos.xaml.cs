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
            /*items.Add(new User() { Name = "John Doe", Age = 42, Mail = "john@doe-family.com" });
            items.Add(new User() { Name = "Jane Doe", Age = 39, Mail = "jane@doe-family.com" });
            items.Add(new User() { Name = "Sammy Doe", Age = 7, Mail = "sammy.doe@gmail.com" });*/
            items.Add(new Jugadores() { nombre = "Juanito", apellidoPaterno = "Rubez", apellidoMaterno = "Rodriguez", numero = 10 });
            lvUsers.ItemsSource = items;

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
                var query = string.Format ("SELECT id, nombre FROM equipos where idtorneo ={0}",this.iIdTorneo);

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

        private void btnSelecciona_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<string, string> listEquipo = (KeyValuePair<string, string>)cmbEquipos.SelectedItem;
            int idEquipo = Int32.Parse(listEquipo.Key);

            if (idEquipo == 0)
            {
                HabilitaTxt(true);
            }
            else
            {
                
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
            AgregarEquipo();
            AgregarJugadores();
        }

        public bool AgregarEquipo()
        {
            bool bRegresa = false;

            using (var ctx = GetInstance())
            {
                string query = string.Format("INSERT INTO equipos(nombre,idTorneo) VALUES('{0}',{1})", txtNombreEquipo.Text,iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {

                    if (command.ExecuteNonQuery() == 1)
                    {
                        string txtMensaje = string.Format("Se creo correctamente el equipo '{0}'", txtNombreEquipo.Text);
                        MessageBox.Show(txtMensaje);
                        bRegresa = true;
                        cmbEquipos.ItemsSource = null;
                        llenarCombo();
                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal al crear el nuevo torneo");
                    }
                }
            }

            return bRegresa;
        }

        public bool AgregarJugadores()
        {
            bool bRegresa = false;

            using (var ctx = GetInstance())
            {
                foreach(Jugadores jugador in items)
                { 
                    string query = string.Format("INSERT INTO jugadores(nombre,paterno,materno,numero,idEquipo) VALUES" +
                        "('{0}','{1}','{2}',{3},{4})", jugador.nombre,jugador.apellidoPaterno,jugador.apellidoMaterno,jugador.numero,1);

                    using (var command = new SQLiteCommand(query, ctx))
                    {

                        if (command.ExecuteNonQuery() == 1)
                        {
                            string txtMensaje = string.Format("Se creo correctamente el equipo '{0}'", txtNombreEquipo.Text);
                            MessageBox.Show(txtMensaje);
                            bRegresa = true;
                            cmbEquipos.ItemsSource = null;
                            llenarCombo();
                        }
                        else
                        {
                            MessageBox.Show("Algo salio mal al crear el nuevo torneo");
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

      /*  public void llenarGrid()
        {
            DataGridTextColumn dgtcNombre = new DataGridTextColumn();
            DataGridTextColumn dgtcPa = new DataGridTextColumn();
            DataGridTextColumn dgtcMa = new DataGridTextColumn();
            DataGridTextColumn dgtcNu = new DataGridTextColumn();

            double valor = dgdEquipos.Width;

            dgtcNombre.Header = "Nombre";
            dgtcNombre.IsReadOnly = false;
            dgtcNombre.Binding = new Binding("nombre");
            dgtcNombre.Width = (valor/100)*30;
            dgdEquipos.Columns.Add(dgtcNombre);

            dgtcPa.Header = "Apellido Paterno";
            dgtcPa.Binding = new Binding("apellidoPaterno");
            dgtcPa.Width = (valor / 100) * 30;
            dgdEquipos.Columns.Add(dgtcPa);

            dgtcMa.Header = "Apellido Materno";
            dgtcMa.Binding = new Binding("apellidoMaterno");
            dgtcMa.Width = (valor / 100) * 30;
            dgdEquipos.Columns.Add(dgtcMa);

            dgtcNu.Header = "##";
            dgtcNu.Binding = new Binding("numero");
            dgtcNu.Width = (valor / 100) * 7;
            dgdEquipos.Columns.Add(dgtcNu);

            dgdEquipos.Items.Add(new Jugadores() { nombre = "Juanito", apellidoPaterno = "Rubez", apellidoMaterno="Rodriguez",numero= 10 });
        }*/

        public class Jugadores
        {
            public string nombre { get; set; }
            public string apellidoPaterno { get; set; }
            public string apellidoMaterno { get; set; }            
            public int numero { get; set; }
        }

        public class User
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public string Mail { get; set; }
        }

        private void lvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAgregarJugador_Click(object sender, RoutedEventArgs e)
        {
            lvUsers.ItemsSource = null;
            items.Add(new Jugadores() { nombre = txtNombreJugador.Text, apellidoPaterno = txApellidoPaterno.Text, apellidoMaterno = txtApellidoMaterno.Text, numero = System.Convert.ToInt32(txtNumero.Text) });
            lvUsers.ItemsSource = items;
        }

        private void cmbEquipos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbEquipos.SelectedIndex == 0)
                HabilitaTxt(false);


        }

        /*
        public class Item
        {
            public int Num { get; set; }
            public string Start { get; set; }
            public string Finich { get; set; }
        }

        private void generate_columns()
        {
            DataGridTextColumn c1 = new DataGridTextColumn();
            c1.Header = "Num";
            c1.Binding = new Binding("Num");
            c1.Width = 110;
            dataGrid1.Columns.Add(c1);
            DataGridTextColumn c2 = new DataGridTextColumn();
            c2.Header = "Start";
            c2.Width = 110;
            c2.Binding = new Binding("Start");
            dataGrid1.Columns.Add(c2);
            DataGridTextColumn c3 = new DataGridTextColumn();
            c3.Header = "Finich";
            c3.Width = 110;
            c3.Binding = new Binding("Finich");
            dataGrid1.Columns.Add(c3);

            dataGrid1.Items.Add(new Item() { Num = 1, Start = "2012, 8, 15", Finich = "2012, 9, 15" });
            dataGrid1.Items.Add(new Item() { Num = 2, Start = "2012, 12, 15", Finich = "2013, 2, 1" });
            dataGrid1.Items.Add(new Item() { Num = 3, Start = "2012, 8, 1", Finich = "2012, 11, 15" });

        }*/
    }
}
