using HexMaster.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HexMaster.Views
{
    /// <summary>
    /// Interaction logic for ServiceView.xaml
    /// </summary>
    public partial class ServiceView : UserControl
    {
        public ServiceView()
        {
            InitializeComponent();
        }

        private ServiceBase _service = null;



        public ServiceBase Service
        {
            get { return _service; }
            set
            {
                _service = value;
                lblServiceName.Text = _service.ServiceName;
                btnPause.IsEnabled = false;
                btnStop.IsEnabled = false;
            }
        }

        private bool InvokeServiceMethod(ServiceCommands command)
        {
            var success = false;
            Type serviceBaseType = _service.GetType();
            object[] parameters = null;
            if (command == ServiceCommands.Start)
            {
                parameters = new object[] { null };
            }

            string method = "OnStart";
            switch (command)
            {
                case ServiceCommands.Stop:
                    method = "OnStop";
                    break;
                case ServiceCommands.Pause:
                    method = "OnPause";
                    break;
                case ServiceCommands.Start:
                default:
                    method = "OnStart";
                    break;
            }

            try
            {
                serviceBaseType.InvokeMember(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, _service, parameters);
                success = true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("An exception was thrown while trying to call the {0} of the {1} service.  Examine the inner exception for more information.", method, _service.ServiceName), ex.InnerException);
            }
            return success;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (InvokeServiceMethod(ServiceCommands.Start))
            {
                btnStop.IsEnabled = _service.CanStop;
                btnPause.IsEnabled = _service.CanStop;
                btnPlay.IsEnabled = false;
            }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (InvokeServiceMethod(ServiceCommands.Pause))
            {
                btnStop.IsEnabled = _service.CanStop;
                btnPause.IsEnabled = false;
                btnPlay.IsEnabled = true;
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (InvokeServiceMethod(ServiceCommands.Stop))
            {
                btnStop.IsEnabled = false;
                btnPause.IsEnabled = false;
                btnPlay.IsEnabled = true;
            }
        }

    }
}
