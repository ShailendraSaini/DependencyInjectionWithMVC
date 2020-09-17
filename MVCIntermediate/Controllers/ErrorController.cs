namespace MVCIntermediate.Controllers
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Error Controller
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        ///     Index action
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int statusCode, Exception exception)
        {
            Response.StatusCode = statusCode;
            return View();
        }

        /// <summary>
        ///     HttpError404 action
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HttpError404(int statusCode, Exception exception)
        {
            Response.StatusCode = statusCode;
            return View();
        }

        /// <summary>
        ///     Http500 action
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Http500(int statusCode, Exception exception)
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}