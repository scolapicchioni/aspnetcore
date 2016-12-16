using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ConsoleClient.Models
{
    [DataContract]
    public class TodoItem
    {
        [DataMember(Name ="key")]
        public string Key { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "isComplete")]
        public bool IsComplete { get; set; }

        public override string ToString()
        {
            return $"{Key} - {Name} - {(IsComplete ? "Completed" : "Not Completed")}";
        }
    }
}
