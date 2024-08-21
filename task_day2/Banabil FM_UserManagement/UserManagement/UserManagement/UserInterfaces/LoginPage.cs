using Services.Dtos;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.UserInterfaces
{
    public static class LoginPage
    {
        public static void Start()
        {
            var loginService = LoginService.Instance;

            Console.WriteLine("Enter username");
            var username = Console.ReadLine() ?? "";

            Console.WriteLine("Enter password");
            var password = Console.ReadLine() ?? "";

            var loginResponse = loginService.Login(new LoginRequest()
            {
                Username = username,
                Password = password
            });

            var loginInfo = loginService.GetLoginInfo();

            Console.WriteLine("==================================================");
            if (!loginInfo.IsLoggedIn)
                Console.WriteLine(loginResponse.Error);
            else
                Console.WriteLine($"Welcome, {loginInfo.Username}. You're logged in as {loginInfo.UserType}");
            Console.WriteLine("==================================================");
        }
    }
}
