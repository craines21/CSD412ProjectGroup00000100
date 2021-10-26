using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSD412ProjectGroup00000100.Models
{
    public class Vote

    {
        private int _itemId = 0;

        private int _voteId = 0;

        private string _voterId = "";
       

        private DateTime _voteDateTime = DateTime.MinValue;
   
        [Key]
        public int VoteId
        {
            get { return _voteId; }
            set { _voteId = value; }
        }
        public int ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        public virtual Item Item { get; set; }

        public string VoterId
        {
            get { return _voterId; }
            set { _voterId = value; }
        }

        public virtual ApplicationUser Voter { get; set; }
        public DateTime VoteDateTime
        {
            get { return _voteDateTime; }
            set { _voteDateTime = value; }

        }
    }
}
