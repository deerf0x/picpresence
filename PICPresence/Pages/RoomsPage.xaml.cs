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
using System.Collections.ObjectModel;
using System.Xml.Linq;
using picpresencelib.Core;
using picpresencelib.Utils;
using System.Threading;
using System.Text.RegularExpressions;
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PICPresence.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RoomsPage : Page
    {

        private RoomFlow RoomFlow;
        private List<Room> RoomList;
        private Room CurrentRoom;
        private String alert = "  AFORO EXCEDIDO ";
        private readonly int BaudRate = 2400;
        private readonly int DataBits = 8;
        private readonly int ReceivedBytesThreshold = 1;
        private readonly string Port = "COM1";
        private SerialPortFlow com;

        public RoomsPage()
        {
            this.InitializeComponent();
            
            this.RoomFlow = new RoomFlow("https://picpresence.ncastillo.xyz/api/collections/rooms/records");

            InitialState();

            this.com = new SerialPortFlow(
                   Port,
                   BaudRate,
                   DataBits,
                   ReceivedBytesThreshold,
                   ListenToPIC
                );

            com.Open();

            var attach = new Attach(com);

            attach.Run(this.R1, this.R2, 1000);  

        }

        private async void InitialState()
        {
            DataNotLoaded.IsActive = await fetchRooms();

            SetData();

            CurrentRoom = RoomList[0];
        }

        private string R1()
        {
            if(CurrentRoom != null)
            {
                return "AULA: " + CurrentRoom.Name;
            }

            return new string(' ', 16);
        }

        private string R2()
        {
            if (CurrentRoom != null)
            {
                var time = DateTime.Now.ToString("hh:mm:ss");

                return " CT: " + CurrentRoom.MaxCapacity + " CA:" + CurrentRoom.CurrentCapacity +
                            "   " + time;
            }
            return new string(' ', 16);
        }
        
        
        private async Task<Boolean> fetchRooms()
        {
            this.RoomList = await RoomFlow.GetRoomsAsync();
           

            return  !(RoomList.Count > 0);
        }

        private void SetData()
        {
            
            this.roomData.Source = RoomList;
        }

        private async void NewRom(object sender, RoutedEventArgs e)
        {
            var NewRom = new Room
            {
                Name = TxbName.Text,
                MaxCapacity = int.Parse(TxbCapacity.Text),
                CurrentCapacity = 0
            };

            var successful = await RoomFlow.Add(NewRom);

            if (successful)
            {
                await fetchRooms();
                SetData();
            }
        }

        private void RoomSelect(object sender, ItemClickEventArgs e)
        {
            Room roomSelected = e.ClickedItem as Room;

            CurrentRoom = roomSelected;

        }

        private async void ListenToPIC(string data)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                
                if (checkCapacity(CurrentRoom))
                {
                    if (data == "I")
                    {
                        CurrentRoom.CurrentCapacity++;
                    }
                    else
                    {
                        CurrentRoom.CurrentCapacity--;   
                    }
                }  
            });
            var successful = await RoomFlow.Put(CurrentRoom);

            if (successful)
            {
                await fetchRooms();
            }
        }
        
        private bool checkCapacity(Room room)
        {
            var currentCapacity = room.CurrentCapacity;

            return currentCapacity <= room.MaxCapacity || currentCapacity > 0;
        }
    }
}
