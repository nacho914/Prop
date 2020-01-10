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

        public MainEquipos()
        {
            InitializeComponent();            

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
                txtNombreEquipo.IsEnabled = true;
                txtNombreEquipo.Focus();
            }
            else
            {                

            }
        }

        private void btnCreaEquipo_Click(object sender, RoutedEventArgs e)
        {
            AgregarEquipo();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            llenarCombo();
            llenarGrid();
        }

        public void llenarGrid()
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
        }

        public class Jugadores
        {
            public string nombre { get; set; }
            public string apellidoPaterno { get; set; }
            public string apellidoMaterno { get; set; }            
            public int numero { get; set; }
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
