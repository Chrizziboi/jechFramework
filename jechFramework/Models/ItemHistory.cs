using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    public class ItemHistory
    {

       public int internalId { get; }
       public string location { get; }
       public  DateTime dateTime { get; }


        public ItemHistory(int internalId, string location, DateTime dateTime) 
        { 
            this.internalId = internalId;
            this.location = location;
            this.dateTime = dateTime;
        }



    }
}
