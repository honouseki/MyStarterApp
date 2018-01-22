using MyStarterApp.Data;
using MyStarterApp.Models.Domain;
using MyStarterApp.Models.Interfaces;
using MyStarterApp.Models.ViewModels;
using MyStarterApp.Services.Authentication;
using MyStarterApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyStarterApp.Services.Services
{
    public class UserService : BaseService, IUserService
    {
        // Creating an instance of the cryptography service; Setting const variables for registration/login
        private Base64StringCryptographyService _cryptographyService = new Base64StringCryptographyService();
        private IAuthenticationService _authenticationService;
        private const int HASH_ITERATION_COUNT = 1;
        private const int RAND_LENGTH = 15;

        // Using interface
        public UserService(IAuthenticationService authService)
        {
            _authenticationService = authService;
        }

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

        // Selects user by id (admin)
        public User AdminSelectById(int id)
        {
            User model = new User();
            this.DataProvider.ExecuteCmd(
                "Users_AdminSelectById",
                inputParamMapper: delegate(SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@id", id);
                },
                singleRecordMapper: delegate(IDataReader reader, short set)
                {
                    model = UserMapper(reader);
                }
            );
            return model;
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
            // Runs CheckUsername/CheckEmail to make sure that those fields do not already exist in the database
            // If the username/email does not exist, the Id returned should be 0, thus running the registration process
            int check1 = CheckUsername(model.Username);
            int check2 = CheckEmail(model.Email);
            if (check1 == 0 && check2 == 0)
            {
                // Registration Process begins by setting userId (to be returned) to 0, and declaring needed variables
                int userId = 0;
                string salt;
                string hashPassword;
                string password = model.Password;

                // Generates a random string containing 'RAND_LENGTH' characters
                salt = _cryptographyService.GenerateRandomString(RAND_LENGTH);
                // Generates a hash password containing 'HASH_ITERATION_COUNT' characters
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
                return -1;
            }
        }

        // Login user
        public int Login(LoginUser model, bool remember)
        {
            int isSuccessful = 0;
            string hashPassword;
            model.Username = model.Username.ToLower();
            // Runs 'RetrieveSaltHash' to make sure that the username does exist, 
            //     and if so, retrieve the user's salt and hash password information
            LoginUser loginModel = RetrieveSaltHash(model.Username);
            // If the username exists, Id should NOT be 1, and Salt should NOT be null/empty
            // If these requirements are met, the login process runs
            if(loginModel.Id != 0 && !String.IsNullOrEmpty(loginModel.Salt))
            {
                // Has to be multiple of 4; Q: Why?
                int multOf4 = loginModel.Salt.Length % 4;
                if (multOf4 > 0)
                {
                    // If modulus of the salt is not 0 (is > 0), 
                    //     add to salt as many "=" to make it evenly divisible by 4
                    loginModel.Salt += new string('=', 4 - multOf4);
                }

                // In the case that hashing fails, continue to return isSuccessful (which should be false)
                try
                {
                    hashPassword = _cryptographyService.Hash(model.Password, loginModel.Salt, HASH_ITERATION_COUNT);
                }
                catch (Exception)
                {
                    return isSuccessful;
                }

                // To store into cookie
                IUserAuthData resp = new UserBase()
                {
                    UserId = loginModel.Id,
                    Username = loginModel.Username,
                    Remember = remember
                };


                if (model.Username == loginModel.Username && hashPassword == loginModel.HashPassword)
                {
                    // To create the cookie
                    if (loginModel.Suspended == false)
                    {
                        _authenticationService.Login(resp, new Claim[] { });
                        isSuccessful = 1;
                    } else
                    {
                        isSuccessful = -1;
                    }
                    
                }
            }
            return isSuccessful;
        }

        // Retrieves specific user's salt and hash information; to be used to Login()
        private LoginUser RetrieveSaltHash(string username)
        {
            LoginUser model = new LoginUser();
            this.DataProvider.ExecuteCmd(
                "Users_RetrieveSaltHash",
                inputParamMapper: delegate(SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@username", username);
                },
                singleRecordMapper: delegate(IDataReader reader, short set)
                {
                    int index = 0;
                    model.Id = reader.GetSafeInt32(index++);
                    model.Username = reader.GetSafeString(index++);
                    model.Salt = reader.GetSafeString(index++);
                    model.HashPassword = reader.GetSafeString(index++);
                    model.Suspended = reader.GetSafeBool(index++);
                }
            );
            return model;
        }

        // Checks if username already exists
        public int CheckUsername(string username)
        {
            int check = 0;
            this.DataProvider.ExecuteCmd(
                "Users_CheckUsername",
                inputParamMapper: delegate(SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@username", username);
                },
                singleRecordMapper: delegate(IDataReader reader, short set)
                {
                    int index = 0;
                    check = reader.GetSafeInt32(index++);
                }
            );
            return check;
        }
        // Checks if email already exists
        public int CheckEmail(string email)
        {
            int check = 0;
            this.DataProvider.ExecuteCmd(
                "Users_CheckEmail",
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@email", email);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int index = 0;
                    check = reader.GetSafeInt32(index++);
                }
            );
            return check;
        }

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
