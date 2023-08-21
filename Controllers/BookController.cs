using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineBookStoreApp.Data;
using OnlineBookStoreApp.Models;

namespace OnlineBookStoreApp.Controllers
{
    public class BookController : Controller
    {
        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Book
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("BookViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }

        

        // GET: Book/AddOrEdit/
        public IActionResult AddOrEdit(int? id)
        {
           BookViewModel bookViewModel = new BookViewModel();
            if (id > 0)
            {
                bookViewModel = FetchBookByID(id);
            }
            return View(bookViewModel);
        }

        // POST: Book/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("BookID,BookName,Author,ISBN,Price,publishedYear,Publisher")] BookViewModel bookViewModel)
        {
           
            if (ModelState.IsValid)
            {

                
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {  
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("BookAddOrEdit", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("BookId", bookViewModel.BookID);
                    sqlCmd.Parameters.AddWithValue("BookName", bookViewModel.BookName);
                    sqlCmd.Parameters.AddWithValue("Author", bookViewModel.Author);
                    sqlCmd.Parameters.AddWithValue("ISBN", bookViewModel.ISBN);
                    sqlCmd.Parameters.AddWithValue("Price", bookViewModel.Price);
                    sqlCmd.Parameters.AddWithValue("publishedYear", bookViewModel.publishedYear);
                    sqlCmd.Parameters.AddWithValue("Publisher", bookViewModel.Publisher);
                    sqlCmd.ExecuteNonQuery();
                }  
                return RedirectToAction(nameof(Index));
            }
            return View(bookViewModel);
        }

        // GET: Book/Delete/5
        public IActionResult Delete(int? id)
        {  BookViewModel bookViewModel = FetchBookByID(id);
    
            return View(bookViewModel);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("BookDeleteById", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("BookId",id);
               
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public BookViewModel FetchBookByID(int? id)
        {
            BookViewModel bookViewModel = new BookViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("BookViewByID", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("BookId", id);
                sqlDa.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    bookViewModel.BookID = Convert.ToInt32(dtbl.Rows[0]["BookId"].ToString());
                    bookViewModel.BookName = dtbl.Rows[0]["BookName"].ToString();
                    bookViewModel.Author = dtbl.Rows[0]["Author"].ToString();
                    bookViewModel.ISBN = dtbl.Rows[0]["ISBN"].ToString();
                    bookViewModel.Price = dtbl.Rows[0]["Price"].ToString();
                    bookViewModel.publishedYear = (dtbl.Rows[0]["publishedYear"].ToString());
                    bookViewModel.Publisher = dtbl.Rows[0]["Publisher"].ToString();
                }
                return bookViewModel;
            }
        }

    }
}
