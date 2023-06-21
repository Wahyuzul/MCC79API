using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_roles")]
    public class Role
    {
        [Key]
        public Guid Guid { get; set; }

        [Column("name", TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("modified_date")]
        public DateTime ModifiedDate { get; set; }


    }
}
