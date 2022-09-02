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
using PICPresence.Util;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PICPresence.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RoomsPage : Page
    {
        private SerialPortFlow com;
        private Attach2 attach2;
        private RoomFlow RoomFlow;
        private List<Room> RoomList;
        private Room CurrentRoom;
        private String alert = "  AFORO EXCEDIDO ";

        public RoomsPage()
        {
            this.InitializeComponent();
            
            this.RoomFlow = new RoomFlow("https://picpresence.ncastillo.xyz/api/collections/rooms/records");

            InitialState();

            this.com = (Application.Current as App).com;
            this.attach2 = (Application.Current as App).attach2;

            if (com != null && com.CurrentState == SerialPortFlow.State.OPEN.ToString())
            {
                NotConnectedInfoBar.IsOpen = false;
                com._suscribe = PicDataReceivedHandler;
                if (attach2.thr == null)
                {
                    Debug.WriteLine("ATTACH THREAD CREATED");
                    attach2.Run(this.R1, this.R2, this.ReactiveFetch);
                }
            }

            this.Grilla.SelectionChanged += Grilla_SelectionChanged;
        }

        private async void ReactiveFetch()
        {
            await fetchRooms();
            DispatcherQueue.TryEnqueue(() =>
            {

                SetData();
            });
        }

        private async void InitialState()
        {
            DataNotLoaded.IsActive = await fetchRooms();

            SetData();

            CurrentRoom = RoomList[0];
        }

        private string R1()
        {
            if (CurrentRoom != null)
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

            if (CurrentRoom != null)
            {
                var RoomFetch = RoomList.First(r => r.Id == CurrentRoom.Id);

                CurrentRoom = RoomFetch;

                // var index = RoomList.IndexOf(CurrentRoom);

                //this.Grilla.SelectedIndex = index;

            }

            this.roomData.Source = RoomList;

        }

        private void Grilla_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentRoom != null)
            {
                this.Grilla.SelectedIndex = RoomList.IndexOf(CurrentRoom);
            }
        }

        private async void NewRoom(object sender, RoutedEventArgs e)
        {

            if (TxbName.Text != "" && TxbCapacity.Text != "")
            {
                var NewRoom = new Room
                {
                    Name = TxbName.Text,
                    MaxCapacity = int.Parse(TxbCapacity.Text),
                    CurrentCapacity = 0
                };

                var successful = await RoomFlow.Add(NewRoom);

                if (successful)
                {
                    await fetchRooms();
                    SetData();
                }
            }
        }

        private void RoomSelect(object sender, ItemClickEventArgs e)
        {
            Room roomSelected = e.ClickedItem as Room;

            CurrentRoom = roomSelected;
        }

        private async void PicDataReceivedHandler(string data)
        {
            DispatcherQueue.TryEnqueue(async () =>
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
                else
                {
                    var contentDialog = new ContentDialog
                    {
                        Title = "Capacidad Excedida",
                        Content = "El ambiente ha alcanzado o excedido su aforo máximo",
                        CloseButtonText = "Aceptar",
                        XamlRoot = this.Content.XamlRoot
                    };
                    await contentDialog.ShowAsync();
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

            return currentCapacity <= room.MaxCapacity || currentCapacity >= 0;
        }
    }
}
