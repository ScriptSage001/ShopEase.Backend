using System.Data;
using Dapper;
using ShopEase.Backend.AuthService.Application;
using ShopEase.Backend.AuthService.Core.Entities;
using ShopEase.Backend.AuthService.Core.Primitives;
using ShopEase.Backend.AuthService.Infrastructure.Repository;

namespace ShopEase.Backend.AuthService.Infrastructure
{
    public class AuthServiceRepository : IAuthServiceRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public AuthServiceRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public Error RegisterUser(User user)
        {
            var dapperParams = new DynamicParameters();
            dapperParams.Add("@Id", dbType: DbType.Guid, direction: ParameterDirection.Input);
            dapperParams.Add("@Name", user.Name, dbType: DbType.String, direction: ParameterDirection.Input);
            dapperParams.Add("@Email", user.Email, dbType: DbType.String, direction: ParameterDirection.Input);
            dapperParams.Add("@MobileNumber", user.MobileNumber, dbType: DbType.String, direction: ParameterDirection.Input);
            dapperParams.Add("@AltMobileNumber", user.AltMobileNumber, dbType: DbType.String, direction: ParameterDirection.Input);

            using (var connection = _dbConnectionFactory.GetConnection())
            {
                connection.Execute(StoredProcedures.RegisterUser, dapperParams, commandType: CommandType.StoredProcedure);
            }

            return new Error(" ", " ");
        }
    }

}