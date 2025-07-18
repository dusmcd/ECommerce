﻿using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public List<Order> Orders { get; set; } = [];

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? State { get; set; }

        [Required]
        public string? ZipCode { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        public bool IsUser { get; set; }

        public string? SessionId { get; set; }


        [Required, DataType(DataType.Time)]
        public DateTime CreatedAt { get; set; }

        [Required, DataType(DataType.Time)]
        public DateTime UpdatedAt { get; set; }
    }
}
