using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace projectMalshinon
{
    internal class DAL
    {
        private DAL() { }
        static DAL _DAL = null;
        private string connStr = "server=localhost;user=root;password=;database=malshinon";
        private MySqlConnection _conn;
        public people report { get; set; }
        public people target { get; set; }
        public static DAL GetInstance()
        {
            if (_DAL == null)
            {
                _DAL = new DAL();
            }
            return _DAL;
        }
        public void GetPersonByName(string first_name, string last_name, people r_t)
        {
            try
            {
                using (var connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    string q = $"SELECT * FROM `people` WHERE first_name = '{first_name}'";
                    var cmd = new MySqlCommand(q, connection);
                    var reader = cmd.ExecuteReader();
                    if(reader.Read())
                    {
                        r_t.id = reader.GetInt32("id");
                        r_t.first_name = reader.GetString("first_name");
                        r_t.last_name = reader.GetString("last_name");
                        r_t.secret_code = reader.GetString("secret_code");
                        r_t.type_people = reader.GetString("type_people");
                        r_t.num_reports = reader.GetInt32("num_reports");
                        r_t.num_mentions = reader.GetInt32("num_mentions");

                        if(r_t.first_name == report.first_name)
                        {
                            report = r_t;
                        }
                        else if(r_t.first_name == target.first_name)
                        {
                            target = r_t;
                        }
                    }
                    else
                    {
                        InsertNewPerson(first_name, last_name);
                        GetPersonByName(first_name, last_name, new people(first_name, last_name));
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("MySQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
            }
        }
        public void GetPersonBySecretCode(string SecretCode)
        {
            try
            {
                using (var connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    string q = $"SELECT * FROM `people` WHERE secret_code = '{SecretCode}'";
                    var cmd = new MySqlCommand(q, connection);
                    var reader = cmd.ExecuteReader();
                    if(reader.Read())
                    {
                        string Fname = reader.GetString("first_name");
                        string Lname = reader.GetString("last_name");
                        GetPersonByName(Fname, Lname, new people(Fname, Lname));
                    }
                    else
                    {
                        Console.WriteLine("enter your full name: ");
                        string[] name = Console.ReadLine().Split();
                        GetPersonByName(name[0], name[1], report = new people(name[0], name[1]));
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("MySQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
            }
        }
        public void InsertNewPerson(string first_name, string last_name, string type_people = "target")
        {
            try
            {
                using (var connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    Guid g = Guid.NewGuid();
                    string t = Convert.ToBase64String(g.ToByteArray()).Substring(2, 8);
                    string q = $"INSERT INTO `people` (first_name, last_name, type_people, secret_code) VALUES('{first_name}', '{last_name}','{type_people}', '{t}')";
                    var cmd = new MySqlCommand(q, connection);
                    cmd.ExecuteReader();
                    CreateAlert(find_id(first_name),"Added a line ib Database");
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("MySQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
            }
        }
        public string[] text_report_return_name(string str)
        {
            string Fname = null;
            string Lname = null;

            string[] parts = str.Split(' ');
            foreach (string s in parts)
            {
                if (char.IsUpper(s[0]))
                {
                    if (Fname == null)
                    {
                        Fname = s;
                    }
                    else if (Lname == null)
                    {
                        Lname = s;
                        break;
                    }
                }
            }
            return new string[] { Fname, Lname };
        }
        public void InsertIntelReport(string str)
        {
            string[] name = text_report_return_name(str);
            GetPersonByName(name[0], name[1], target = new people(name[0], name[1]));
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                string q = $"INSERT INTO `intelreports` (reporter_ID, target_ID, text_report) VALUES({report.id}, {target.id}, '{str}')";
                var cmd = new MySqlCommand(q, connection);
                cmd.ExecuteReader();
            }
            CreateAlert(find_id(name[0]),$"Report on {target.first_name} {target.last_name} sent successfully");
        }
        public void UpdateReportCount(people r_t)
        {
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                string q = $"UPDATE people SET num_reports = num_reports + 1 WHERE id = {r_t.id}";
                var cmd = new MySqlCommand(q, connection);
                cmd.ExecuteReader();
            }
        }
        public void UpdateMentionCount(people r_t)
        {
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                string q = $"UPDATE people SET num_reports = num_reports + 1 WHERE id = {r_t.id}";
                var cmd = new MySqlCommand(q, connection);
                cmd.ExecuteReader();
            }
            r_t = Refresh(r_t);
            Test_15_minutes(r_t.id);
        }
        public void GetReporterStats(people r_t)
        {
            double avg_text = 0;
            int count = 0;

            r_t = Refresh(r_t);
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                string q = $"SELECT COUNT(`reporter_ID`) count_reporter_ID , AVG(LENGTH(`text_report`)) avg_text_report FROM `intelreports` WHERE `reporter_ID` = {r_t.id}";
                var cmd = new MySqlCommand(q, connection);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    avg_text = reader.GetInt32("avg_text_report");
                    count = reader.GetInt32("count_reporter_ID");
                }
            }
            if (avg_text >= 100 && count >= 10)
            {
                Qeury($"UPDATE people SET type_people = 'potential_agent' WHERE id = {r_t.id}");
                CreateAlert(r_t.id, $"Type of ID: {r_t.id} this year for potential_agent");
            }
        }
        public void GetTargetStats(people r_t)
        {
            r_t = Refresh(r_t);
            if (r_t.num_mentions >= 20)
            {
                CreateAlert(r_t.id, "potential threat alert !");
            }
        }
        public void CreateAlert(int target_id ,string alert)
        {
            Qeury($"INSERT INTO `alerts` (target_ID, reason) VALUES({target_id}, '{alert}')");
            Console.WriteLine(alert);
        }
        public void GetAlerts()
        {
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                string q = $"SELECT * FROM `alerts`";
                var cmd = new MySqlCommand(q, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Alerts alerts = new Alerts(
                        reader.GetInt32("id"),
                        reader.GetInt32("target_ID"),
                        reader.GetDateTime("created_at"),
                        reader.GetString("reason")
                        );
                    Console.WriteLine(alerts);
                }
            }
        }
        public people Refresh(people r_t)
        {
            if (r_t.id == report.id)
                r_t = report;
            else if (r_t.id == target.id)
                r_t = target;
            return r_t;
        }
        public void Qeury(string qeury)
        {
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                var cmd = new MySqlCommand(qeury, connection);
                cmd.ExecuteReader();
            }
        }
        public void Test_15_minutes(int target_id)
        {
            int count = 0;
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                string q = $"SELECT COUNT(`timestamp`) count_row FROM `intelreports` WHERE `target_ID` = {target_id} AND timestamp BETWEEN NOW() -INTERVAL 15 MINUTE AND NOW()";
                var cmd = new MySqlCommand(q, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    count = reader.GetInt32("count_row");
                }
            }
            if(count >= 3)
            {
                CreateAlert(target_id, "Rapid reports detected");
            }
        }
        public int find_id(string first_name)
        {
            int id = 0;

            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                string q = $"SELECT * FROM `people` WHERE first_name = '{first_name}'";
                var cmd = new MySqlCommand(q, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id = reader.GetInt32("id");
                }
            }
            return id;
        }
        public void Login()
        {
            Console.Write("Enter your name or code: ");
            string[] loginig = Console.ReadLine().Split();
            if(loginig.Length == 2)
            {
                GetPersonByName(loginig[0], loginig[1], report = new people(loginig[0], loginig[1]));
            }
            else if(loginig.Length == 1)
            {
                GetPersonBySecretCode(loginig[0]);
            }
        }
        //public void GetAllPotentialAgent()
        //{

        //    using (var connection = new MySqlConnection(connStr))
        //    {
        //        connection.Open();
        //        string q = $"SELECT * FROM `people`";
        //        var cmd = new MySqlCommand(q, connection);
        //        var reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            people alerts = new people(
        //                reader.GetInt32("id"),
        //                reader.GetString("created_at"),
        //                reader.GetString("created_at"),
        //                reader.GetString("created_at"),
        //                reader.GetString("reason"),
        //                reader.GetInt32("target_ID"),
        //                reader.GetInt32("target_ID")
        //                );
        //            Console.WriteLine(alerts);
        //        }
        //    }
        //}
    }
}
