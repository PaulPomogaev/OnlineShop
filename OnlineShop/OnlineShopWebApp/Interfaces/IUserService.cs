using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        List<Role> GetAllRoles();
        User? GetUserById(int id);
        UserEdit GetUserEditModel(int id);
        void UpdateUserProfile(UserEdit model);
        ChangePassword GetChangePasswordModel(int userId);
        void ChangeUserPassword(ChangePassword model);
        UserRole GetUserRoleModel(int userId);
        void AssignUserRoles(UserRole model);
        void DeleteUser(int id);
        UserCreate GetUserCreateModel();
        void CreateUser(UserCreate model);
        UserDetails GetUserDetails(int userId);
    }
}
