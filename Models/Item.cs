using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSD412ProjectGroup00000100.Models
{
    public class Item
    {

        private int _itemId = 0;

        private int _pollId = 0;

        private string _userId = "";

        private string _name = "";

        private string _description = "";

        private string _colorValue = "";

        [Key]
        public int ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }
       
        public int PollId
        {
            get { return _pollId; }
            set { _pollId = value; }
        }
        public virtual Poll Poll { get; set; }

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

        public string ColorValue
        {
            get { return _colorValue; }
            set { _colorValue = value; }
        }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
