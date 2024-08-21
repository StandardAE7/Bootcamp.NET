using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class MemberService
    {
        public readonly static MemberService Instance = new MemberService();
        private readonly UserRepository userRepository = UserRepository.Instance;

        static MemberService() { }

        public void AddNewMember(string username, string password)
        {
            var newMember = new Member()
            {
                Username = username,
                Password = password
            };

            userRepository.Save(newMember);
        }

        public void DisableMember(string username)
        {
            var userToDisable = userRepository.GetByUsername(username);

            if (userToDisable is null || userToDisable is not Member)
                return;

            var user = (Member)userToDisable;
            user.IsDisabled = true;
            userRepository.Save(user);
        }

        public void ChangePassword(string username, string newPassword)
        {
            var user = userRepository.GetByUsername(username);

            if (user == null || user is not Member)
                return;

            var member = (Member)user;
            member.Password = newPassword;
            userRepository.Save(member);
        }

        public Member? GetMemberByUsername(string username)
        {
            var user = userRepository.GetByUsername(username);
            return user as Member;
        }

        public List<Member> GetAllMembers()
        {
            return userRepository.GetAll().OfType<Member>().ToList();
        }

        public void DeleteMember(string username)
        {
            var user = userRepository.GetByUsername(username);

            if (user == null || user is not Member)
                return;

            userRepository.Delete(user);
        }

    }
}
