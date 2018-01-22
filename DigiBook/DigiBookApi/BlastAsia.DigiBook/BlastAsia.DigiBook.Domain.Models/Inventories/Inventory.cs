﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BlastAsia.DigiBook.Domain.Models.Inventories
{
    public class Inventory
    {
        [Key]
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int QOH { get; set; }
        public int QOR { get; set; }
        public int QOO { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsActive { get; set; }
        public string Bin { get; set; }
    }
}