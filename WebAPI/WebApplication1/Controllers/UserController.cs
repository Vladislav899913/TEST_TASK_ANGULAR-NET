using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserController : ApiController
    {
        public string Post(User user)
        {
            try
            {
                Regex regexEmail = new Regex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,8})+$");
                Regex regexPhone = new Regex(@"^((8|\+7)[\- ]?)?(\(?\d{3,4}\)?[\- ]?)?[\d\- ]{5,10}$");
                MatchCollection matchesEmail = regexEmail.Matches(user.email);
                MatchCollection matchesPhone = regexPhone.Matches(user.phone);
                if (user.name != "" && matchesEmail.Count > 0 && matchesPhone.Count > 0 && user.theme != "" && user.message != "")
                {
                    int countOfMathesNameAndPhone = 0;
                    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TEST_TASK-DB_CONNECTION"].ConnectionString))
                    {
                        var command = new SqlCommand(string.Empty, connection);
                        command.CommandType = CommandType.Text;
                        var adapter = new SqlDataAdapter();
                        connection.Open();

                        command.CommandText = $"select count(*) from Contacts where Name = '{user.name}' and Phone = '{user.phone}'";
                        countOfMathesNameAndPhone = (int)command.ExecuteScalar();

                        if (countOfMathesNameAndPhone == 0)
                        {
                            DataTable tableContact = new DataTable();
                            command.CommandText = $"insert into Contacts values('{user.name}', '{user.email}', '{user.phone}')";
                            adapter.SelectCommand = command;
                            adapter.Fill(tableContact);
                        }

                        command.CommandText = $"select count(*) from Themes where Theme = '{user.theme}'";
                        int countOfMathesTheme = (int)command.ExecuteScalar();

                        if (countOfMathesTheme == 0)
                        {
                            DataTable tableTheme = new DataTable();
                            command.CommandText = $"insert into Themes values('{user.theme}')";
                            adapter.SelectCommand = command;
                            adapter.Fill(tableTheme);
                        }

                        command.CommandText = $"select id from Contacts where Name = '{user.name}'";
                        int contactID = (int)command.ExecuteScalar();

                        command.CommandText = $"select id from Themes where Theme = '{user.theme}'";
                        int themeID = (int)command.ExecuteScalar();

                        command.CommandText = $"insert into Messages values('{user.message}', '{contactID}', '{themeID}')";
                        DataTable tableMessage = new DataTable();
                        adapter.SelectCommand = command;
                        adapter.Fill(tableMessage);
                    }

                    return $"Имя: {user.name}<br />Email: {user.email}<br />Телефон: {user.phone}<br />Тема: {user.theme}<br />Сообщение: {user.message}";
                } else return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
