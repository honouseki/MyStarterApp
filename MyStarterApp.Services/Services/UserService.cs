using MyStarterApp.Data;
using MyStarterApp.Models.Domain;
using MyStarterApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStarterApp.Services.Services
{
    public class UserService : BaseService
    {
        private Base64StringCryptographyService _cryptographyService = new Base64StringCryptographyService();
        private const int HASH_ITERATION_COUNT = 1;
        private const int RAND_LENGTH = 15;

        // Selects all users (admin)
        public List<User> AdminSelectAll()
        {
            List<User> result = new List<User>();
            this.DataProvider.ExecuteCmd(
                "Users_AdminSelectAll",
                inputParamMapper: null,
                singleRecordMapper: delegate(IDataReader reader, short set)
                {
                    User model = UserMapper(reader);
                    result.Add(model);
                }
            );
            return result;
        }


        // Selects user by username
        public User SelectByUsername(string username)
        {
            User model = new User();
            this.DataProvider.ExecuteCmd(
                "Users_SelectByUsername",
                inputParamMapper: delegate(SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@username", username);
                },
                singleRecordMapper: delegate(IDataReader reader, short set)
                {
                    model = UserMapper(reader);
                }
            );
            return model;
        }

        // Insert new user; Register
        public int Insert(LoginUser model)
        {
            User loginModel = SelectByUsername(model.Username);
            if (loginModel.Id == 0)
            {
                int userId = 0;
                string salt;
                string hashPassword;
                string password = model.Password;

                salt = _cryptographyService.GenerateRandomString(RAND_LENGTH);
                hashPassword = _cryptographyService.Hash(password, salt, HASH_ITERATION_COUNT);
                model.Salt = salt;
                model.HashPassword = hashPassword;

                this.DataProvider.ExecuteNonQuery(
                    "Users_Insert",
                    inputParamMapper: delegate(SqlParameterCollection paramCol)
                    {
                        // Creates an output variable for id; this value will return upon successful insertion
                        SqlParameter paramId = new SqlParameter();
                        paramId.ParameterName = "@id";
                        paramId.SqlDbType = SqlDbType.Int;
                        paramId.Direction = ParameterDirection.Output;
                        paramCol.Add(paramId);

                        paramCol.AddWithValue("@username", model.Username);
                        paramCol.AddWithValue("@email", model.Email);
                        paramCol.AddWithValue("@salt", model.Salt);
                        paramCol.AddWithValue("@hashPassword", model.HashPassword);
                    },
                    returnParameters: delegate(SqlParameterCollection paramCol)
                    {
                        userId = (int)paramCol["@id"].Value;
                    }
                );
                return userId;
            }
            else
            {
                // This is sent in the case the login model DOES exist.
                // Question: Why is this set to -1? What happens when this is returned?
                // Assumption: If returning -1, then display a message stating that the username is already taken
                loginModel.Id = -1;
                return loginModel.Id;
            }
        }

        
        // Login user

        // Maps a user object
        private static User UserMapper(IDataReader reader)
        {
            User model = new User();
            int index = 0;
            model.Id = reader.GetSafeInt32(index++);
            model.Username = reader.GetSafeString(index++);
            model.Email = reader.GetSafeString(index++);
            model.RoleId = reader.GetSafeInt32(index++);
            model.Confirmed = reader.GetSafeBool(index++);
            model.Suspended = reader.GetSafeBool(index++);
            model.CreatedDate = reader.GetSafeDateTime(index++);
            model.ModifiedDate = reader.GetSafeDateTime(index++);
            model.ModifiedBy = reader.GetSafeString(index++);
            return model;
        }
    }
}
