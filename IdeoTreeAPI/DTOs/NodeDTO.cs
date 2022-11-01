using IdeoTreeAPI.Model;
using System.ComponentModel.DataAnnotations;

namespace IdeoTreeAPI.DTOs
{
    public class NodeDTO
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"A-z0-9")]
        public string Data { get; set; }
        public ICollection<NodeDTO> Children { get; set; }
    }
}
