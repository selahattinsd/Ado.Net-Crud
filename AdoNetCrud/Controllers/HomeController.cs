using AdoNetCrud.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace AdoNetCrud.Controllers
{
    public class HomeController : Controller
    {
       public string conntionString = " Server = 104.247.162.242\\MSSQLSERVER2017;Database=akadem58_sd;User Id = akadem58_sd; Password=******;";

       
        public IActionResult Index()
        {
            var blogPosts = new List<BlogPost>();
            using(SqlConnection connection = new SqlConnection(conntionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT id, title , summary, slug FROM Blog ORDER BY created_on DESC", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var blogItem = new BlogPost();
                    blogItem.id = reader.GetInt32(0);
                    blogItem.title = reader.GetString(1);
                    blogItem.summary = reader.GetString(2);
                    blogItem.slug = reader.GetString(3);
                    blogPosts.Add(blogItem);

                }
            }

            return View(blogPosts);
        }
        [Route("{slug}")]
        public IActionResult Detay(string slug)
        {
            var blogItem = new BlogPostDetail();

            using(SqlConnection connection = new SqlConnection(conntionString))
            {
                try
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT title, content FROM Blog WHERE slug = @slug", connection);
                    command.Parameters.AddWithValue("@slug", slug);
                    var reader = command.ExecuteReader();
                    reader.Read();
                    blogItem.title= reader.GetString(1);
                    blogItem.content= reader.GetString(2);
                }
                catch 
                {

                    HttpContext.Response.StatusCode = 404;
                    return View("SayfaBulunamadı");
                }
            }
            return View(blogItem);
        }
        public IActionResult IcerikEkle ()
        {
            return View();
        }
        [HttpPost]
        public IActionResult IcerikEkle(BlogModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Msg = "Eksik İçerik Var";
                return View("IcerikEkle");
            }
            using (SqlConnection connection = new SqlConnection(conntionString))
            {
                try
                {
                    connection.Open();
                    var command = new SqlCommand(
                         "INSERT INTO Blog (title, summary, content, slug, created_on, updated_on) VALUES (@title, @summary, @content, @slug, @created_on, @updated_on)",
                        connection);
                    command.Parameters.AddWithValue("@title", model.title);
                    command.Parameters.AddWithValue("@summary", model.summary);
                    command.Parameters.AddWithValue("@content", model.content);
                    command.Parameters.AddWithValue("@slug", model.slug);
                    command.Parameters.AddWithValue("@created_on", DateTime.Now);
                    command.Parameters.AddWithValue("@updated_on", DateTime.Now);

                    command.ExecuteNonQuery();
                    return RedirectToAction("IcerikEklendi");

                }
                catch (Exception e)
                {

                    ViewBag.Msg = "Eklenemedi. Hata oldu. Git bir bak istersen. " + e.ToString();
                    return View("IcerikEkle");

                }
            }


                
        }
        public IActionResult IcerikEklendi()
        {
            ViewBag.MsgTitle = "Blog Eklendi.";
            ViewBag.Msg = "Blog Eklendi.";
            return View("Mesaj");
        }
        public IActionResult Duzenle(int id)
        {
            using (SqlConnection connection = new SqlConnection(conntionString))
            {
                try
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT id, slug, title, summary, content FROM Blog WHERE id = @id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    var reader = command.ExecuteReader();

                    reader.Read();

                    var blogItem = new BlogPost();
                    blogItem.id = reader.GetInt32(0);
                    blogItem.slug = reader.GetString(1);
                    blogItem.title = reader.GetString(2);
                    blogItem.summary = reader.GetString(3);
                    blogItem.content = reader.GetString(4);

                    return View("IcerikDuzenle", blogItem);

                }
                catch 
                {

                    ViewBag.Msg = "Hata oldu. Daha sonra tekrar deneyiniz.";
                    return View("Mesaj");
                }
            }
        }
        [HttpPost]
        public IActionResult Duzenle(BlogUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Msg = "Hatalı form gönderdiniz";
                return View("Mesaj");
            }

            using (SqlConnection connection = new SqlConnection(conntionString))
            {
                try
                {
                    connection.Open();

                    var command = new SqlCommand(
                            "UPDATE BlogSET title = @title, summary = @summary, content = @content, slug = @slug, updated_on = @updated_on WHERE id = @id",
                            connection);

                    command.Parameters.AddWithValue("@id", model.id);
                    command.Parameters.AddWithValue("@title", model.title);
                    command.Parameters.AddWithValue("@summary", model.summary);
                    command.Parameters.AddWithValue("@content", model.content);
                    command.Parameters.AddWithValue("@slug", model.slug);
                    command.Parameters.AddWithValue("@updated_on", DateTime.Now);

                    command.ExecuteNonQuery();

                    return RedirectToAction("IcerikGuncellendi");

                }
                catch (Exception e)
                {
                    ViewBag.Msg = "Hata oldu. Daha sonra tekrar deneyiniz.";
                    return View("Mesaj");
                }
            }

        }
        public IActionResult IcerikGuncellendi()
        {
            ViewBag.MsgTitle = "İçerik güncellendi";
            ViewBag.Msg = "İçerik güncellendi";
            return View("Mesaj");
        }
        public IActionResult Sil(int id)
        {

            using (SqlConnection connection = new SqlConnection(conntionString))
            {
                try
                {
                    connection.Open();

                    var command = new SqlCommand(
                            "DELETE FROM contents WHERE id = @id",
                            connection);

                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();

                    ViewBag.MsgTitle = "İçerik Silindi";
                    ViewBag.Msg = "İçerik Silindi";
                    return View("Mesaj");

                }
                catch (Exception e)
                {
                    ViewBag.MsgTitle = "İçerik bulunamadı";
                    ViewBag.Msg = "Böyle bir içerik bulunamadı";
                    return View("Mesaj");
                }
            }

        }
        public IActionResult Listele()
        {
            var blogPosts = new List<BlogPost>();
            using (SqlConnection connection = new SqlConnection(conntionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT id, title , summary, slug FROM Blog ORDER BY created_on DESC", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var blogItem = new BlogPost();
                    blogItem.id = reader.GetInt32(0);
                    blogItem.title = reader.GetString(1);
                    blogItem.summary = reader.GetString(2);
                    blogItem.slug = reader.GetString(3);
                    blogPosts.Add(blogItem);

                }
            }
            return View(blogPosts);
        }




    }
}