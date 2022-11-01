using System.ComponentModel.DataAnnotations;

namespace IdeoTreeAPI.Model
{
    public class TreeNodeDB
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"A-z0-9")]
        public string Data { get; set; }
        public TreeNodeDB? Parent { get; set; }
        public int? ParentId { get; set; }
        public ICollection<TreeNodeDB> Children { get; set; }
    }
}
