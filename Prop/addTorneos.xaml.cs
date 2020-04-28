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
using Microsoft.Win32;
using System.IO;
using System.Drawing;


namespace Prop
{
    /// <summary>
    /// Interaction logic for addTorneos.xaml
    /// </summary>
    public partial class addTorneos : Window
    {
        public int iIdTorneo;
        public String sNombreTorneoAnterior;
        public System.Drawing.Image img;
        public string imgloc, imgloc2,sBase1,sBase2;


        public addTorneos()
        {
            InitializeComponent();

        }

        private void btnGuardarTorneo_Click(object sender, RoutedEventArgs e)
        {
            bool bRespuesta = false;
            if (txtNombreTorneo.Text != "")
            {
                if (iIdTorneo > 0)
                    bRespuesta = ModificarTorneo();
                else
                    bRespuesta = AgregarTorneo();

                if (bRespuesta)
                {
                    MainTorneo torneo = new MainTorneo();
                    torneo.Show();
                    this.Close();

                }
            }
            else
            {
                MessageBox.Show("Debes de agregar un nombre", "Error");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (iIdTorneo > 0)
            {
                txtNombreTorneo.Text = sNombreTorneoAnterior;
                cargarDatosTorneo();
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

        public bool AgregarTorneo()
        {
            bool bRegresa = false;
            string base64String = "";
            string base64String2 = "";

            using (var ctx = GetInstance())
            {
                base64String = imagen64(1);
                base64String2 = imagen64(2);
                string query = string.Format("INSERT INTO torneos(nombre,logotorneo,logotorneo2) VALUES('{0}','{1}','{2}')", txtNombreTorneo.Text,base64String, base64String2);

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

            string base64String = "";
            string base64String2 = "";

            using (var ctx = GetInstance())
            {
                base64String = imagen64(1);
                base64String2 = imagen64(2);

                string query = string.Format("UPDATE torneos SET nombre = '{0}',logotorneo='{1}',logotorneo2='{2}' WHERE id = {3}", txtNombreTorneo.Text, base64String, base64String2, iIdTorneo);

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

        public void cargarImagen(string sImgBase64,int iTipo)
        {
            try
            { 
                byte[] binaryData = Convert.FromBase64String(sImgBase64);

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = new MemoryStream(binaryData);
                bi.EndInit();

                if (iTipo == 1)
                    imgDrop.Source = bi;
                else
                    imgDrop2.Source = bi;
            }
            catch
            { }
        }

        public string imagen64(int iTipo)
        {                        
            if(iTipo==1)
                return ImageToByte((BitmapImage)imgDrop.Source,1);
            else
                return ImageToByte((BitmapImage)imgDrop2.Source,2);
        }


        public string ImageToByte(BitmapImage imageSource, int iTipo)
        {
            try { 
                Stream stream = imageSource.StreamSource;
                Byte[] buffer = null;
                String sUsable = "";

                if (iTipo == 1)
                    sUsable = imgloc;
                else
                    sUsable = imgloc2;

                if (stream != null && stream.Length > 0)
                {
                    if (iTipo == 1)
                        return sBase1;
                    else
                        return sBase2;                
                }
                else
                {
                    FileStream fs = new FileStream(sUsable, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    buffer = br.ReadBytes((int)fs.Length);

                    return Convert.ToBase64String(buffer);
                }
            }
            catch
            {
                return "";
            }
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            MainTorneo main = new MainTorneo();                       
            main.Show();
            this.Close();
        }

        private void btnImagenTorneo_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Imagenes(.png;.jpeg;.jpg;)|*.png;*.jpeg;*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                imgDrop.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                imgloc = openFileDialog.FileName;
                //MessageBox.Show(openFileDialog.FileName);
            }

        }


        private void cargarDatosTorneo()
        {
            string sLogo = "";
            string sLogo2 = "";
            using (var ctx = GetInstance())
            {
                var query = string.Format("SELECT logotorneo,logotorneo2 FROM torneos WHERE id ={0}", iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sLogo = reader["logotorneo"].ToString();
                            sLogo2 = reader["logotorneo2"].ToString();
                        }
                    }
                }

            }
            sBase1 = sLogo;
            sBase2 = sLogo2;
            cargarImagen(sLogo,1);
            cargarImagen(sLogo2, 2);
        }

        private void btnimagen2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Imagenes(.png;.jpeg;.jpg;)|*.png;*.jpeg;*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                imgDrop2.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                imgloc2 = openFileDialog.FileName;
                //MessageBox.Show(openFileDialog.FileName);
            }
        }
    }
        
}
