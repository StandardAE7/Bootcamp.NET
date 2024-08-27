using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Models
{
    [Table("TodoDetail")]
    public class TodoDetail
    {
        [Key]
        [Required]
        public Guid TodoDetailId { get; set; }

        [Required]
        public string Activity { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string DetailNote { get; set; }

        [Required]
        public Guid TodoId { get; set; }

        [ForeignKey("TodoId")]
        public Todo Todo { get; set; }
    }
}
