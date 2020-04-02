using System;

namespace DotNet2020.Domain._5.Entities
{
    public class User
    {
        public string Login { get; private set; }

        public User(string login)
        {
            if (String.IsNullOrEmpty(login))
                throw new ArgumentException("Must be not empty!", "Login");
            Login = login;
        }
    }
}
