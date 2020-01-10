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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Prop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public int iIdTorneo;
        public string sNombreTorneo;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblNombreTorneo.Content = string.Format("Bienvenido al torneo {0}", sNombreTorneo);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainEquipos equipos = new MainEquipos();
            equipos.iIdTorneo = this.iIdTorneo;
            equipos.sNombreTorneo = this.sNombreTorneo;
            equipos.Show();
            this.Close();
        }
    }
}
