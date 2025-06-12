using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace projectMalshinon
{
    internal class reportDAL
    {
        static reportDAL _reportDAL = null;


        private string connStr = "server=localhost;user=root;password=;database=malshinon";
        private MySqlConnection _conn;
        private reportDAL() { }

        public static reportDAL GetInstance()
        {
            if (_reportDAL == null)
            {
                _reportDAL = new reportDAL();
            }
            return _reportDAL;
        }

        //מקבל שם ובודק אם הוא קיים
        public bool People_in_Table(string first_name, string last_name)
        {
            try
            {
                using (var connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    string q = $"SELECT * FROM `people` WHERE first_name = '{first_name}'";
                    var cmd = new MySqlCommand(q, connection);
                    var reader = cmd.ExecuteReader();
                    return (reader.Read());
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
            return false;
        }
        //מוסיף ל - people
        public void addPeopleTarget(string first_name, string last_name)
        {
            try
            {
                using (var connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    Guid g = Guid.NewGuid();
                    string t = Convert.ToBase64String(g.ToByteArray()).Substring(2, 8);
                    string q = $"INSERT INTO `people` (first_name, last_name, type_people, secret_code) VALUES('{first_name}', '{last_name}', 'target', '{t}')";
                    var cmd = new MySqlCommand(q, connection);
                    cmd.ExecuteReader();
                    Console.WriteLine("Added a line ib Database");
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
        public void addPeople(string first_name, string last_name)
        {
            try
            {
                using (var connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    Guid g = Guid.NewGuid();
                    string t = Convert.ToBase64String(g.ToByteArray()).Substring(2, 8);
                    string q = $"INSERT INTO `people` (first_name, last_name, secret_code) VALUES('{first_name}', '{last_name}', '{t}')";
                    var cmd = new MySqlCommand(q, connection);
                    cmd.ExecuteReader();
                    Console.WriteLine("Added a line ib Database");
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
        //מוצא ID לפי שם
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
        //מנתח את טקסט ומוציא את הID של הבן אדם
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
            return new string[] {Fname, Lname};
        }
        //דיווח
        public void text_report(string str, int id_report, int id_target)
        {
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                string q = $"INSERT INTO `intelreports` (reporter_ID, target_ID, text_report) VALUES({id_report}, {id_target}, '{str}')";
                var cmd = new MySqlCommand(q, connection);
                cmd.ExecuteReader();
            }
            Qeury($"UPDATE people SET num_mentions = num_mentions + 1 WHERE id = {id_target}");
            Qeury($"UPDATE people SET num_reports = num_reports + 1 WHERE id = {id_report}");
            reported(id_report, id_target);
        }
        //מקבל STR של שאילתה רגילה ללא החזרת ערך
        public void Qeury(string qeury)
        {
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                var cmd = new MySqlCommand(qeury, connection);
                cmd.ExecuteReader();
            }
        }
        //בדיקת רמת הדיווח
        public void reported(int id_reporter, int id_target)
        {
            double avg_text = 0;
            double count = 0;
            int count_mentions = 0;

            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                string q = $"SELECT COUNT(`reporter_ID`) count_reporter_ID , AVG(LENGTH(`text_report`)) avg_text_report FROM `intelreports` WHERE `reporter_ID` = {id_reporter}";
                var cmd = new MySqlCommand(q, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    avg_text = reader.GetInt32("avg_text_report");
                    count = reader.GetInt32("count_reporter_ID");
                }
            }
            using (var connection = new MySqlConnection(connStr))
            {
                connection.Open();
                string q = $"SELECT * FROM `people` WHERE `id` = {id_target}";
                var cmd = new MySqlCommand(q, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    count_mentions = reader.GetInt32("num_mentions");
                }
            }
            if (avg_text >= 100 && count >= 10)
            {
                Qeury($"UPDATE people SET type_people = 'potential_agent' WHERE id = {id_reporter}");
                Console.WriteLine($"Type of ID: {id_reporter} this year for potential_agent");
            }
            if (count_mentions >= 20)
            {
                Console.WriteLine("potential threat alert !");
            }
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
    }     
}
