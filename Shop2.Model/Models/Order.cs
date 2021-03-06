﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop2.Model.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public virtual IEnumerable<OrderDetail> OrderDetails { set; get; }

        [Required]
        [MaxLength(50)]
        public string CustomerName { set; get; }
        [Required]
        [MaxLength(256)]
        public string CustomerAddress { set; get; }
        [Required]
        [MaxLength(256)]
        public string CustomerEmail { set; get; }
        [Required]
        [MaxLength(50)]
        public string CustomerMobile { set; get; }
        [Required]
        [MaxLength(256)]
        public string CustomerMessage { set; get; }
        public DateTime? CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public string PaymentMethod { set; get; }
        public string PaymentStatus { set; get; }

        
        public bool Status { set; get; }


        // lấy thông tin user nếu đăng nhập
        [StringLength(128)] 
        [Column(TypeName ="nvarchar")]
        public string CustomerID { set; get; }

        [ForeignKey("CustomerID")]
        public virtual ApplicationUser User { set; get; }
    }
}