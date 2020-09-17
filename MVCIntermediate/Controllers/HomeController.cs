namespace MVCIntermediate.Controllers
{
    using MVCIntermediate.Components;
    using MVCIntermediate.DAL;
    using MVCIntermediate.Models;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Mvc;

    /// <summary>
    ///     Home Controller
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        /// <summary>
        ///     Parameterized Constructor of Home Controller --- Called using DI
        /// </summary>
        /// <param name="logger"></param>
        public HomeController(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Index action
        /// </summary>
        /// <returns></returns>
        // GET: Home
        [Route]
        public ActionResult Index()
        {
            string sUser = User.Identity.Name;
            List<UserEdit> userList = null;
            bool bStatus = false;
            try
            {
                if (string.IsNullOrEmpty(sUser))
                {
                    return RedirectToAction("Login", "User");
                }
                if (IsAdmin(sUser))
                {
                    userList = SelectAllUsers();
                    bStatus = true;
                }
                ViewBag.User = sUser;
                ViewBag.Status = bStatus;
            }
            catch (Exception ex) { _logger.Log(ex.StackTrace); }
            return View(userList);
        }

        /// <summary>
        ///     Delete action
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int? Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    string sUserName = User.Identity.Name;
                    if (!string.IsNullOrEmpty(sUserName))
                    {
                        using (DB db = new DB())
                        {
                            if (db.DeleteUser(Id))
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { _logger.Log(ex.StackTrace); }
            return View();
        }

        #region Non action
        /// <summary>
        ///     SelectAllUsers method
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public List<UserEdit> SelectAllUsers()
        {
            List<UserEdit> userList = new List<UserEdit>();
            try
            {
                string sUserName = User.Identity.Name;
                UserEdit userProfile = new UserEdit();
                if (!string.IsNullOrEmpty(sUserName))
                {
                    using (DB db = new DB())
                    {
                        userList = db.GetAllUsers();
                    }
                }
            }
            catch (Exception ex) { _logger.Log(ex.StackTrace); }
            return userList;
        }

        /// <summary>
        /// IsAdmin method
        /// </summary>
        /// <param name="sEmailId"></param>
        /// <returns></returns>
        [NonAction]
        public bool IsAdmin(string sEmailId)
        {
            bool IsAdmin = false;
            try
            {
                using (DB db = new DB())
                {
                    IsAdmin = db.VerifyAdmin(sEmailId);
                }
            }
            catch (Exception ex) { _logger.Log(ex.StackTrace); }
            return IsAdmin;
        }
        #endregion
    }
}