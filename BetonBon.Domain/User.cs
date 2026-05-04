using System;
using System.Collections.Generic;
using System.Text;

namespace BetonBon.Domain
{
    public class User
    {
        public Guid Id { get; init; }
        public string Username { get; private set; }


        // Parameterless constructor for EF purposes
        public User() { }

        private User(string username)
        {
            Id = Guid.NewGuid();
            Username = username;
        }

        internal static User CreateUser(string username)
        {
            return new User(username);
        }
    }
}
