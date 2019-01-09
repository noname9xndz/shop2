using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Model.Abstract
{
    // public lớp ảo và triển khai interface IAuditable
    // khi dùng chỉ cần kế thừa lớp ảo này Auditable
    // C# cho đa kế thừa interface nhưng lại không cho đa kế thừa class
    public abstract class Auditable : IAuditable
    {

        public DateTime? CreatedDate { set; get; }

        [MaxLength(256)]
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        [MaxLength(256)]
        public string UpdatedBy { set; get; }

        [MaxLength(256)]
        public string MetaKeyword { set; get; }

        [MaxLength(256)]
        public string MetaDescription { set; get; }

        public bool Status { set; get; }
    }

}
