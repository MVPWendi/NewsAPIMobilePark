using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAPIMobilePark.Models.DTOs
{
    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Query { get; set; }
        public string? Language { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public string Result { get; set; } = "";
    }
}
