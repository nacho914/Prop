using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.IO;

namespace Prop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public int iIdTorneo;       
        public List<TablaEquipos> itemsTabla;
        public List<goleadores> itemsGoleo;
        public string sLogo;
        public string sLogo2;

        public MainWindow()
        {
            itemsTabla = new List<TablaEquipos>();
            itemsGoleo = new List<goleadores>();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
            //MessageBox.Show(macAddr);
           if(macAddr.Equals("E4D53DBF3B20"))
            {
                //var rpt = new IListPdfReport().CreatePdfReport();
                cargarDatosTorneo();
                cargarTabla();
                cargarGoleadores();
                
            }
            else
            {

                MessageBox.Show("Este no es tu sistema, sal de aqui perro de la calle");
                mModifica.IsEnabled = false;
                mJuegos.IsEnabled = false;
                mJornadas.IsEnabled = false;
            }
            // MessageBox.Show(macAddr);
        }


        private void cargarDatosTorneo()
        {
            string sNombreTorneo="";
            using (var ctx = GetInstance())
            {
                var query = string.Format("SELECT nombre,logotorneo,logotorneo2 FROM torneos WHERE id ={0}", iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            sNombreTorneo = reader["nombre"].ToString();
                            sLogo = reader["logotorneo"].ToString();
                            sLogo2 = reader["logotorneo2"].ToString();

                        }
                    }
                }
            }

            lblNombreTorneo.Content = string.Format("Bienvenido al torneo {0}", sNombreTorneo);
            cargarImagen(sLogo,1);
            cargarImagen(sLogo2,2);

        }
        public void cargarImagen(string sImgBase64,int iTipo)
        {            
            string urlImg = "";

            try
            { 
                byte[] binaryData = Convert.FromBase64String(sImgBase64);

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = new MemoryStream(binaryData);
                bi.EndInit();
                

                if (iTipo==1)
                {                                         
                    urlImg = System.IO.Path.GetTempPath() + "imglogo" + iIdTorneo + ".jpg";
                    imgLogo.Source = bi;
                }
                else
                {
                    urlImg = System.IO.Path.GetTempPath() + "imglogo2" + iIdTorneo + ".jpg";
                    imgLogo2.Source = bi;
                }
                                                
                using (var fileStream = new FileStream(urlImg, FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bi));
                    encoder.Save(fileStream);
                }
            }
            catch
            { }
        }


        public void cargarTabla()
        {

            int iLugar = 1;

            using (var ctx = GetInstance())
            {
                var query = string.Format("select E.nombre AS nombre,count(*) as juegos, (SELECT count(*)  FROM jornadas where idganador = E.numeroequipo AND idtorneo = E.idtorneo) as JG, (SELECT count(*) FROM jornadas where(idlocal = E.numeroequipo OR idvisitante = E.numeroequipo) AND idganador = 0 AND guardado = 1 AND idtorneo = E.idtorneo) as JE, (SELECT count(*) FROM jornadas  where(idlocal = E.numeroequipo OR idvisitante = E.numeroequipo)  AND idganador NOT in (0, E.numeroequipo) AND guardado = 1 AND idtorneo = E.idtorneo) as JP,  coalesce((SELECT SUM(goles) FROM DetallesGoles WHERE idequipo = e.numeroequipo AND idtorneo = E.idtorneo),0) AS GF,  coalesce((SELECT SUM(goles) FROM DetallesGoles WHERE idEquipoContra = e.numeroequipo AND idtorneo = E.idtorneo),0)  AS GC,coalesce(coalesce((SELECT SUM(goles) FROM DetallesGoles WHERE  idequipo = e.numeroequipo AND idtorneo = E.idtorneo),0) - coalesce((SELECT SUM(goles) FROM DetallesGoles  WHERE idEquipoContra = e.numeroequipo AND idtorneo = E.idtorneo),0),0) AS Dif,    ((SELECT count(*) FROM jornadas where idganador = E.numeroequipo AND idtorneo = E.idtorneo) *3) +     ((SELECT count(*) FROM jornadas where(idlocal = E.numeroequipo OR idvisitante = E.numeroequipo)  AND idganador = 0 AND guardado = 1 AND idtorneo = E.idtorneo)) AS Puntos from Equipos as E INNER JOIN jornadas as J on ((E.numeroequipo = J.idlocal OR E.numeroequipo = J.idvisitante) AND E.idtorneo= J.idtorneo ) WHERE J.idtorneo = {0} AND J.guardado = 1 GROUP BY E.nombre,E.numeroequipo ORDER BY puntos DESC,Dif DESC ,GF DESC,GC ASC", iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //iIdEquipo = Int32.Parse(reader["id"].ToString());
                            TablaEquipos tabla = new TablaEquipos();
                            tabla.iLugar = iLugar;
                            tabla.sEquipo = reader["nombre"].ToString();
                            tabla.iJJ = Int32.Parse(reader["juegos"].ToString());
                            tabla.iJG = Int32.Parse(reader["JG"].ToString());
                            tabla.iJE = Int32.Parse(reader["JE"].ToString());
                            tabla.iJP = Int32.Parse(reader["JP"].ToString());
                            tabla.iJP = Int32.Parse(reader["JP"].ToString());
                            tabla.iGF = Int32.Parse(reader["GF"].ToString());
                            tabla.iGC = Int32.Parse(reader["GC"].ToString());
                            tabla.iDif = Int32.Parse(reader["Dif"].ToString());
                            tabla.iPuntos = Int32.Parse(reader["Puntos"].ToString());

                            iLugar++;
                            itemsTabla.Add(tabla);
                        }
                    }
                }
            }
            //asigrnarLugar();
            lvTabla.ItemsSource = null;
            lvTabla.ItemsSource = itemsTabla;

        }

        private void cargarGoleadores()
        {
            using (var ctx = GetInstance())
            {
                var query = string.Format("SELECT a.idjugador as idjugador, sum(a.goles) AS goles, c.nombre || \" \" || c.paterno || \" \" || c.materno as nombre, b.nombre as nombreequipo from DetallesGoles AS a INNER join Equipos AS B ON a.idequipo = b.numeroequipo AND a.idtorneo = b.idtorneo INNER join jugadores as c ON a.idjugador = c.id WHERE a.idtorneo = {0} group by idjugador ORDER BY goles DESC ", iIdTorneo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {                            
                            goleadores goleo = new goleadores();

                            goleo.idJugador = Int32.Parse(reader["idjugador"].ToString());
                            goleo.iGoles = Int32.Parse(reader["goles"].ToString());
                            goleo.sJugador = reader["nombre"].ToString();
                            goleo.sEquipo = reader["nombreequipo"].ToString();

                            itemsGoleo.Add(goleo);
                        }
                    }
                }
            }

            lvGoleadores.ItemsSource = null;
            lvGoleadores.ItemsSource = itemsGoleo;
        }

        //Sustituir por funcion en query no sea tonto
        public void asigrnarLugar()
        {
            int iLugar = 1;
            itemsTabla = itemsTabla.OrderByDescending(TablaEquipos => TablaEquipos.iPuntos).ThenBy(TablaEquipos => TablaEquipos.iDif).ThenBy(TablaEquipos => TablaEquipos.iGF).ToList();

            foreach (TablaEquipos tabla in itemsTabla)
            {
                tabla.iLugar = iLugar;
                iLugar++;
            }
        }

        private void mModifica_Click(object sender, RoutedEventArgs e)
        {
            MainEquipos equipos = new MainEquipos();
            equipos.iIdTorneo = this.iIdTorneo;            
            equipos.Show();
            this.Close();
        }



        private void mJuegos_Click(object sender, RoutedEventArgs e)
        {

            MainJuegos juegos = new MainJuegos();
            juegos.iIdTorneo = this.iIdTorneo;
            juegos.Show();
            this.Close();
        }

        private void mJornadas_Click(object sender, RoutedEventArgs e)
        {
            MainJornadas jornadas = new MainJornadas();
            jornadas.iIdTorneo = this.iIdTorneo;
            jornadas.Show();
            this.Close();
        }


        private void mRegresar_Click(object sender, RoutedEventArgs e)
        {
            MainTorneo tor = new MainTorneo();
            tor.Show();            
            this.Close();
        }

        private void mReportes_Click(object sender, RoutedEventArgs e)
        {
            MainReportes repo = new MainReportes();

            repo.itemsTabla = this.itemsTabla;
            repo.itemsGoleo = this.itemsGoleo;
            repo.iIdTorneo = this.iIdTorneo;
            repo.Show();

            this.Close();
        }
    }

    public class TablaEquipos
    {
        public int iLugar { get; set; }
        public string sEquipo { get; set; }
        public int iJJ { get; set; }
        public int iJG { get; set; }
        public int iJE { get; set; }
        public int iJP { get; set; }
        public int iGF { get; set; }
        public int iGC { get; set; }
        public int iDif { get; set; }
        public int iPuntos { get; set; }

    }

    public class goleadores
    {
        public int idJugador { get; set; }
        public int iGoles { get; set; }
        public string sJugador { get; set; }
        public string sEquipo { get; set; }

    }


}
