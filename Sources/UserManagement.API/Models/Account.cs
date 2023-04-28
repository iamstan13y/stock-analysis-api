﻿using ModelLibrary.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserManagement.API.Enums;

namespace UserManagement.API.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public DateTime DateCreated { get; set; }
        public AccountType AccountType { get; set; }
        public Status Status { get; set; }
        [NotMapped]
        public string? Token { get; set; }
    }
}