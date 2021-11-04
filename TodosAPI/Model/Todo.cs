using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace TodosAPI.Model
{
    public class Todo
    {
        [Key]
        public int TodoId { get; set; }
        public string Content { get; set; }
        public bool Complited { get; set; }

    }
}
