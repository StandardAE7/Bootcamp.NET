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
    public static class MemberPage
    {
        private static LoginService loginService = LoginService.Instance;
        private static MemberService memberService = MemberService.Instance;
        private static LoginInfo loginInfo = loginService.GetLoginInfo();
        private static List<string> options = new List<string>()
        {
            "Change Password",
            "Delete Account",
            "Logout"
        };
        public static void Start()
        {
            while (loginInfo.IsLoggedIn)
            {
                var choice = Helpers.GetChoice(options);

                if (choice == 1)
                {
                    // Verify current password
                    Console.Write("Enter your current password: ");
                    var currentPassword = Console.ReadLine();

                    var user = memberService.GetMemberByUsername(loginInfo.Username); // Get current member details
                    if (user == null || user.Password != currentPassword)
                    {
                        Console.WriteLine("Current password is incorrect.");
                        continue;
                    }

                    // Enter and verify new password
                    Console.Write("Enter your new password: ");
                    var newPassword = Console.ReadLine();

                    Console.Write("Retype your new password: ");
                    var retypePassword = Console.ReadLine();

                    if (newPassword != retypePassword)
                    {
                        Console.WriteLine("Passwords do not match.");
                        continue;
                    }

                    // Update password
                    memberService.ChangePassword(loginInfo.Username, newPassword);
                    Console.WriteLine("Password changed successfully.");
                }

                if (choice == 2)
                {
                    Console.Write("Are you sure you want to delete your account? This action cannot be undone. (yes/no): ");
                    var confirmation = Console.ReadLine();

                    if (confirmation?.ToLower() == "yes")
                    {
                        try
                        {
                            memberService.DeleteMember(loginInfo.Username);
                            loginService.Logout(); // Log out after account deletion
                            Console.WriteLine("Your account has been deleted and you have been logged out.");
                            continue; // Exit loop since user is logged out
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Account deletion canceled.");
                    }
                }

                if (choice == options.Count)
                {
                    loginService.Logout();
                    Console.WriteLine("You've logged out.");
                    continue;
                }
            }
        }
    }
}
