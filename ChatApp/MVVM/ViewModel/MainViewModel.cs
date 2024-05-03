using ChatClient.MVVM.Core;
using ChatClient.MVVM.Model;
using ChatClient.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Protocol.Processing;
using Protocol.Processing.Packet;
using Protocol.Utils;

namespace ChatClient.MVVM.ViewModel
{
    internal class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }

        public RelayCommand ConnectToServerCommand { get; }
        public String Username { get; set; } = string.Empty;
        
        private readonly Server _server;

        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            _server = new Server();
            _server.connectedEvent += UserConnected;
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));
        }

        private void UserConnected()
        {
            var broadcastedUsers = _server.PacketReader.Deserialize<BroadcastUsersPacket>();
            Console.WriteLine($"[{DateTime.Now}]: When receiving a broadcast, user: {Username} received {broadcastedUsers.Users.Count} users.");
            foreach (var user in broadcastedUsers.Users)
            {
                var newUser = new UserModel
                {
                    Username = user.username,
                    Guid = user.guid
                };

                if (!Users.Any(user => user.Username == newUser.Username && user.Guid == newUser.Guid))
                {
                    Application.Current.Dispatcher.Invoke(() => Users.Add(newUser));
                }
            }
        }
    }
}
