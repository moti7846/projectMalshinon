using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectMalshinon
{
    internal class people
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string secret_code { get; set; }
        public string type_people { get; set; }
        public int num_reports { get; set; }
        public int num_mentions { get; set; }

        public people(int id, string first_name, string last_name, string secret_code, string type_people, int num_reports, int num_mentions)
        {
            this.id = id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.secret_code = secret_code;
            this.type_people = type_people;
            this.num_reports = num_reports;
            this.num_mentions = num_mentions;

        }
        public people(string first_name, string last_name, string secret_code, string type_people, int num_reports, int num_mentions)
        {
            this.first_name = first_name;
            this.last_name = last_name;
            this.secret_code = secret_code;
            this.type_people = type_people;
            this.num_reports = num_reports;
            this.num_mentions = num_mentions;

        }
        //public static void printer(List<people> age)
        //{
        //    Console.WriteLine("id______CodeName______RealName______Location______agentStatus______MissionsCompleted");
        //    foreach (people item in age)
        //    {
        //        Console.WriteLine($"{item.Id}       {item.CodeName}    {item.RealName}    {item.Location}         {item.agentStatus}              {item.MissionsCompleted}");
        //    }
        //}
        public override string ToString()
        {
            return 
                $"id: {id} - " + 
                $"first_name: {first_name} - " + 
                $"last_name: {last_name} - " + 
                $"secret_code: {secret_code} - " + 
                $"type_people: {type_people} - " + 
                $"num_reports: {num_reports} - " + 
                $"num_mentions: {num_mentions} - "
                ;
        }
    }
}

