using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace IVS.Pages.insurance_provider
{
    public class viewdataModel : PageModel
    {
        public List<coverageinfo> coveragelist = new List<coverageinfo>();

        public void OnGet()
        {
            coveragelist.Clear();

            try
            {
                String connectionString = "Data Source=DESKTOP-18JPE9U\\SQLEXPRESS;Initial Catalog=verificationdb;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    String sqlQuery = "SELECT coverageid, company, medecin_name, coverage_status, updatedby FROM coverage";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            coverageinfo coverage = new coverageinfo();
                            coverage.coverageid = "" + reader.GetInt32(0);
                            coverage.coverage_status = reader.GetString(1);
                            coverage.company = reader.GetString(2);
                            coverage.medecin_name = reader.GetString(3);
                            coverage.updatedby = reader.GetString(4);
                            coveragelist.Add(coverage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
                // Log the error instead of printing it to the console.
            }
        }

        public class coverageinfo
        {
            public string coverageid;
            public string coverage_status;
            public string company;
            public string medecin_name;
            public string updatedby;
        }
    }
}
