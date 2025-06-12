using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectMalshinon
{
    internal class Alerts
    {
        public int id { get; set; }
        public int target_ID { get; set; }
        public DateTime created_at { get; set; }
        public string reason { get; set; }

        public Alerts(int id, int target_ID, DateTime created_at, string reason)
        {
            this.id = id;
            this.target_ID = target_ID;
            this.created_at = created_at;
            this.reason = reason;
        }
        public Alerts(int target_ID, DateTime created_at, string reason)
        {
            this.target_ID = target_ID;
            this.created_at = created_at;
            this.reason = reason;
        }
        public override string ToString()
        {
            return
                $"id: {id} - " +
                $"target_ID: {target_ID} - " +
                $"created_at: {created_at} - " +
                $"reason: {reason} - "
                ;
        }
    }
}
