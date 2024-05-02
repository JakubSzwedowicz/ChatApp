﻿using ChatClient.MVVM.Core;
using ChatClient.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.MVVM.ViewModel
{
    internal class MainViewModel
    {
        public RelayCommand ConnectToServerCommand { get; }
        
        private readonly Server _server;

        public MainViewModel()
        {
            _server = new Server();
            ConnectToServerCommand = new RelayCommand(o => _server.ConnecToServer());
        }
    }
}
