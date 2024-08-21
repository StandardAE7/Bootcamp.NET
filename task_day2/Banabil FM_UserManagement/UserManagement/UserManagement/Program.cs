using Services.Dtos;
using Services.Services;
using UserManagement.UserInterfaces;

namespace UserManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                LoginPage.Start();
                UserManagementPage.Start();
            }
        }
    }
}
