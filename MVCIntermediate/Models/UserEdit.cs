namespace MVCIntermediate.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Resources = MVCIntermediate.Properties.Resources;

    /// <summary>
    ///     Model for User Edit
    /// </summary>
    public class UserEdit : IUserEdit
    {
        /// <summary>
        ///     Gets or sets Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets First Name
        /// </summary>
        [StringLength(100, MinimumLength = 4)]
        [Display(Name = nameof(Resources.FirstName), ResourceType = typeof(Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Resources.FieldRequired),
ErrorMessageResourceType = typeof(Resources))]
        public string FirstName { get; set; }

        /// <summary>
        ///     Gets or sets Last Name
        /// </summary>
        [StringLength(50)]
        [Display(Name = nameof(Resources.LastName), ResourceType = typeof(Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Resources.FieldRequired),
ErrorMessageResourceType = typeof(Resources))]
        public string LastName { get; set; }

        /// <summary>
        ///  Gets or sets User Role
        /// </summary>
        [Display(Name = nameof(Resources.UserRole), ResourceType = typeof(Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Resources.FieldRequired),
ErrorMessageResourceType = typeof(Resources))]
        public Role UserRole { get; set; }

        /// <summary>
        /// Gets or sets Date Of Birth
        /// </summary>
        [Display(Name = nameof(Resources.DateOfBirth), ResourceType = typeof(Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Resources.FieldRequired),
ErrorMessageResourceType = typeof(Resources))]
        //[DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DateOfBirth { get; set; }
    }
}