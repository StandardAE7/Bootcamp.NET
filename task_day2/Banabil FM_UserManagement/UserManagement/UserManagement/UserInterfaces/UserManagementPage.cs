using Services.Dtos;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace UserManagement.UserInterfaces
{

    public static class UserManagementPage
    {
        private static LoginService loginService = LoginService.Instance;
        private static LoginInfo loginInfo = loginService.GetLoginInfo();
        public static void Start()
        {
            if(loginInfo.UserType == UserType.ADMIN)
            {
                AdminPage.Start();
            }

            if (loginInfo.UserType == UserType.MEMBER)
            {
                MemberPage.Start();
            }
        }
    }
}
