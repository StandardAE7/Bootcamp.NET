using DataAccess.Models;
using DataAccess.Repositories;
using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Services.Services
{
    public class LoginService
    {
        public readonly static LoginService Instance = new LoginService();
        private readonly UserRepository userRepository = UserRepository.Instance;
        private readonly LoginInfo loginInfo;

        static LoginService() { }
        public LoginService()
        {
            userRepository.Save(new Admin()
            {
                Username = "admin",
                Password = "admin",
            });

            loginInfo = new LoginInfo();
        }

        public LoginResponse Login(LoginRequest loginRequest)
        {
            var userToLogin = userRepository.GetByUsername(loginRequest.Username);

            if(userToLogin is null || userToLogin.Password != loginRequest.Password) 
            {
                return new LoginResponse()
                {
                    Success = false,
                    Error = "Wrong username or password."
                };
            }

            if(userToLogin is Member && ((Member)userToLogin).IsDisabled)
                return new LoginResponse()
                {
                    Success = false,
                    Error = "Your account has been disabled!"
                };

            loginInfo.Username = loginRequest.Username;
            loginInfo.IsLoggedIn = true;
            loginInfo.UserType = userToLogin is Admin ? UserType.ADMIN : UserType.MEMBER;

            return new LoginResponse()
            {
                Success = true
            };
        }

        public void Logout()
        {
            loginInfo.IsLoggedIn = false;
            loginInfo.Username = string.Empty;
            loginInfo.UserType = null;
        }

        public LoginInfo GetLoginInfo()
        {
            return loginInfo;
        }
    }
}
