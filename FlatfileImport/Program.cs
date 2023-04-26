
using System;
using System.Data.SqlClient;
using System.IO;




namespace FlatfileImport

{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Replace the connection string values with your own values
            string connectionString = "Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;";

            // Replace the file path with the path to your flat file
            string filePath = @"C:\path\to\file.txt";

            // Open a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Read the contents of the flat file into a string array
                string[] lines = File.ReadAllLines(filePath);

                // Loop through each line of the file and insert the data into the database
                foreach (string line in lines)
                {
                    // Split the line into its individual fields
                    string[] fields = line.Split('|');

                    // Construct a SQL query that inserts the data into the database
                    string query = "INSERT INTO Members (MemberId, FullName, DOB, Gender, Addr1, Addr2, City, Zip, SSN, CreateID, Createdate, Updateid, Lastupdate) " +
               "VALUES (@MemberId, @FullName, @DOB, @Gender, @Addr1, @Addr2, @City, @Zip, @SSN, @CreateID, @Createdate, @Updateid, @Lastupdate)";

                    // Create a SqlCommand object to execute the query
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Concatenate the first name, last name, and middle name fields into the FullName field
                        string fullName = string.Join(" ", new string[] { fields[1], fields[2], fields[3] });
                        
                        
                        string gender = fields[5].Substring(0, 1);

                        // Set the parameter values for the query
                        command.Parameters.AddWithValue("@MemberId", fields[0]);
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@DOB", DateTime.Parse(fields[4]));
                        command.Parameters.AddWithValue("@Gender", fields[5]);

                        command.Parameters.AddWithValue("@Addr1", fields[6]);
                        command.Parameters.AddWithValue("@Addr2", fields[7]);
                        command.Parameters.AddWithValue("@City", fields[8]);
                        command.Parameters.AddWithValue("@Zip", fields[9]);
                        command.Parameters.AddWithValue("@SSN", fields[10]);
                        command.Parameters.AddWithValue("@CreateID", "DBO");
                        command.Parameters.AddWithValue("@Createdate", DateTime.Now);
                        command.Parameters.AddWithValue("@Updateid", "DBO");
                        command.Parameters.AddWithValue("@Lastupdate", DateTime.Now);
                       
                        // Execute the query
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}