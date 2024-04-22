using EduhubAPI.Dtos;
using EduhubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EduhubAPI.Repositories
{
    public class UserRepository
    {
        private readonly EDUHUBContext _context;
        public UserRepository(EDUHUBContext context)
        {
            _context = context;
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _context.Set<User>().ToList();
        }
        public User Create(User user)
        {
            _context.Users.Add(user);
            user.UserId = _context.SaveChanges();
            return user;
        }
        public User? GetByUserName(string name)
        {
            return _context.Users.FirstOrDefault(u => u.Username == name);
        }
        public User? GetUserByID(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }
        public UserInfo? GetUserInfoByID(int id)
        {
            return _context.UserInfos.FirstOrDefault(u => u.UserId == id);
        }

        public string GetUserRole(int userId)
        {
            var user = _context.Users.Include(u => u.UserType).FirstOrDefault(u => u.UserId == userId);
            return user?.UserType?.UserTypeName ?? string.Empty;
        }
        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }

        public UserInfo UpdateUserInfo(UserInfo userInfo){
            _context.UserInfos.Update(userInfo);
            _context.SaveChanges();
            return userInfo;
        }

        public void DeleteUser(int userId)
        {
            var user = _context.Set<User>().Find(userId);
            if (user != null)
            {
                _context.Set<User>().Remove(user);
                _context.SaveChanges();
            }
        }
        public object GetUserDetails(int userId)
        {
            var userDetails = _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new 
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    UserType = u.UserType.UserTypeName, 
                    UserInfo = u.UserInfos.Select(ui => new
                    {
                        FullName = ui.FullName,
                        Email = ui.Email,
                        DateOfBirth = ui.DateOfBirth,
                        Gender = ui.Gender,
                        PhoneNumber = ui.PhoneNumber,
                        Avatar = ui.Avatar,
                        UserAddress = ui.UserAddress,
                        UserDescription = ui.UserDescription
                    }).FirstOrDefault() 
                }).FirstOrDefault();

            return userDetails;
        }
        public string GetUserName(int userId)
        {
            var user = _context.Users.Include(u => u.UserInfos).FirstOrDefault(u => u.UserId == userId);
            return user?.UserInfos?.Select(ui => ui.FullName).FirstOrDefault() ?? string.Empty;
        }
    }
}
