using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace recyclebin2.Models
{
    public class tempUser
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
}