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
                    UserType = u.UserType.UserTypeName, // Giả sử bạn muốn hiển thị tên loại người dùng
                    UserInfo = u.UserInfos.Select(ui => new
                    {
                        FullName = ui.FullName,
                        Email = ui.Email,
                        DateOfBirth = ui.DateOfBirth,
                        Gender = ui.Gender,
                        PhoneNumber = ui.PhoneNumber
                    }).FirstOrDefault() // Sử dụng FirstOrDefault vì mỗi User có thể chỉ có một UserInfo
                }).FirstOrDefault();

            return userDetails;
        }
        public string GetUserName(int userId)
        {
            var user = _context.Users.Include(u => u.UserInfos).FirstOrDefault(u => u.UserId == userId);
            return user?.UserInfos?.Select(ui => ui.FullName).FirstOrDefault() ?? string.Empty;
        }
        public void UpdateUserFullName(int userId, string fullName)
        {
            var user = GetUserByID(userId);
            if (user != null && user.UserInfos.Any()) // Check if user and UserInfos are not null and UserInfos has at least one element
            {
                // Assuming User has a UserInfo navigation property that includes a collection of UserInfo
                var userInfo = user.UserInfos.First(); // Access the first UserInfo object
                userInfo.FullName = fullName; // Update the FullName of the first UserInfo object
                UpdateUser(user); // Save changes
            }
        }
        public void UpdateUserPhoneNumber(int userId, string phoneNumber)
        {
            var user = GetUserByID(userId);
            if (user != null && user.UserInfos.Any())
            {
                var userInfo = user.UserInfos.First();
                userInfo.PhoneNumber = phoneNumber;
                UpdateUser(user);
            }
        }

        public void UpdateUserDateOfBirth(int userId, DateTime dateOfBirth)
        {
            var user = GetUserByID(userId);
            if (user != null && user.UserInfos.Any())
            {
                var userInfo = user.UserInfos.First();
                userInfo.DateOfBirth = dateOfBirth;
                UpdateUser(user);
            }
        }

        public void UpdateUserGender(int userId, string gender)
        {
            var user = GetUserByID(userId);
            if (user != null && user.UserInfos.Any())
            {
                var userInfo = user.UserInfos.First();
                userInfo.Gender = gender;
                UpdateUser(user);
            }
        }

        public void UpdateUserEmail(int userId, string email)
        {
            var user = GetUserByID(userId);
            if (user != null && user.UserInfos.Any())
            {
                var userInfo = user.UserInfos.First();
                userInfo.Email = email;
                UpdateUser(user);
            }
        }
        public void UpdateUserInfo(int userId, UpdateUserInfoDto dto)
        {
            var user = GetUserByID(userId);
            if (user != null && user.UserInfos.Any())
            {
                var userInfo = user.UserInfos.First();
                userInfo.FullName = dto.FullName;
                userInfo.PhoneNumber = dto.PhoneNumber;
                userInfo.DateOfBirth = dto.DateOfBirth;
                userInfo.Gender = dto.Gender;
                userInfo.Email = dto.Email;
                UpdateUser(user);
            }
        }
        public void UpdateUserAvatar(int userId, string avatar)
        {
            var user = _context.Users.Include(u => u.UserInfos).FirstOrDefault(u => u.UserId == userId);
            if (user != null && user.UserInfos.Any())
            {
                var userInfo = user.UserInfos.First();
                userInfo.Avatar = avatar;
                _context.SaveChanges();
            }
        }
    }
}
