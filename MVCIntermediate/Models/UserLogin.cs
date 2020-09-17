namespace MVCIntermediate.Models
{
    using System.ComponentModel.DataAnnotations;
    using Resources = MVCIntermediate.Properties.Resources;

    /// <summary>
    ///     Model for User Login
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        ///     Gets or sets Email ID
        /// </summary>
        [Display(Name = nameof(Resources.EmailID), ResourceType = typeof(Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Resources.FieldRequired),
 ErrorMessageResourceType = typeof(Resources))]
        public string EmailID { get; set; }

        /// <summary>
        ///     Gets or sets Password
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Resources.FieldRequired),
 ErrorMessageResourceType = typeof(Resources))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        ///     Gets or sets Remember Me
        /// </summary>
        [ScaffoldColumn(false)]
        [Display(Name = nameof(Resources.RememberMe), ResourceType = typeof(Resources))]
        public bool RememberMe { get; set; }
    }
}