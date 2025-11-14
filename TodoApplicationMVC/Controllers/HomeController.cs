using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using TodoApplicationMVC.Models;

namespace TodoApplicationMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly string conStr;

        public HomeController()
        {

            conStr = ConfigurationManager.ConnectionStrings["TodoApplication"].ConnectionString;
        }
        public ActionResult Index()
        {

            var query = "select * from todo";
            List<Todo> todos = getDataList(query);
            return View(todos);
        }


        public ActionResult addTodo()
        {

            return View();
        }

        [HttpPost]
        public ActionResult addTodo(string title, string desc)
        {
            var query = "insert into todo (Title, Description) values (@title, @desc)";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@desc", desc);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return Redirect("Index");
        }



        public ActionResult updateTodo(Guid id)
        {
            var query = "select * from todo where Id = @id";
            Todo todo = null;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    SqlDataReader data = cmd.ExecuteReader();
                    while (data.Read())
                    {
                        todo = MapDataToItem(data);
                    }
                    con.Close();
                }
            }

            if (todo == null)
            {
                return RedirectToAction("Index");
            }

            return View(todo);
        }

        [HttpPost]
        public ActionResult updateTodo(Guid? id, string title, string desc)
        {
            if (id == null)
            {
                return Redirect("Index");
            }
            var query = "update todo set Title = @title, Description = @desc where Id = @id";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("Index");
        }




        [HttpGet]
        public ActionResult delete(Guid id)
        {
            var query = "delete from todo where Id = @id";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("Index");
        }


        private List<Todo> getDataList(string query)
        {
            List<Todo> todos = new List<Todo>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader data = cmd.ExecuteReader();

                    while (data.Read())
                    {
                        todos.Add(MapDataToItem(data));
                    }

                    con.Close();
                }
            }
            return todos;
        }
        private Todo MapDataToItem(SqlDataReader data)
        {
            return new Todo
            {
                Id = (Guid)data["Id"],
                Title = data["Title"].ToString(),
                Description = data["Description"].ToString()
            };
        }
    }
}