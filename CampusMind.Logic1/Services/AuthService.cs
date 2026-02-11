using CampusMind.Logic1.Core;
using CampusMind.Logic1.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusMind.Logic1.Services
{
    // this class will handle the Login/Register 
    // and any auth for objects. following the single-responsibilty principle
    public static class AuthService
    {

        public static User Register(string email, string passwordPlain, string name)
        {
            string passwordHash = PasswordHasher.Hash(passwordPlain);

            int newID = Data1.DataAccess.UserDataAccess.AddNewUser(email, passwordHash, name);
            if (newID != -1)
            {
                return new User(newID, email, passwordHash, name);
            }

            return null;
        }

        public static User Login(string email,string passwordPlain)
        {
            string storedHash = "", name = "";
            int id = -1;

            bool isFound = Data1.DataAccess.UserDataAccess.GetUserByEmail(email, ref id, ref storedHash, ref name);
            if (!isFound)
            {
                return null;
            }

            if (!PasswordHasher.Verify(passwordPlain,storedHash)) { return null; }

            return new User(id, email, storedHash, name);
        }
    }
}
