namespace MVCIntermediate.DAL
{
    using Models;
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;

    /// <summary>
    ///     Mysql DB Class
    /// </summary>
    public class DB : IDisposable
    {
        private string _sConnString = string.Empty;
        private MySqlConnection _mySqlConnection = null;

        /// <summary>
        ///     Constructor od DB class
        /// </summary>
        public DB()
        {
            _sConnString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            _mySqlConnection = new MySqlConnection(_sConnString);
            _mySqlConnection.Open();
        }

        /// <summary>
        ///  Insert User method
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool InsertUser(UserModel user)
        {
            bool bStatus = false;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("InsertUser", _mySqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LName", user.LastName);
                    cmd.Parameters.AddWithValue("@Email", user.EmailID);
                    cmd.Parameters.AddWithValue("@DOB", user.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Pass", user.Password);
                    int nRole = user.UserRole.ToString().ToUpper() == "ADMIN" ? 1 : 0;
                    cmd.Parameters.AddWithValue("@RoleVal", nRole);
                    int nVal = cmd.ExecuteNonQuery();
                    if (nVal > 0)
                        bStatus = true;
                }
            }
            catch (Exception) { }
            return bStatus;
        }

        /// <summary>
        ///     Method to Verify User
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public bool VerifyUser(UserLogin loginUser)
        {
            bool bStatus = false;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("VerifyUser", _mySqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", loginUser.EmailID);
                    cmd.Parameters.AddWithValue("@Pass", Crypto.Hash(loginUser.Password));
                    object nVal = cmd.ExecuteScalar();
                    if (Convert.ToInt32(nVal) > 0)
                        bStatus = true;
                }
            }
            catch (Exception) { }
            return bStatus;
        }

        /// <summary>
        ///     Method to Get User Info
        /// </summary>
        /// <param name="sEmailId"></param>
        /// <returns></returns>
        public UserEdit GetUserInfo(string sEmailId)
        {
            UserEdit userInfo = new UserEdit();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("GetUserInfo", _mySqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", sEmailId);
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                userInfo.FirstName = sdr["FirstName"].ToString();
                                userInfo.LastName = sdr["LastName"].ToString();
                                userInfo.UserRole = Convert.ToInt32(sdr["Role"]) == 1 ? Role.Admin : Role.Normal;
                                userInfo.DateOfBirth = Convert.ToDateTime(sdr["DateOfBirth"]);
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
            return userInfo;
        }

        /// <summary>
        ///     Method to Get all users
        /// </summary>
        /// <returns></returns>
        public List<UserEdit> GetAllUsers()
        {
            List<UserEdit> userList = new List<UserEdit>();
            UserEdit userInfo = new UserEdit();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("GetAllUsers", _mySqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                userList.Add(new UserEdit
                                {
                                    Id = Convert.ToInt32(sdr["Id"]),
                                    FirstName = sdr["FirstName"].ToString(),
                                    LastName = sdr["LastName"].ToString(),
                                    DateOfBirth = Convert.ToDateTime(sdr["DateOfBirth"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
            return userList;
        }

        /// <summary>
        ///     Method to Update User Info
        /// </summary>
        /// <param name="sEmailId"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool UpdateUserInfo(string sEmailId, UserEdit userInfo)
        {
            bool bStatus = false;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("UpdateUserInfo", _mySqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FName", userInfo.FirstName);
                    cmd.Parameters.AddWithValue("@LName", userInfo.LastName);
                    cmd.Parameters.AddWithValue("@DOB", userInfo.DateOfBirth);
                    int nRole = userInfo.UserRole.ToString().ToUpper() == "ADMIN" ? 1 : 0;
                    cmd.Parameters.AddWithValue("@RoleVal", nRole);
                    cmd.Parameters.AddWithValue("@Email", sEmailId);
                    int nVal = cmd.ExecuteNonQuery();
                    if (nVal > 0)
                        bStatus = true;
                }
            }
            catch (Exception) { }
            return bStatus;
        }

        /// <summary>
        ///     Method use to Delet User
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteUser(int? Id)
        {
            bool bStatus = false;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("DeleteUser", _mySqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", Id);
                    int nVal = cmd.ExecuteNonQuery();
                    if (nVal > 0)
                        bStatus = true;
                }
            }
            catch (Exception) { }
            return bStatus;
        }

        /// <summary>
        ///     Method to check wether user exists or npot
        /// </summary>
        /// <param name="sEmailId"></param>
        /// <returns></returns>
        public bool IsUserExists(string sEmailId)
        {
            bool bStatus = false;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("IsUserExists", _mySqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", sEmailId);
                    object nVal = cmd.ExecuteScalar();
                    if (Convert.ToInt32(nVal) > 0)
                        bStatus = true;
                }
            }
            catch (Exception) { }
            return bStatus;
        }

        /// <summary>
        ///     Method to verify Admin user
        /// </summary>
        /// <param name="sEmailId"></param>
        /// <returns></returns>
        public bool VerifyAdmin(string sEmailId)
        {
            bool bStatus = false;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("VerifyAdmin", _mySqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", sEmailId);
                    object nVal = cmd.ExecuteScalar();
                    if (Convert.ToInt32(nVal) > 0)
                        bStatus = true;
                }
            }
            catch (Exception) { }
            return bStatus;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            _mySqlConnection.Dispose();
        }
    }
}