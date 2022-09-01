using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PICPresence.Models;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PICPresence.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PICPresence.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RoomsPage : Page
    {
        public bool DataNotLoaded = true;
        private RoomFlow RoomFlow;
        private List<Room> RoomList;
        public RoomsPage()
        {
            this.InitializeComponent();
            this.RoomFlow = new RoomFlow("https://picpresence.ncastillo.xyz/api/collections/rooms/records");
            FetchRooms();
        }

        public async void FetchRooms()
        {
            this.RoomList = await RoomFlow.GetRoomsAsync();
            DataNotLoaded = RoomList.Count > 0;
        }
    }
}
