using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PuppeteerSharp.PageCoverage;
using System.Data.SqlClient;
using static IVS.Pages.insurance_provider.viewdataModel;

namespace IVS.Pages.insurancepro
{
    public class editModel : PageModel
    {
        public coverageinfo coverage = new coverageinfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String coverageid = Request.Query["coverageid"];
            try
            {
                String connectionString = "Data Source=DESKTOP-18JPE9U\\SQLEXPRESS;Initial Catalog=verificationdb;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    String sqlQuery = "SELECT * FROM coverage WHERE coverageid=@coverageid";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@coverageid", Convert.ToInt32(coverageid));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                coverage.coverageid = "" + reader.GetInt32(0);
                                coverage.coverage_status = reader.GetString(1);
                                coverage.company = reader.GetString(2);
                                coverage.medecin_name = reader.GetString(3);
                                coverage.updatedby = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }
        public void OnPost()
        {
            coverage.coverageid = Request.Form["coverageid"];
            coverage.coverage_status = Request.Form["coverage_status"];
            coverage.company = Request.Form["company"];
            coverage.medecin_name = Request.Form["medecin_name"];
            coverage.updatedby = Request.Form["updatedby"];

            if (coverage.coverageid.Length == 0 || coverage.company.Length == 0 || coverage.medecin_name.Length == 0 ||
                coverage.updatedby.Length == 0)
            {
                errorMessage = "Fields require to be filled";
                return;
            }
            try
            {
                String connectionString = "Data Source=DESKTOP-18JPE9U\\SQLEXPRESS;Initial Catalog=verificationdb;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    String sqlQuery = "UPDATE coverage SET company=@company, medecin_name=@medecin_name, coverage_status=@coverage_status, updatedby=@updatedby WHERE coverageid=@coverageid";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))

                    {
                        cmd.Parameters.AddWithValue("@coverageid", Convert.ToInt32(coverage.coverageid));
                        cmd.Parameters.AddWithValue("@company", coverage.company);
                        cmd.Parameters.AddWithValue("@medecin_name", coverage.medecin_name);
                        cmd.Parameters.AddWithValue("@coverage_status", coverage.coverage_status);
                        cmd.Parameters.AddWithValue("@updatedby", coverage.updatedby);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            coverage.coverageid = ""; coverage.company = "";
            coverage.medecin_name = ""; coverage.coverage_status = "";
            coverage.updatedby = "";

            successMessage = "Record has been successfully entered";

            Response.Redirect("/insurancepro/viewdataInsurancePro");

        }

    }
}

