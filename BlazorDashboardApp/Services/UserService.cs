using BlazorDashboardApp.Data;
using BlazorDashboardApp.Mappers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Data;

namespace BlazorDashboardApp.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext repository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AuthenticationStateProvider authenticationProvider;

        public UserService(ApplicationDbContext repo,
                                UserManager<IdentityUser> usermanager,
                                RoleManager<IdentityRole> rolemanager,
                                AuthenticationStateProvider authprovider)
        {
            repository = repo;
            userManager = usermanager;
            roleManager = rolemanager;
            authenticationProvider = authprovider;
        }

        /*
        public ICollection<IdentityUser> GetAllUsers()
        {
            return userManager.Users.ToList();
        }

        public void GetUserById(string userid)
        {
            if (string.IsNullOrWhiteSpace(userid))
                throw new Exception();

            userManager.FindByIdAsync(userid);
        }
        public async Task<IdentityUser> GetUserByEmail(string useremail)
        {
            if (string.IsNullOrWhiteSpace(useremail))
                throw new Exception();

            return await userManager.FindByEmailAsync(useremail);
        }
        public async Task<bool> UserExists(string useremail)
        {
            if (string.IsNullOrWhiteSpace(useremail))
                throw new Exception();

            var user = await userManager.FindByEmailAsync(useremail);
            if (user is null)
                return false;
            else
                return true;
        }
        public async Task<IdentityUser> CreateUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new Exception();

            var newuser = new IdentityUser { UserName = email, Email = email };
            var result = await userManager.CreateAsync(newuser, password);
            if (!result.Succeeded)
                throw new Exception();

            return newuser;
        }
        public async Task<bool> CreateRole(string rolename)
        {
            var role = await roleManager.FindByNameAsync(rolename);
            if (role is null)
                await roleManager.CreateAsync(new IdentityRole(rolename));
            return true;
        }
        */

        public async Task<IdentityUser> GetCurrentUser()
        {
            var authstate = await authenticationProvider.GetAuthenticationStateAsync();
            var user = await userManager.GetUserAsync(authstate.User);

            if (user is null)
                throw new AccessViolationException();

            return user;
        }
        public async Task<bool> CurrentUserHasRole(string role)
        {
            var user = await GetCurrentUser();

            if (user is null)
                return false;
            return await UserHasRole(user, role);
        }
        public async Task<bool> CurrentUserHasAnyRole(ICollection<string> roles)
        {
            var user = await GetCurrentUser();

            if (user is null)
                return false;
            return await UserHasAnyRole(user, roles);
        }

        private async Task<bool> UserHasRole(IdentityUser user, string role)
        {
            return await userManager.IsInRoleAsync(user, role);
        }
        private async Task<bool> UserHasAnyRole(IdentityUser user, ICollection<string> roles)
        {
            var rolelist = await GetAllUserRoles(user);
            foreach(var role in rolelist)
            {
                if (roles.Any(r => r == role))
                    return true;
            }
            return false;
        }
        private async Task<ICollection<string>> GetAllUserRoles(IdentityUser user)
        {
            return (await userManager.GetRolesAsync(user)).ToList();
        }

        /*
        public async Task<bool> UserAddRole(string email, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(role))
                throw new Exception();
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                throw new Exception();

            return await UserAddRole(user, role);
        }

        public async Task<bool> UserAddRole(IdentityUser user, string role)
        {
            var result = await userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }
        */
    }
}
