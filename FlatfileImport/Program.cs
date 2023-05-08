
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
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QNXT_Test;Integrated Security=True";
   
           

            // Replace the file path with the path to your flat file
            string filePath = @"C:\Users\963193\Documents\practice\QNXT_Training_Member_Inbound_PipeDelimited.txt";
      

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

                    string fileFormat = "FlatFile";

                    // Construct a SQL query that inserts the data into the database
                    string query = "INSERT INTO tzct_trn_member_import (MemberId, FullName, DOB, Gender, Addr1, Addr2, City, Zip, SSN, CreateID, Createdate, Updateid, Lastupdate,FileFormat) " +
               "VALUES (@MemberId, @FullName, @DOB, @Gender, @Addr1, @Addr2, @City, @Zip, @SSN, @CreateID, @Createdate, @Updateid, @Lastupdate, @FileFormat)";

                    // Create a SqlCommand object to execute the query
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Concatenate the first name, last name, and middle name fields into the FullName field
                        string fullName = string.Join(" ", new string[] { fields[0], fields[1], fields[2] });

                        string memberId = GenerateMemberId();
                    
                        // skip all of the header row

                        if (fields[0] == "FirstName")
                        {
                            continue;
                        }

                        // Set the parameter values for the query
                        command.Parameters.AddWithValue("@MemberId",memberId);
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@DOB", DateTime.Parse(fields[3]));
                        command.Parameters.AddWithValue("@Gender", fields[4].ToString().Substring(0, 1));

                        command.Parameters.AddWithValue("@Addr1", fields[5]);
                        command.Parameters.AddWithValue("@Addr2", fields[6]);
                        command.Parameters.AddWithValue("@City", fields[7]);
                        command.Parameters.AddWithValue("@Zip", fields[8]);
                        command.Parameters.AddWithValue("@SSN", fields[9]);
                        command.Parameters.AddWithValue("@CreateID", "DBO");
                        command.Parameters.AddWithValue("@Createdate", DateTime.Now);
                        command.Parameters.AddWithValue("@Updateid", "DBO");
                        command.Parameters.AddWithValue("@Lastupdate", DateTime.Now);
                        command.Parameters.AddWithValue("@FileFormat", fileFormat);

                       
                        // Execute the query
                        command.ExecuteNonQuery();
                    }

                }


            }
            
        }
        private static string GenerateMemberId()
        {

            string memberId = Guid.NewGuid().ToString().Substring(0, 8);
            return memberId;
        }
    }
}