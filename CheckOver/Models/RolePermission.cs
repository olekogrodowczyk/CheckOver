using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class RolePermission
    {
        [Key]
        public int RolePermissionId { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        [ForeignKey("Permission")]
        public int PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
    }
}