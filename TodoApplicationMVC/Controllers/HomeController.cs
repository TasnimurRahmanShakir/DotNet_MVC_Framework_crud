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

        // --- MODIFIED: Index Action ---
        // Now accepts a 'name' parameter from your search form
        public ActionResult Index(string name)
        {
            string query;
            SqlParameter param = null;

            if (!string.IsNullOrEmpty(name))
            {
                // --- SEARCH PATH: Build a LIKE query ---
                query = "select * from todo WHERE Title LIKE @search OR Description LIKE @search";
                param = new SqlParameter("@search", $"%{name}%"); // Add wildcards
                ViewBag.Title = $"Search results for '{name}'";
            }
            else
            {
                // --- DEFAULT PATH: Get all todos ---
                query = "select * from todo";
                ViewBag.Title = "Home Page";
            }

            List<Todo> todos = getDataList(query, param);
            return View(todos);
        }

        public ActionResult addTodo()
        {
            return View();
        }

        // --- MODIFIED: addTodo Post Action ---
        // Added Guid generation to ensure the ID is saved
        [HttpPost]
        public ActionResult addTodo(string title, string desc)
        {
            // Create the ID in C#
            var newId = Guid.NewGuid();

            var query = "insert into todo (Id, Title, Description) values (@id, @title, @desc)";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", newId); // Add the new Guid
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@desc", desc);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index"); // Corrected Redirect
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
                return RedirectToAction("Index");
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

        // --- MODIFIED: getDataList Method ---
        // Now accepts an optional SqlParameter
        private List<Todo> getDataList(string query, SqlParameter parameter = null)
        {
            List<Todo> todos = new List<Todo>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Add the parameter if it exists
                    if (parameter != null)
                    {
                        cmd.Parameters.Add(parameter);
                    }

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