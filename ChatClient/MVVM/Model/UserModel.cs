using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.MVVM.Model
{
    internal class UserModel
    {
        public required string Username { get; set; }
        public required string Guid { get; set; }
    }
}
