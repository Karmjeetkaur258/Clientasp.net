using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Mycliets.Pages.clients
{
    public class CreateModel : PageModel    
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];
            

            if (clientInfo.name.Length == 0 ||clientInfo.email.Length==0|| clientInfo.phone.Length == 0 ||
                clientInfo.address.Length == 0)
            {
                errorMessage = "All fields required";
                return;
            }
            //save new data in database

            try
            {
                String connnectionString = "Data Source=DESKTOP-JDTGTVC\\SQLEXPRESS;Initial Catalog=mystore;Integrated Security=True";
                using(SqlConnection connnection = new SqlConnection(connnectionString))
                {
                    connnection.Open();
                    String sql = "insert into clients" + "(name,email,phone,address)values"
                        + "(@name,@email,@phone,@address)";
                    using (SqlCommand command=new SqlCommand(sql, connnection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;

            }
           
    

                clientInfo.name = "";
                clientInfo.email = "";
                clientInfo.phone = "";
                clientInfo.address = "";
                successMessage = "new Client added";
            Response.Redirect("/clients/index");
            

        }
    }
}
