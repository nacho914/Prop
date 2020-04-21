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
    /// Interaction logic for MainTorneo.xaml
    /// </summary>
    public partial class MainTorneo : Window
    {
        public MainTorneo()
        {
            InitializeComponent();

            llenarCombo();                        

        }
       
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           /*
            //Forma para sacar los datos del combobox
            ComboBox senderComboBox = (ComboBox)sender;            
            KeyValuePair<string, string> typeItem = (KeyValuePair<string, string>)senderComboBox.SelectedItem;
            string value = typeItem.Key;
            MessageBox.Show(value);
            */
        }

        private void btnAceptarTorneo_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<string, string> listTorneo = (KeyValuePair<string, string>)cmbtorneo.SelectedItem;
            int iIdTorneo = Int32.Parse(listTorneo.Key);
            
            if(iIdTorneo == 0)
            {
                addTorneos agregarTorneo = new addTorneos();
                agregarTorneo.Show();
                this.Close();
            }
            else
            {
                //MessageBox.Show(value);
                MainWindow main = new MainWindow();
                main.iIdTorneo = iIdTorneo;                
                main.Show();
                this.Close();

            }
        }



        private void btnEliminarTorneo_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<string, string> listTorneo = (KeyValuePair<string, string>)cmbtorneo.SelectedItem;
            string value = listTorneo.Key;

            if (value == "0")
            {
                MessageBox.Show("Favor de seleccionar un torneo a eliminar");
            }
            else
            {
                EliminarTorneo(Int32.Parse(value));
                EliminarJornadas(Int32.Parse(value));
                EliminarEquipos(Int32.Parse(value));
                EliminarGoles(Int32.Parse(value));
            }
        }


        private void btnModificaTorneo_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<string, string> listTorneo = (KeyValuePair<string, string>)cmbtorneo.SelectedItem;
            string value = listTorneo.Key;

            if (value == "0")
            {
                MessageBox.Show("Favor de seleccionar un torneo a Modificar");
            }
            else
            {
                addTorneos modificarTorneo = new addTorneos();
                modificarTorneo.iIdTorneo = Convert.ToInt32(value);
                modificarTorneo.sNombreTorneoAnterior = listTorneo.Value.ToString();
                modificarTorneo.Show();
                this.Close();
            }
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

            listTorneos.Add("0", "Agregar nuevo torneo");

            using (var ctx = GetInstance())
            {
                var query = "SELECT id, nombre FROM torneos";

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
                cmbtorneo.ItemsSource = listTorneos;
                cmbtorneo.SelectedValue = "key";
                cmbtorneo.DisplayMemberPath = "Value";
                cmbtorneo.SelectedIndex = 0;
            }
        }


        public void EliminarTorneo(int iTorneo)
        {
            using (var ctx = GetInstance())
            {
                string query = string.Format("DELETE FROM torneos WHERE id = {0}", iTorneo);
                using (var command = new SQLiteCommand(query, ctx))
                {
                    if (command.ExecuteNonQuery() > 0)
                    {
                        string txtMensaje = string.Format("Se elimino correctamente el torneo");
                        MessageBox.Show(txtMensaje);
                        cmbtorneo.ItemsSource = null;
                        llenarCombo();
                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal al eliminar el torneo");
                    }
                }
            }

        }

        public void EliminarJornadas(int iTorneo)
        {
            using (var ctx = GetInstance())
            {
                string query = string.Format("DELETE FROM jornadas WHERE idtorneo = {0}", iTorneo);
                using (var command = new SQLiteCommand(query, ctx))
                {
                    command.ExecuteNonQuery();

                }
            }
        }

        public void EliminarEquipos(int iTorneo)
        {
            using (var ctx = GetInstance())
            {
                string query = string.Format("DELETE FROM Equipos WHERE idtorneo = {0}", iTorneo);
                using (var command = new SQLiteCommand(query, ctx))
                {
                    command.ExecuteNonQuery();

                }
            }
        }


        public void EliminarGoles(int iTorneo)
        {
            using (var ctx = GetInstance())
            {
                string query = string.Format("DELETE FROM DetallesGoles WHERE idtorneo = {0}", iTorneo);
                using (var command = new SQLiteCommand(query, ctx))
                {
                    command.ExecuteNonQuery();

                }
            }
        }
    }
}
