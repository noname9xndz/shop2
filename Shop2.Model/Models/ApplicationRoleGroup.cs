using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Model.Models
{
    [Table("ApplicationRoleGroups")]
    public class ApplicationRoleGroup // quản lý quyền nào thuộc nhóm nào
    {
        [Key]
        [Column(Order = 1)]
        public int GroupId { set; get; }

        [Key]
        [Column(Order = 2)]
        [StringLength(128)]
        public string RoleId { set; get; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole ApplicationRole { set; get; }

        [ForeignKey("GroupId")]
        public virtual ApplicationGroup ApplicationGroup { set; get; }
    }
}
