using System;
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
        public int OnHand { get; set; }
        public int OnReserved { get; set; }
        public int OnOrdered { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsActive { get; set; }
        public string Bin { get; set; }
    }
}