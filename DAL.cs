using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace _08_06
{
    internal class DAL
    {

        public string connStr = "server=localhost;user=root;password=;database=commandoDB";
        private MySqlConnection _conn;

        public MySqlConnection openConnection()
        {
            if (_conn == null)
            {
                _conn = new MySqlConnection(connStr);
            }

            if (_conn.State != System.Data.ConnectionState.Open)
            {
                _conn.Open();
                Console.WriteLine("Connection successful.");
            }

            return _conn;
        }

        public void closeConnection()
        {
            if (_conn != null && _conn.State == System.Data.ConnectionState.Open)
            {
                _conn.Close();
                _conn = null;
            }
        }

        public DAL()
        {
            try
            {
                openConnection();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }
        public void CreateDatabase(string databaseName)
        {
            try
            {
                openConnection();
                string createDbQuery = $"CREATE DATABASE IF NOT EXISTS `{databaseName}`";
                using (var cmd = new MySqlCommand(createDbQuery, _conn))
                {
                    cmd.ExecuteNonQuery();
                }


                Console.WriteLine($"Database '{databaseName}' created (or already exists).");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating database: " + ex.Message);
            }
        }
        public void CreateTable(string tableName, string databaseName)
        {

            try
            {
                SwitchDatabase(databaseName);

                string createTableQuery = $@"
                        CREATE TABLE IF NOT EXISTS `{tableName}` (
                    Id INT PRIMARY KEY AUTO_INCREMENT,
                    CodeName VARCHAR(100),
                    RealName VARCHAR(100),
                    Location VARCHAR(100),
                    MissionsCompleted INT,
                    Status VARCHAR(50),
                    CONSTRAINT CHK_Status CHECK (`Status` IN ('Active', 'Injured', 'Missing', 'Retired'))
                    );";
                using (var cmd = new MySqlCommand(createTableQuery, _conn))
                {
                    cmd.ExecuteNonQuery();
                }


                Console.WriteLine($"Table {tableName} created (or already exists).");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating table: " + ex.Message);
            }
        }
        public void DropTable(string tableName, string databaseName)
        {

            try
            {
                SwitchDatabase(databaseName);

                string dropeTableQuery = $"DROP TABLE IF EXISTS `{tableName}`; ";
                using (var cmd = new MySqlCommand(dropeTableQuery, _conn))
                {
                    cmd.ExecuteNonQuery();
                }


                Console.WriteLine($"Table '{tableName}' dropped (if it existed).");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error dropping table: " + ex.Message);
            }
        }
        public void SwitchDatabase(string newDb)
        {
            if (_conn != null && _conn.State == ConnectionState.Open)
            {
                _conn.Close();
                _conn = null;
            }

            connStr = $"server=localhost;user=root;password=;database={newDb}";

            openConnection();

            Console.WriteLine($"Switched to database '{newDb}'");
        }
        public void AddAgent(ModelsAgent dataRow, string tableName)
        {
            try
            {
                openConnection();

                string query = $@"
            INSERT INTO {tableName} 
            (CodeName, RealName, Location, Status, MissionsCompleted)
            VALUES 
            (@CodeName, @RealName, @Location, @Status, @MissionsCompleted);";

                using (var cmd = new MySqlCommand(query, _conn))
                {
                    cmd.Parameters.AddWithValue("@CodeName", dataRow.CodeName);
                    cmd.Parameters.AddWithValue("@RealName", dataRow.RealName);
                    cmd.Parameters.AddWithValue("@Location", dataRow.Location);
                    cmd.Parameters.AddWithValue("@Status", dataRow.Status);
                    cmd.Parameters.AddWithValue("@MissionsCompleted", dataRow.MissionsCompleted);

                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Row inserted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting row: " + ex.Message);
            }
        }

        public List<ModelsAgent> getAgent(string query = "SELECT * FROM commandos")
        {
            List<ModelsAgent> agentList = new List<ModelsAgent>();
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;

            try
            {
                openConnection();
                cmd = new MySqlCommand(query, _conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int Id = reader.GetInt32("Id");
                    string CodeName = reader.GetString("CodeName");
                    string RealName = reader.GetString("RealName");
                    string Location = reader.GetString("Location");
                    int MissionsCompleted = reader.GetInt32("MissionsCompleted");
                    string Status = reader.GetString("Status");

                    ModelsAgent emp = new ModelsAgent(Id, CodeName, RealName, Location, MissionsCompleted, Status);
                    agentList.Add(emp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while fetching commandoDB: {ex.Message}");
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                closeConnection();
            }

            return agentList;
        }
        public void UpdateAgentLocation(int agentId, string newLocation)
        {
            try
            {
                openConnection();
                string updateQuery = $"UPDATE `commandos` SET `Location`='{newLocation}' WHERE id ={agentId}";
                using (var cmd = new MySqlCommand(updateQuery, _conn))
                {
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine($"The location is update.");
            }



            catch (Exception ex)
            {
                Console.WriteLine("Error updating table: " + ex.Message);
            }
        }
        public void DeleteAgent(int agentId)
        {
            try
            {
                openConnection();
                string deleteQuery = $"DELETE FROM `commandos` WHERE `Id` ={agentId}";
                using (var cmd = new MySqlCommand(deleteQuery, _conn))
                {
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine($"The agent is delete.");
            }



            catch (Exception ex)
            {
                Console.WriteLine("Error deleting table: " + ex.Message);
            }

        }




    }
}


