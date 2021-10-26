using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSD412ProjectGroup00000100.Models
{
    public class ApiPoll
    {
        private int _pollId = 0;
        private string _name = "";
        private string _description = "";

        public int PollId
        {
            get { return _pollId; }
            set { _pollId = value; }
        }

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

        public virtual ICollection<Item> Items { get; set; }
    }
}
