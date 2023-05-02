using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static IVS.Pages.insurance_provider.viewdataModel;
using System.Data.SqlClient;
namespace IVS.Pages.insurancepro
{
    public class createModel : PageModel
    {
        public coverageinfo coverage = new coverageinfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost() 
        {
            coverage.coverageid = Request.Form["coverageid"];
            coverage.coverage_status = Request.Form["coverage_status"];
            coverage.company = Request.Form["company"];
            coverage.medecin_name = Request.Form["medecin_name"];
            coverage.updatedby= Request.Form["updatedby"];

            if (coverage.coverageid.Length == 0 || coverage.company.Length == 0 || coverage.medecin_name.Length == 0 ||
                coverage.updatedby.Length == 0)
            {
                errorMessage = "Fields require to be filled";
                return;
            }
            //save data

            try
            {
                String connectionString = "Data Source=DESKTOP-18JPE9U\\SQLEXPRESS;Initial Catalog=verificationdb;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    String sqlQuery = "INSERT INTO coverage (coverageid,company,medecin_name,coverage_status,updatedby) VALUES (@coverageid,@company,@medecin_name,@coverage_status,@updatedby)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
              
                    {
                        cmd.Parameters.AddWithValue("@coverageid", coverage.coverageid);
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
                errorMessage= ex.Message; 
                return;
            }
            coverage.coverageid = ""; coverage.company=""; 
            coverage.medecin_name = ""; coverage.coverage_status = "";
            coverage.updatedby = "";

            successMessage = "New record has been successfully entered";

            Response.Redirect("/insurancepro/viewdataInsurancePro");
        }
    }
}
