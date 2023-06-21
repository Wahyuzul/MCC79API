using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_educations")]
    public class Education
    {
        [Key]
        public Guid Guid { get; set; }

        [Column("major", TypeName = "nvarchar(100)")]
        public string Major { get; set; }

        [Column("degree", TypeName = "nvarchar(10)")]
        public string Degree { get; set; }

        [Column("gpa")]
        public double GPA { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [Column("university_guid")]
        public Guid UniversityGuid { get; set; }
    }
}
