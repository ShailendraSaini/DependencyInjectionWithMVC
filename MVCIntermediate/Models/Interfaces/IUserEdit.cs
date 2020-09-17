namespace MVCIntermediate.Models
{
    using System;

    /// <summary>
    ///     IUserEdit Interface
    /// </summary>
    public interface IUserEdit
    {
        /// <summary>
        ///     Gets or sets Date Of Birth
        /// </summary>
        DateTime? DateOfBirth { get; set; }

        /// <summary>
        ///     Gets or sets First Name
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        ///     Gets or sets Id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        ///     Gets or sets Last Name
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        ///     Gets or sets User Role
        /// </summary>
        Role UserRole { get; set; }
    }
}