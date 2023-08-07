using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Mycliets.Pages.clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public String name1 = "";
        public void OnGet()//allows to see data from current client
        {
            String id = Request.Query["id"];
           // Console.WriteLine(id);
            try
            {
                String connectionString = "Data Source=DESKTOP-JDTGTVC\\SQLEXPRESS;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients where id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                                clientInfo.created = reader.GetDateTime(5).ToString();
                               
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: " + ex.ToString());

            }

          
        }
        public void OnPost()
        {//update data to client
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];

            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (  clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 ||
                clientInfo.address.Length == 0)
            {
                errorMessage = "All fields required";
                return;
            }
            try
            {
                String connnectionString = "Data Source=DESKTOP-JDTGTVC\\SQLEXPRESS;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connnection = new SqlConnection(connnectionString))
                {
                    connnection.Open();
                    String sql = "UPDATE  clients " +
                        "SET name=@name, email=@email,phone=@phone,address=@address" + "where id=@id"; 
                    using (SqlCommand command = new SqlCommand(sql, connnection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        //name1="new name="+clientInfo.name;
                        

                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@id", clientInfo.id);
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage=ex.Message;
                return;
            }
            successMessage = "successfully updated";
            Response.Redirect("/Clients/index");

        }
    }
}
