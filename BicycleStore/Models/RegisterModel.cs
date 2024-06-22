﻿using System.ComponentModel.DataAnnotations;

namespace BicycleStore.Models
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
