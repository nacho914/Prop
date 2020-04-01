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
    /// Interaction logic for MainJornadas.xaml
    /// </summary>
    public partial class MainJornadas : Window
    {
        public int iIdTorneo;
        public List <equipos> equipo;
        public List<Partidos> Partido;
        public List<lvJornadas> listJornadas;
        int iCargo = 0;

        public MainJornadas()
        {
            InitializeComponent();
            equipo = new List<equipos>();
            Partido = new List<Partidos>();
            listJornadas = new List<lvJornadas>();            
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Si las jornadas aun no se han generado, se tienen que realizar.

            if(!VerificarJornadasGeneradas())
            {                
                obtenerEquipos();

                if (!VerificarEquiposPares())
                    generarJornadasImpares();
                else
                    generarJornadasPares();

                guardarJornadas();
                marcarJornadasTorneo();
            }
            llenarComboJornadas();
            cargarJornada();
            iCargo = 1;

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
                        if(reader.Read())
                        {
                           if( Int32.Parse(reader["jornadas"].ToString()) != 0)
                           {
                                bRegresar = true;
                           }
                        }
                    }
                }               
            }

            return bRegresar;
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

        public static SQLiteConnection GetInstance()
        {
            var db = new SQLiteConnection(
                string.Format("Data Source={0};Version=3;", "Prop.db")
            );

            db.Open();

            return db;
        }

        private void btn_Jornadas_Click(object sender, RoutedEventArgs e)
        {
            
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
                            equipo.Add(new equipos { nombre = reader["nombre"].ToString(), iEquipo = iContador});
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
            int iRondas = (equipo.Count()*(equipo.Count() - 1)) / 2;
            int iJornadas = equipo.Count() - 1;

            int iJuegosTotales = iRondas * iJornadas;
            int iJor = 1;
            int iJue = 1;

            for (int i = 1; i <= iJuegosTotales; i++)
            {
                Partido.Add((new Partidos { jornada = iJor, local = iJue, visitante = -1 }));
                iJue++;
                if (iJue > (iJornadas+1))
                    iJue = 1;
                if (i % (iRondas-1) == 0)
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


        private void llenarComboJornadas()
        {
            Dictionary<string, string> listJornadas = new Dictionary<string, string>();
            string txt;
            //listTorneos.Add("0", "Agregar nuevo equipo");

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
                            listJornadas.Add(reader["jornada"].ToString(),txt);
                        }
                    }
                }
                cmb_jornadas.ItemsSource = listJornadas;
                cmb_jornadas.SelectedValue = "key";
                cmb_jornadas.DisplayMemberPath = "Value";
                cmb_jornadas.SelectedIndex = 0;
            }
        }

        public void cargarJornada()
        {

            int iJornada = cmb_jornadas.SelectedIndex+1;
            string local="", visitante="";

            using (var ctx = GetInstance())
            {

                var query = string.Format("SELECT idlocal,idvisitante FROM jornadas WHERE idtorneo ={0} AND NumeroJornada = {1}", this.iIdTorneo, iJornada);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            local = obtenerNombreEquipo(Int32.Parse(reader["idlocal"].ToString()));
                            visitante = obtenerNombreEquipo(Int32.Parse(reader["idvisitante"].ToString()));
                            listJornadas.Add(new lvJornadas() { local = local, visitante = visitante, marlocal = "-", marvisitante = "-",vs = "VS" });

                        }   
                    }
                }
                lvJornadasPrincipal.ItemsSource = null;
                lvJornadasPrincipal.ItemsSource = listJornadas;                

            }
              
        }

        public string obtenerNombreEquipo(int idEquipo)
        {
            string nombre = "";

            using (var ctx = GetInstance())
            {
                
                var query = string.Format("SELECT nombre FROM Equipos WHERE idtorneo ={0} AND numeroequipo = {1}", this.iIdTorneo, idEquipo);

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            nombre = reader["nombre"].ToString();
                        }
                    }
                }

            }

            return nombre;
        }

        private void cmb_jornadas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(iCargo == 1)
            { 
                listJornadas.Clear();
                cargarJornada();
            }
        }
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
        public int visitante { get; set;}
    }

    public class lvJornadas
    {
        public string local { get; set; }
        public string marlocal { get; set; }
        public string marvisitante { get; set;}
        public string visitante { get; set; }
        public string vs { get; set; }

    }
}
