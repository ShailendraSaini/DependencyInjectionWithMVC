namespace MVCIntermediate
{
    using System;
    using System.Text;

    /// <summary>
    ///     Crypto class for encryption
    /// </summary>
    public static class Crypto
    {
        /// <summary>
        /// Hash method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Hash(string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }
    }
}