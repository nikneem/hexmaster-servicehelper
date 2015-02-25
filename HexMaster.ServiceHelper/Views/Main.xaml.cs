using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
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

namespace HexMaster.Views
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }

        public IEnumerable<ServiceBase> Services { get; set; }

        private void ExitButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
        	Close();
        }

        private void frmMain_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            if ((Services != null) && (Services.Count() > 0))
            {
                spServices.Children.Clear();
                foreach (var service in Services)
                {
                    var view = new ServiceView();
                    view.Service = service;
                    spServices.Children.Add(view);
                }
            }
        }
    }
}
