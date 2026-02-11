using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampusMind.Logic1.Security;

namespace CampusMind.Logic1.Core
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; private set; }
        public string Name { get; set; }

        public User()
        {

        }

        public User(int id, string email, string passwordHash, string name)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            Name = name;
        }
    }
}
