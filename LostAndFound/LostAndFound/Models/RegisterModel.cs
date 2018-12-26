namespace LostAndFound.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class RegisterModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RegisterModel()
        {
            this.Posts = new HashSet<Post>();
            this.Replies = new HashSet<Reply>();
            this.Reports = new HashSet<Report>();
        }

        public int ID { get; set; }
        [Required]
        [DisplayName("First name")]
        [MaxLength(40, ErrorMessage = "First name must be less than 40 characters long")]
        public string FName { get; set; }
        [Required]
        [DisplayName("Last name")]
        [MaxLength(40, ErrorMessage = "Last name must be less than 40 characters long")]
        public string LName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string Email { get; set; }
        [RegularExpression("^01[0-9]{9}$", ErrorMessage = "Invalid phone number")]
        public string Mobile { get; set; }
        [DisplayName("Profile picture")]
        public string Photo { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        [MaxLength(40, ErrorMessage = "Password must be at most 40 characters long"), MinLength(8, ErrorMessage = "Password must be least 8 characters long")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        [MaxLength(40, ErrorMessage = "Password must be at most 40 characters long"), MinLength(8, ErrorMessage = "Password must be least 8 characters long")]
        public string CPassword { get; set; }
        public bool Type { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only")]
        public int Token { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reply> Replies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report> Reports { get; set; }
    }
}
