using Microsoft.EntityFrameworkCore;
using Task.Connector.ConnectorDatabase;
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
        }

        public void CreateUser(UserToCreate user)
        {
			context.Users.Add(new User
			{
				Login = user.Login,
				FirstName = user.Properties.FirstOrDefault(p => p.Name == "firstName")?.Value ?? "firstName",
				LastName = user.Properties.FirstOrDefault(p => p.Name == "lastName")?.Value ?? "lastName",
				MiddleName = user.Properties.FirstOrDefault(p => p.Name == "middleName")?.Value ?? "middleName",
				TelephoneNumber = user.Properties.FirstOrDefault(p => p.Name == "telephoneNumber")?.Value ??
								  "telephoneNumber",
				IsLead = user.Properties.FirstOrDefault(p => p.Name == "isLead")?.Value == "true",
			});

			context.Passwords.Add(new Password
			{
				UserId = user.Login,
				Password1 = user.HashPassword,
			});

			context.SaveChanges();
		}

        public IEnumerable<Property> GetAllProperties()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserProperty> GetUserProperties(string userLogin)
        {
            throw new NotImplementedException();
        }

        public bool IsUserExists(string userLogin)
        {
            return context.Users.Where(x=> x.Login == userLogin).FirstOrDefault() == null ? false : true;
        }

        public void UpdateUserProperties(IEnumerable<UserProperty> properties, string userLogin)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Permission> GetAllPermissions()
        {
            throw new NotImplementedException();
        }

        public void AddUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetUserPermissions(string userLogin)
        {
            throw new NotImplementedException();
        }

        public ILogger Logger { get; set; }
    }
}