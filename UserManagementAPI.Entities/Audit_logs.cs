using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Serialization;

namespace UserManagementAPI.Entities
{
    [Table("Audit_logs")]
    public class Audit_logs
    {
        public int id { get; set; }
        public int uid { get; set; }
        public string action { get; set; }
        public string log { get; set; }
        public DateTime datetime { get; set; }

    }
}
