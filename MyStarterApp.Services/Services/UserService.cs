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
        // Creating an instance of the cryptography service; Setting const variables for registration/login
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
            // Runs 'SelectByUsername' to make sure that the username does not exist
            // If the username does not exist, the Id returned should be 0, thus running the registration process
            User loginModel = SelectByUsername(model.Username);
            if (loginModel.Id == 0)
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
                loginModel.Id = -1;
                return loginModel.Id;
            }
        }

        
        // Login user
        public bool Login(LoginUser model, bool remember)
        {
            bool isSuccessful = false;
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
                string hashPassword = _cryptographyService.Hash(model.Password, loginModel.Salt, HASH_ITERATION_COUNT);

                // REVISIT....
                // To store into cookie later?
                UserBase resp = new UserBase()
                {
                    UserId = loginModel.Id,
                    Roles = new[] { "User" },
                    Username = loginModel.Username,
                    Email = loginModel.Email,
                    Remember = remember,
                    RoleId = loginModel.RoleId,
                    Confirmed = loginModel.Confirmed,
                    Suspended = loginModel.Suspended
                };
                // To create the cookie? Will do later
                //Claim emailClaim = new Claim(userData.Email.ToString(), "LPGallery");
                //_authenticationService.LogIn(response, new Claim[] { emailClaim });
                // This is where we use remember to store in the cookie^

                if (model.Username == loginModel.Username && hashPassword == loginModel.HashPassword)
                {
                    isSuccessful = true;
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
                    // These are to store into cookie
                    model.RoleId = reader.GetSafeInt32(index++);
                    model.Confirmed = reader.GetSafeBool(index++);
                    model.Suspended = reader.GetSafeBool(index++);
                }
            );
            return model;
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
