using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using MySqlConnector;

namespace learningapp.Pages;

public class IndexModel : PageModel
{
     public List<Course> Courses=new List<Course>();
    private readonly ILogger<IndexModel> _logger;
    private IConfiguration _configuration;
    public IndexModel(ILogger<IndexModel> logger,IConfiguration configuration)
    {
        _logger = logger;
        _configuration=configuration;
    }

    public void OnGet()
    {

        string connectionString = _configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;
        var sqlconnection = new MySqlConnection(connectionString);
        sqlconnection.Open();
        var sqlcommand = new MySqlCommand("SELECT * FROM Courses", sqlconnection);
        using (MySqlDataReader sqlDataReader = sqlcommand.ExecuteReader())
        {
            while(sqlDataReader.Read())
            {
                Courses.Add(new Course
                {
                    CourseID = Int32.Parse(sqlDataReader["CourseID"].ToString()),
                    CourseName = sqlDataReader["CourseName"].ToString(),
                    Rating = Decimal.Parse(sqlDataReader["Rating"].ToString())
                });
            }
        }
    }
}
