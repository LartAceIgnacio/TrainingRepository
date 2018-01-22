using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlastAsia.DigiBook.Domain.Models.Inventories
{
    public class Inventory
    {
        [Key]
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int QonHand { get; set; }
        public int QonReserved { get; set; }
        public int QonOrdered { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsActive { get; set; }
        public string Bin { get; set; }
    }
}