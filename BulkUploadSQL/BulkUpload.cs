using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;

namespace BulkUploadSQL
{
    public class BulkUpload
    {
        public bool BulkUploaddata() 
        {
            string connectionString = "Data Source=DESKTOP-9KGSMC0\\SQLEXPRESS;Initial Catalog=EmployeesDb;Integrated Security=True;TrustServerCertificate=True";
            string table = "customersprocess";
            string FilePath = @"C:\Users\haris\source\repos\BulkUploadSQL\BulkUploadSQL\records_1000000.out";


            DataTable custinfo = new DataTable();
            custinfo.Columns.Add("Acronym",typeof(string));
            custinfo.Columns.Add("FirstName", typeof(string));
            custinfo.Columns.Add("LastName", typeof(string));
            custinfo.Columns.Add("City", typeof(string));
            custinfo.Columns.Add("Active", typeof(string));
            custinfo.Columns.Add("Rating", typeof(string));
            try
            {
                var lines = File.ReadAllLines(FilePath);
                var rows = lines.Where(line => ! string.IsNullOrWhiteSpace(line)).Select(line =>
                {
                    string Acronym = line.Length >= 12 ? line.Substring(0, 11).Trim() : "";
                    string FirstName = line.Length >= 25 ? line.Substring(12, 13).Trim() : "";
                    string LastName = line.Length >= 35 ? line.Substring(25, 10).Trim() : "";
                    string City = line.Length >= 45 ? line.Substring(35, 10).Trim() : "";
                    string Active = line.Length >= 47 ? line.Substring(45, 2).Trim() : "";
                    string Rating = line.Length >= 55 ? line.Substring(47,8).Trim() : "";

                    return new object[] { Acronym,FirstName,LastName,City,Active,Rating };
                });
                foreach (var row in rows)
                {
                    custinfo.Rows.Add(row);
                }
                BulkInsert(custinfo,connectionString,table);
                return true;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);  
            }
// This is to test the comment #haresh
            return false;
        }
        public void BulkInsert(DataTable custinfo, string connectionst, string table)
        {
            using (SqlBulkCopy blkcpy = new SqlBulkCopy(connectionst))
            {
                blkcpy.DestinationTableName = table;
                blkcpy.BatchSize = 100;
                blkcpy.BulkCopyTimeout = 600;
                try
                {
                    blkcpy.WriteToServer(custinfo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }
        }
    }
}
