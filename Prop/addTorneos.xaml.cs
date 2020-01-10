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
    /// Interaction logic for addTorneos.xaml
    /// </summary>
    public partial class addTorneos : Window
    {
        public int iIdTorneo;
        public String sNombreTorneoAnterior;

        public addTorneos()
        {
            InitializeComponent();

        }

        private void btnGuardarTorneo_Click(object sender, RoutedEventArgs e)
        {
            bool bRespuesta = false;
            if(txtNombreTorneo.Text != "")
            {
                if (iIdTorneo > 0)
                    bRespuesta = ModificarTorneo();
                else
                    bRespuesta = AgregarTorneo();

                if(bRespuesta)
                { 
                    MainTorneo torneo = new MainTorneo();
                    torneo.Show();
                    this.Close();
                    
                }
            }
            else
            {
                MessageBox.Show("Debes de agregar un nombre","Error");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (iIdTorneo > 0)
                txtNombreTorneo.Text = sNombreTorneoAnterior;
        }

        public static SQLiteConnection GetInstance()
        {
            var db = new SQLiteConnection(
                string.Format("Data Source={0};Version=3;", "Prop.db")
            );

            db.Open();

            return db;
        }

        public bool AgregarTorneo()
        {
            bool bRegresa = false;

            using (var ctx = GetInstance())
            {
                string query = string.Format("INSERT INTO torneos(nombre) VALUES('{0}')", txtNombreTorneo.Text);

                using (var command = new SQLiteCommand(query, ctx))
                {

                    if (command.ExecuteNonQuery() == 1)
                    {
                        string txtMensaje = string.Format("Se creo correctamente el torneo '{0}'", txtNombreTorneo.Text);
                        MessageBox.Show(txtMensaje);
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


        public bool ModificarTorneo()
        {
            bool bRegresa = false;

            using (var ctx = GetInstance())
            {
                string query = string.Format("UPDATE torneos SET nombre = '{0}' WHERE id = {1}", txtNombreTorneo.Text, iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    if (command.ExecuteNonQuery() == 1)
                    {
                        string txtMensaje = string.Format("Se modifico correctamente el torneo '{0}'", txtNombreTorneo.Text);
                        MessageBox.Show(txtMensaje);
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
    }
}
