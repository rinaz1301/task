using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.ComponentModel;
using Task.Connector.ConnectorDatabase;
using Task.Connector.Helper;
using Task.Integration.Data.Models;
using Task.Integration.Data.Models.Models;

namespace Task.Connector
{
    public class ConnectorDb : IConnector
    {
        private ConnectorDbContext context;
		public ConnectorDb()
        {

        }
        public void StartUp(string connectionString)
        {
			context = new ConnectorDbContext(connectionString);
            //try
            //{
            //    var checkError = context.ChangeTracker;
            //}
            //catch(InvalidProviderException ex)
            //{
            //    Logger.Error($"{ex.Name}:{ex.Message}");
            //}
        }

        public void CreateUser(UserToCreate user)
        {
            context.Users.Add(new User
            {
                Login = user.Login,
                FirstName = user.Properties.FirstOrDefault(x => x.Name == "firstName")?.Value ?? "",
                LastName = user.Properties.FirstOrDefault(x => x.Name == "lastName")?.Value ?? "",
                MiddleName = user.Properties.FirstOrDefault(x => x.Name == "middleName")?.Value ?? "",
                TelephoneNumber = user.Properties.FirstOrDefault(x => x.Name == "telephoneNumber")?.Value ?? "",
                IsLead = user.Properties.FirstOrDefault(x => x.Name == "isLead")?.Value == "true",
            });
			context.Passwords.Add(new Passwords
			{
				UserId = user.Login,
				Password = user.HashPassword,
			});

			context.SaveChanges();

			Logger.Debug($"User {user.Login} was added.");
		}

        public IEnumerable<Property> GetAllProperties()
        {
			return context.Model.FindEntityType(typeof(User)).
                GetProperties().
                Where(p=> p.Name != "Login").
                Select(p => p.Name).
                    Union(context.Model.FindEntityType(typeof(Passwords)).
                GetProperties().
                Where(p => p.Name == "Password").
                Select(p => p.Name)).
                    Select(x => new Property
                    (
                        x, 
                        $"{x} description")
                    );
        }

        public IEnumerable<UserProperty> GetUserProperties(string userLogin)
        {
            var user = context.Users.FirstOrDefault(x => x.Login ==  userLogin);

            return user.GetType().
                GetProperties().
                Where(p => p.Name.ToUpper() != "LOGIN").
				Select(x => new UserProperty
                (
                    x.Name, 
                    user.GetType().GetProperty(x.Name).GetValue(user).ToString())
                );
        }

        public bool IsUserExists(string userLogin)
        {
            return context.Users.FirstOrDefault(x => x.Login == userLogin) == null ? false : true;
        }

        public void UpdateUserProperties(IEnumerable<UserProperty> properties, string userLogin)
        {
			var user = context.Users.FirstOrDefault(x => x.Login == userLogin);

            foreach(var p in properties)
            {
                user.GetType().GetProperty(p.Name).SetValue(user, p.Value);
            }

            context.SaveChanges();
        }

        public IEnumerable<Permission> GetAllPermissions()
        {
            return context.RequestRights.Select(x => new Permission
            (
                x.Id.ToString(),
                x.Name,
                "Request"
            )).ToList().Union(context.ItRoles.Select(x => new Permission
            (
                x.Id.ToString(),
                x.Name,
                "Role"
            )).ToList());
        }

        public void AddUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {
            context.UserRequestRights.AddRange(rightIds.
                Where(x => x.Contains("Request")).
                Select(x => new UserRequestRight
                {
                    UserId = userLogin,
                    RightId = Convert.ToInt32(x.Split(":")[1])
                }));
            context.UserItroles.AddRange(rightIds.
                Where(x => x.Contains("Role")).
                Select(x => new UserItrole
                {
                    UserId = userLogin,
                    RoleId = Convert.ToInt32(x.Split(":")[1])
                }));
            context.SaveChanges();

			Logger.Debug($"User {userLogin} permissions was added.");
		}

        public void RemoveUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {
			context.UserRequestRights.RemoveRange(rightIds.
				Where(x => x.Contains("Request")).
				Select(x => new UserRequestRight
				{
					UserId = userLogin,
					RightId = Convert.ToInt32(x.Split(":")[1])
				}));
			context.UserItroles.RemoveRange(rightIds.
				Where(x => x.Contains("Role")).
				Select(x => new UserItrole
				{
					UserId = userLogin,
					RoleId = Convert.ToInt32(x.Split(":")[1])
				}));
            context.SaveChanges();

            Logger.Debug($"User {userLogin} permissions was deleted.");
        }

        public IEnumerable<string> GetUserPermissions(string userLogin)
        {
            var userPermissions = context.UserRequestRights.
                Where(x => x.UserId == userLogin).
                Select(x => $"Request:{x.RightId}").ToList();
            var userRole = context.UserItroles.
                Where(x => x.UserId == userLogin).
                Select(x => $"Role:{x.RoleId}").ToList();

            return userPermissions.Union(userRole);
        }

        public ILogger Logger { get; set; }
    }
}