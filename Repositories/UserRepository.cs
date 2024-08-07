﻿using EduhubAPI.Dtos;
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
            return _context.Users.ToList();
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

        public UserInfo UpdateUserInfo(UserInfo userInfo)
        {
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
        public UserDetailsDto GetUserDetails(int userId)
        {
            var userDetails = _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new UserDetailsDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    UserType = u.UserType.UserTypeName,
                    UserInfo = u.UserInfos.Select(ui => new UserInfoDto
                    {
                        FullName = ui.FullName,
                        Email = ui.Email,
                        DateOfBirth = ui.DateOfBirth,
                        Gender = ui.Gender,
                        PhoneNumber = ui.PhoneNumber,
                        Avatar = ui.Avatar,
                        UserAddress = ui.UserAddress,
                        UserDescription = ui.UserDescription,
                        Expertise = ui.Expertise,
                    }).FirstOrDefault()
                }).FirstOrDefault();

            return userDetails;
        }
        public string GetUserName(int userId)
        {
            var user = _context.Users.Include(u => u.UserInfos).FirstOrDefault(u => u.UserId == userId);
            return user?.UserInfos?.Select(ui => ui.FullName).FirstOrDefault() ?? string.Empty;
        }
        public string GetUserAvatar(int userId)
        {
            var user = _context.Users.Include(u => u.UserInfos).FirstOrDefault(u => u.UserId == userId);
            return user?.UserInfos?.Select(ui => ui.Avatar).FirstOrDefault() ?? string.Empty;
        }
        public (string Name, string Email) GetUserNameAndEmail(int userId)
        {
            var user = _context.UserInfos.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                return (user.FullName ?? string.Empty, user.Email ?? string.Empty);
            }
            return (string.Empty, string.Empty);
        }

        public IEnumerable<UserDetailsDto> GetAllTeachers()
        {
            return _context.Users
                .Where(u => u.UserTypeId == 2)
                .Join(_context.UserInfos,
                      user => user.UserId,
                      userInfo => userInfo.UserId,
                      (user, userInfo) => new UserDetailsDto
                      {
                          UserId = user.UserId,
                          Username = user.Username,
                          UserType = user.UserType.UserTypeName,
                          UserInfo = new UserInfoDto
                          {
                              FullName = userInfo.FullName,
                              Email = userInfo.Email,
                              DateOfBirth = userInfo.DateOfBirth,
                              Gender = userInfo.Gender,
                              PhoneNumber = userInfo.PhoneNumber,
                              Avatar = userInfo.Avatar,
                              UserAddress = userInfo.UserAddress,
                              UserDescription = userInfo.UserDescription,
                              Expertise = userInfo.Expertise
                          },
                          CourseReviews = _context.Courses
                              .Where(c => c.TeacherId == user.UserId)
                              .SelectMany(c => c.Reviews)
                              .Select(r => new ReviewDto
                              {
                                  ReviewId = r.ReviewId,
                                  Rating = r.Rating ?? 0,
                              }).ToList()
                      }).ToList();
        }
        public IEnumerable<User> GetUsers()
        {
            return _context.Users.Include(u => u.UserInfos).ToList();
        }

    }
}
