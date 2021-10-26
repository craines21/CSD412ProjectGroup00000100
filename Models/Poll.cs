using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSD412ProjectGroup00000100.Models
{
    public class Poll
    {

        private int _pollId = 0;

        private string _userId = "";

        private string _name = "";

       private string _description = "";

       private Boolean _state = false;

       [Key]
       public int PollId
       {
            get { return _pollId; }
            set { _pollId = value; }
       }
  
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        public virtual ApplicationUser User { get; set; }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public Boolean State
        {
            get { return _state; }
            set { _state = value; }
        }
        public virtual ICollection<Item> Items { get; set; }
    }
}
