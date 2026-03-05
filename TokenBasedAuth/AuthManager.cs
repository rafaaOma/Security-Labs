using System;
using System.Collections.Generic;
using TokenBasedAuth.Models;

namespace TokenBasedAuth
{
    public class AuthManager
    {
        private readonly List<User> users = new List<User>();
        private readonly TokenManager tokenManager = new TokenManager();

        
        public void Register()// register a new user with username & passowrd
        {
            Console.Write("Enter a username: ");
            string username = Console.ReadLine();
            Console.Write("Enter a password: ");
            string password = Console.ReadLine();

            if (users.Exists(u => u.Username == username))
            {
                Console.WriteLine("Username already exists.");
                return;
            }

            users.Add(new User { Username = username, Password = password });
            Console.WriteLine("Registration successful.");
        }

             public void Login() //authetiacte user and generate token
        {
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            var user = users.Find(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                Console.WriteLine("Invalid credentials.");
                return;
            }

            user.Token = tokenManager.GenerateToken(username);//pass username to tokenManager to generate token
            Console.WriteLine($"Login successful. Your token: {user.Token}");
        }

         public User GetUserByToken(string token)//retrive user info based on token
        {
            return users.Find(u => u.Token == token);
        }
    }
}