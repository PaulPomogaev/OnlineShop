using Microsoft.AspNetCore.Http.HttpResults;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System;
using OnlineShop.Db.Models;
using OnlineShop.Db.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace OnlineShopWebApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRolesRepository _rolesRepository;
        
        public UserService(IUserRepository userRepository, IRolesRepository rolesRepository)
        {
            _userRepository = userRepository;
            _rolesRepository = rolesRepository;
        }

        public void CreateUser(UserCreate model)
        {
            if(_userRepository.Exists(model.Login))
            {
                throw new InvalidOperationException("Пользователь с таким логином уже существует");
            }

            var data = new UserCreationData
            {
                Login = model.Login,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone
            };

            _userRepository.AddFull(data);
        }

        public void DeleteUser(int id)
        {
            _userRepository.Delete(id);
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public List<Role> GetAllRoles()
        {
            return _rolesRepository.GetAll();
        }

        public UserDetails GetUserDetails(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Пользоваетль не найден");
            }

           
            return new UserDetails
            {
                Id = user.Id,
                Login = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,     
                CreatedDate = user.CreatedDate,
                RoleNames = _userRepository.GetUserRoles(userId)
            };
        }

        public ChangePassword GetChangePasswordModel(int userId)
        {
            return new ChangePassword { UserId = userId };
        }

        public void ChangeUserPassword(ChangePassword model)
        {
            _userRepository.ChangePassword(model.UserId, model.OldPassword, model.NewPassword);
        }

        public User? GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public UserCreate GetUserCreateModel()
        {
            return new UserCreate();
        }

        public UserEdit GetUserEditModel(int id)
        {
            var user = _userRepository.GetById(id);
            if(user == null)
            {
                throw new InvalidOperationException("Пользователь не найден");
            }

            return new UserEdit
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber
            };

        }

        public UserRole GetUserRoleModel(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");

            return new UserRole
            {
                UserId = userId,
                UserLogin = user.Login,
                AllRoles = _rolesRepository.GetAll(),
                UserRoleIds = _userRepository.GetUserRoleIds(userId) 
            };
        }

        public void UpdateUserProfile(UserEdit model)
        {
            _userRepository.UpdateProfile(model.Id, model.FirstName, model.LastName, model.Email, model.Phone);
        }

        public void AssignUserRoles(UserRole model)
        {
            _userRepository.AssignRoles(model.UserId, model.UserRoleIds);
        }
    }
}
