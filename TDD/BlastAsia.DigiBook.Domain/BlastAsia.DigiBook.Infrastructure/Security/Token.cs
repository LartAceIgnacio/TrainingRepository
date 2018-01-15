using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Security
{
    public class Token
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public string Value { get; set; }

        public int Type { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
