using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Domain.DAL;
using BookStore.Domain.Models;
using BookStore.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using BookStore.Utility;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace BookStore.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly BookStoreContext _context;
        private readonly AppSettings _appSettings;
        private readonly IHostingEnvironment _env;

        public BookController(BookStoreContext context, IOptions<AppSettings> appSettings, IHostingEnvironment env)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _env = env;
        }

        // GET: Book
        [Route("[controller]/index")]
        [Route("[controller]")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }


        [Route("[controller]/upload")]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/upload")]
        public async Task<IActionResult> Upload([Bind("BookFile,DoubanUrl,BookEditionCommnet")] UploadViewModel vm)
        {
            if (ModelState.IsValid)
            {
                #region 根据douban Id判断图书是否存在
                int doubanId = DoubanUtil.GetDoubanId(vm.DoubanUrl);
                var book = _context.Books.FirstOrDefault(m => m.DoubanId == doubanId);
                if (book != null)
                {
                    ModelState.AddModelError("BookFile", $"书籍已经存在,图书地址:{Url.Action("Detail", "Book", new { id = book.Id })}");
                    return View(vm);
                }
                #endregion

                #region 定义图书目录,文件名,文件路径
                var bookFileFolder = Path.Combine(_env.ContentRootPath, _appSettings.UploadBookDir);
                if (!Directory.Exists(bookFileFolder)) Directory.CreateDirectory(bookFileFolder);
                var filename = Utils.GetRandomFileName() + Path.GetExtension(vm.BooKFile.FileName);
                var filepath = Path.Combine(bookFileFolder, filename);
                #endregion

                #region 获得并判断checkSum,上传文件
                string checkSum;
                using (var ms = new MemoryStream())
                {
                    vm.BooKFile.CopyTo(ms);
                    ms.Position = 0;
                    checkSum = Utils.GetCheckSum(ms);
                    var bookEdition = _context.BookEditions.FirstOrDefault(m => m.CheckSum == checkSum);
                    if (bookEdition != null)
                    {
                        ModelState.AddModelError("BookFile", $"书籍已经存在,图书地址:{Url.Action("Edition", "Book", new { id = bookEdition.Id })}");
                        return View(vm);
                    }
                    //将内存流写入文件
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        ms.Position = 0;
                        ms.CopyTo(stream);
                    }
                }
                #endregion
               
                var loginUser = _context.Users.FirstOrDefault(m => m.Username == HttpContext.User.Identity.Name);
                var doubanBook = DoubanUtil.GetDoubanBook(vm.DoubanUrl);
                var b = new Book
                {
                    Title = doubanBook.Title ?? vm.BooKFile.FileName,
                    Author = doubanBook.Author,
                    AuthorIntroduction = doubanBook.AuthorIntroduction,
                    Logo = doubanBook.Logo,
                    Translator = doubanBook.Translator,
                    Publisher = doubanBook.Publisher,
                    Isbn = doubanBook.Isbn,
                    Introduction = doubanBook.Introduction,
                    BookCatelog = doubanBook.BookCatelog,
                    DoubanId = doubanBook.SubjectId,
                    DoubanUrl = doubanBook.Url,
                    DoubanRatingScore = doubanBook.RatingScore,
                    DoubanRatingPeople = doubanBook.RatingPeople,
                    CreateTime = DateTime.Now,
                    IsDelete = 0
                };

                #region 处理标签
                var bookTagList = new List<BookTag>();
                var doubanTagList = doubanBook.TagList;
                var dbTagList = _context.Tags.Where(x => doubanTagList.Contains(x.Name)).ToList();
                doubanTagList.ForEach(doubanTagName =>
                {
                    var bookTag = new BookTag();
                    if (dbTagList.Any(x => x.Name == doubanTagName))
                    {
                        bookTag.Tag = dbTagList.FirstOrDefault(x => x.Name == doubanTagName);
                    }
                    else
                    {
                        bookTag.Tag = new Tag { Name = doubanTagName };
                    }
                    bookTag.Book = b;
                    bookTagList.Add(bookTag);
                });
                #endregion
                b.BookTags = bookTagList;

                var be = new BookEdition
                {
                    Filename = filename,
                    OriginalFilename = vm.BooKFile.FileName,
                    Hashcode = Utils.GetGuid(),
                    Filesize = vm.BooKFile.Length,
                    CheckSum = checkSum,
                    CreateTime = DateTime.Now,
                    Book = b,
                    User = loginUser
                };
                _context.Add(b);
                _context.Add(be);

                if (!string.IsNullOrEmpty(vm.BookEditionCommnet))
                {
                    var bec = new BookEditionComment
                    {
                        Comment = vm.BookEditionCommnet,
                        BookEdition = be,
                        User = loginUser,
                        CreateTime = DateTime.Now

                    };
                    _context.Add(bec);
                }
                await _context.SaveChangesAsync();

                var editionUrl = Url.Action("edition", "book", new { id = be.Id });
                TempData.Flash("success", $"图书上传成功,书名：<a href='{editionUrl}'><span class='text-danger'><b>{vm.BooKFile.FileName}</b></span></a>");
                return RedirectToAction("Upload");

            }
            return View(vm);
        }

        [Route("[controller]/search/douban")]
        public JsonResult SearchDouban(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return Json(new List<DoubanBook>());
            var bookList = DoubanUtil.GetDoubanBookList(keyword);
            return Json(bookList);
        }


        public IActionResult Detail(int id)
        {
            throw new NotImplementedException();
        }

        public IActionResult Edition(int id)
        {
            throw new NotImplementedException();
        }

        [Route("[controller]/edition/download/{guid}/{filename}")]
        public IActionResult EdtionDownload(string guid,string filename)
        {
            var loginUser = _context.Users.FirstOrDefault(m => m.Username == HttpContext.User.Identity.Name);
            var be = _context.BookEditions.FirstOrDefault(b => b.Hashcode == guid);
            string bookFileDir = Path.Combine(_env.ContentRootPath, _appSettings.UploadBookDir);
            string filePath = Path.Combine(bookFileDir, be.Filename);
            return File(new FileStream(filePath,FileMode.Open), "application/x-stream", be.OriginalFilename);
        }

        public IActionResult UploadExt()
        {
            throw new NotImplementedException();
        }
    }
}
