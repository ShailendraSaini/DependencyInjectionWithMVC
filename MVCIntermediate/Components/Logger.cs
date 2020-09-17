namespace MVCIntermediate.Components
{
    using System.IO;
    using System.Web.Hosting;

    /// <summary>
    ///     Logger Class
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        ///     Log method
        /// </summary>
        /// <param name="logData"></param>
        public void Log(string logData)
        {
            string path = Path.Combine(HostingEnvironment.MapPath("~/app_data"), "log.txt");
            using (FileStream stream = new FileStream(path, FileMode.Append))
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(logData);
                writer.Flush();
            }
        }
    }
}