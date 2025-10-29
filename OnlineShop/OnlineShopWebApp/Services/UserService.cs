using Microsoft.AspNetCore.Http.HttpResults;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System;
using OnlineShop.Db.Models;
using OnlineShop.Db.Interfaces;

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

        public void AssignUserRoles(UserRole model)
        {
            _userRepository.AssignRoles(model.UserId, model.UserRoleIds);
        }

        public void CreateUser(UserCreate model)
        {
            if(_userRepository.Exists(model.Login))
            {
                throw new InvalidOperationException("Пользователь с таким логином уже существует");
            }

            _userRepository.AddFull(
                model.Login,
                model.Password,
                model.FirstName,
                model.LastName,
                model.Email,
                model.Phone
                );
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

            var allRoles = _rolesRepository.GetAll();
            var roleNames = allRoles.Where(r => user.RoleIds.Contains(r.Id)).Select(r => r.Name).ToList();
            return new UserDetails
            {
                Id = user.Id,
                Login = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                CreatedDate = user.CreatedDate,
                RoleNames = roleNames
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
                Phone = user.Phone
            };

        }

        public UserRole GetUserRoleModel(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Пользователь не найден"); ;
            }

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
    }
}
