using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkUploadSQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start BulkLoad");
            BulkUpload BLK = new BulkUpload();
            BLK.BulkUploaddata();

        }
    }
}
