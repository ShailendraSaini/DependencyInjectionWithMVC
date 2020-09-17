namespace MVCIntermediate.Controllers
{
    using MVCIntermediate.Components;
    using MVCIntermediate.DAL;
    using MVCIntermediate.Models;
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    [RoutePrefix("User")]
    public class UserController : Controller
    {
        private readonly ILogger _logger;
        public UserController()
        {

        }
        public UserController(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Registration action [When we use a tide (~) sign with the Route attribute, it will override the route prefix]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("~/NewUser")]
        public ActionResult Registration()
        {
            return View();
        }

        /// <summary>
        ///     Registration action
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(UserModel user)
        {
            bool Status = false;
            string sMessage = "";
            try
            {
                if (ModelState.IsValid)
                {
                    #region //Email is already Exist 
                    if (IsEmailExist(user.EmailID))
                    {
                        ModelState.AddModelError(Properties.Resources.EmailExist, Properties.Resources.EmailExistsErrMsg);
                        return View(user);
                    }
                    #endregion

                    #region  Password Hashing 
                    user.Password = Crypto.Hash(user.Password);
                    user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                    #endregion

                    #region Save to Database
                    using (DB db = new DB())
                    {
                        if (db.InsertUser(user))
                        {
                            sMessage = Properties.Resources.RegistrationDone;
                            Status = true;
                        }
                        else
                        {
                            sMessage = Properties.Resources.RegistrationFailed;
                        }
                    }
                    #endregion
                }
                else
                {
                    sMessage = Properties.Resources.InvalidRequest;
                }
                ViewBag.Message = sMessage;
                ViewBag.Status = Status;
            }
            catch (Exception ex) { _logger.Log(ex.StackTrace); }
            return View(user);
        }

        /// <summary>
        ///     Login action for Http Get request
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Login()
        {
            FormsAuthentication.SignOut();
            return View();
        }

        /// <summary>
        ///     Login action for Http Post request
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "login")]
        public ActionResult Login(UserLogin login)
        {
            string sMessage = string.Empty;
            bool bStatus = false;
            try
            {
                if (ModelState.IsValid)
                {
                    using (DB db = new DB())
                    {
                        if (db.VerifyUser(login))
                        {
                            bStatus = true;
                            int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                            var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);
                            FormsAuthentication.SetAuthCookie(login.EmailID, false);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            sMessage = Properties.Resources.InvalidCredential;
                        }
                    }
                }
                else
                {
                    sMessage = Properties.Resources.InvalidRequest;
                }
                if (!bStatus)
                {
                    sMessage = Properties.Resources.InvalidCredential;
                }
                ViewBag.Message = sMessage;
                ViewBag.UserName = login.EmailID;
                ViewBag.Status = bStatus;
            }
            catch (Exception ex) { _logger.Log(ex.StackTrace); }
            return View();
        }

        /// <summary>
        ///     Logout action
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        /// <summary>
        ///     Edit action for Http Get request
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult Edit()
        {
            string sUserName = User.Identity.Name;
            UserEdit userProfile = new UserEdit();
            try
            {
                if (!string.IsNullOrEmpty(sUserName))
                {
                    using (DB db = new DB())
                    {
                        userProfile = db.GetUserInfo(sUserName);
                    }
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
            catch (Exception ex) { _logger.Log(ex.StackTrace); }
            return View(userProfile);
        }

        /// <summary>
        ///     Edit action for Http Post request
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult Edit(UserEdit userProfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string sMessage = string.Empty;
                    bool bStatus = false;
                    string sUserName = User.Identity.Name;
                    if (string.IsNullOrEmpty(sUserName))
                    {
                        return RedirectToAction("Login", "User");
                    }
                    else
                    {
                        #region Update to Database
                        using (DB db = new DB())
                        {
                            if (db.UpdateUserInfo(sUserName, userProfile))
                            {
                                sMessage = Properties.Resources.UserProfileUpdateMsg;
                                bStatus = true;
                            }
                            else
                            {
                                sMessage = Properties.Resources.UserProfileUpdateFailedMsg;
                            }
                        }
                        #endregion
                    }
                    ViewBag.Message = sMessage;
                    ViewBag.Status = bStatus;
                }
            }
            catch (Exception ex) { _logger.Log(ex.StackTrace); }
            return View(userProfile);
        }

        /// <summary>
        ///     IsEmailExist method
        /// </summary>
        /// <param name="sEmailId"></param>
        /// <returns></returns>
        [NonAction]
        public bool IsEmailExist(string sEmailId)
        {
            bool IsExists = false;
            using (DB db = new DB())
            {
                IsExists = db.IsUserExists(sEmailId);
            }
            return IsExists;
        }
    }
}