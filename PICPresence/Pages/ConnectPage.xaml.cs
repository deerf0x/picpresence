using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PICPresence.Util;
using picpresencelib.Core;
using picpresencelib.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PICPresence.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConnectPage : Page
    {
        public SerialPortFlow com;
        public Attach2 attach2;

        private readonly int BaudRate = 2400;
        private readonly int DataBits = 8;
        private readonly int ReceivedBytesThreshold = 1;


        public ConnectPage()
        {
            this.InitializeComponent();
            fetchConnectedPorts();
            connectionStatus();
            this.com = (Application.Current as App).com;
            this.attach2 = (Application.Current as App).attach2;
        }


        private void fetchConnectedPorts()
        {
            portCBox.Items.Clear();

            string[] ports = SerialPort.GetPortNames();

            foreach (string port in ports)
            {
                portCBox.Items.Add(port);
            }
        }

        private void disconnectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (com != null && com.CurrentState == SerialPortFlow.State.OPEN.ToString())
            {
                com.Close();
                connectionStatus();
            }
        }


        private void connectionStatus()
        {
            if (com != null && com.CurrentState == SerialPortFlow.State.OPEN.ToString())
            {
                statusLbl.Text = "Conectado al PIC en " + com._serialPort.PortName;
            }
            else
            {
                statusLbl.Text = "PIC Desconectado";
            }
        }

        private async void connectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (com == null || com._serialPort == null || !com._serialPort.IsOpen)
            {
                if (portCBox.SelectedIndex == -1)
                {
                    var contentDialog = new ContentDialog
                    {
                        Title = "Puerto no seleccionado",
                        Content = "Por favor seleccione un puerto COM",
                        CloseButtonText = "Aceptar",
                        XamlRoot = this.Content.XamlRoot
                    };
                    await contentDialog.ShowAsync();

                }
                else
                {
                    try
                    {
                        this.com = new SerialPortFlow(
                           (string) portCBox.SelectedValue,
                           BaudRate,
                           DataBits,
                           ReceivedBytesThreshold,
                           SampleInitCallback
                        );

                        (Application.Current as App).com = this.com;

                        com.Open();

                        (Application.Current as App).attach2 = new Attach2(com);

                        connectionStatus();

                        Thread.Sleep(30);
                    }
                    catch (Exception ex)
                    {
                        statusLbl.Text = ex.Message;
                    }
                }
            }
        }

        private async void SampleInitCallback(string data)
        {
            // Just to get this thing initialized...
        }

        private void portRefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            fetchConnectedPorts();
        }

    }
}
