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

        [Route("[controller]/upload")]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/upload")]
        public async Task<IActionResult> Upload(UploadViewModel vm)
        {
            if (ModelState.IsValid)
            {
                #region 根据douban Id判断图书是否存在

                int doubanId = DoubanUtil.GetDoubanId(vm.DoubanUrl);
                var book = _context.Books.FirstOrDefault(m => m.DoubanId == doubanId);
                if (book != null)
                {
                    ModelState.AddModelError("BookFile",
                        $"书籍已经存在,图书地址:{Url.Action("Detail", "Book", new {id = book.Id})}");
                    return View(vm);
                }

                #endregion

                #region 定义图书目录,文件名,文件路径

                var bookFileFolder = Path.Combine(_env.ContentRootPath, _appSettings.UploadBookDir);
                if (!Directory.Exists(bookFileFolder)) Directory.CreateDirectory(bookFileFolder);
                var filename = Utils.GetId() + Path.GetExtension(vm.BooKFile.FileName);
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
                        ModelState.AddModelError("BookFile",
                            $"书籍已经存在,图书地址:<a href='{Url.Action("Edition", "Book", new {id = bookEdition.Id})}'>图书已经存在</a>");
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
                    Id = IdGen.NewId(AppkeyEnum.Books.ToString()),
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
                    var bookTag = new BookTag
                    {
                        Id = IdGen.NewId(AppkeyEnum.BookTags.ToString()),
                        Tag = dbTagList.Any(x => x.Name == doubanTagName)
                            ? dbTagList.FirstOrDefault(x => x.Name == doubanTagName)
                            : new Tag {Id = IdGen.NewId(AppkeyEnum.Tags.ToString()), Name = doubanTagName},
                        Book = b
                    };
                    bookTagList.Add(bookTag);
                });

                #endregion

                b.BookTags = bookTagList;

                var be = new BookEdition
                {
                    Id = IdGen.NewId(AppkeyEnum.BookEditions.ToString()),
                    Filename = filename,
                    OriginalFilename = vm.BooKFile.FileName,
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
                        Id = IdGen.NewId(AppkeyEnum.BookEditionComments.ToString()),
                        Comment = vm.BookEditionCommnet,
                        BookEdition = be,
                        User = loginUser,
                        CreateTime = DateTime.Now
                    };
                    _context.Add(bec);
                }
                await _context.SaveChangesAsync();

                var editionUrl = Url.Action("edition", "book", new {id = be.Id});
                TempData.Flash("success",
                    $"图书上传成功,书名：<a href='{editionUrl}'><span class='text-danger'><b>{vm.BooKFile.FileName}</b></span></a>");
                return RedirectToAction(nameof(Upload));
            }
            return View(vm);
        }

        [Route("[controller]/upload/ext")]
        public IActionResult UploadExt()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/upload/ext")]
        public async Task<IActionResult> UploadExt(UploadExtViewModel vm)
        {
            if (ModelState.IsValid)
            {
                #region 定义图书目录,文件名,文件路径

                var bookFileFolder = Path.Combine(_env.ContentRootPath, _appSettings.UploadBookDir);
                if (!Directory.Exists(bookFileFolder)) Directory.CreateDirectory(bookFileFolder);
                var filename = Utils.GetId() + Path.GetExtension(vm.BooKFile.FileName);
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
                        ModelState.AddModelError("BookFile",
                            $"<strong>书籍已经存在,图书地址:{Url.Action("Edition", "Book", new {id = bookEdition.Id})}</strong>");
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

                #region Logo上传

                var logoFileFolder = Path.Combine(_env.WebRootPath, _appSettings.UploadBookLogoDir);
                if (!Directory.Exists(logoFileFolder)) Directory.CreateDirectory(logoFileFolder);
                var logoFilename = Utils.GetId() + Path.GetExtension(vm.Logo.FileName);
                var logoFilepath = Path.Combine(logoFileFolder, logoFilename);
                using (var stream = new FileStream(logoFilepath, FileMode.Create))
                {
                    vm.Logo.CopyTo(stream);
                }

                #endregion

                var b = new Book
                {
                    Id = IdGen.NewId(AppkeyEnum.Books.ToString()),
                    Title = vm.Title ?? vm.BooKFile.FileName,
                    Author = vm.Author,
                    Logo = logoFilename,
                    AuthorIntroduction = vm.AuthorIntroduction,
                    Translator = vm.Translator,
                    Publisher = vm.Publisher,
                    Isbn = vm.Isbn,
                    Introduction = vm.Introduction,
                    BookCatelog = vm.BookCatelog,
                    CreateTime = DateTime.Now,
                    IsDelete = 0
                };

                #region 处理标签

                var bookTagList = new List<BookTag>();
                var tagList = vm.Tags.Split(',').ToList();

                var dbTagList = _context.Tags.Where(x => tagList.Contains(x.Name)).ToList();
                tagList.ForEach(doubanTagName =>
                {
                    var bookTag = new BookTag
                    {
                        Id = IdGen.NewId(AppkeyEnum.BookTags.ToString()),
                        Tag = dbTagList.Any(x => x.Name == doubanTagName)
                            ? dbTagList.FirstOrDefault(x => x.Name == doubanTagName)
                            : new Tag {Id = IdGen.NewId(AppkeyEnum.Tags.ToString()), Name = doubanTagName},
                        Book = b
                    };
                    bookTagList.Add(bookTag);
                });

                #endregion

                b.BookTags = bookTagList;

                var be = new BookEdition
                {
                    Id = IdGen.NewId(AppkeyEnum.BookEditions.ToString()),
                    Filename = filename,
                    OriginalFilename = vm.BooKFile.FileName,
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
                        Id = IdGen.NewId(AppkeyEnum.BookEditionComments.ToString()),
                        Comment = vm.BookEditionCommnet,
                        BookEdition = be,
                        User = loginUser,
                        CreateTime = DateTime.Now
                    };
                    _context.Add(bec);
                }
                await _context.SaveChangesAsync();

                var editionUrl = Url.Action("edition", "book", new {id = be.Id});
                TempData.Flash("success",
                    $"图书上传成功,书名：<a href='{editionUrl}'><span class='text-danger'><b>{vm.BooKFile.FileName}</b></span></a>");
                return RedirectToAction(nameof(UploadExt));
            }
            return View(vm);
        }

        [Route("[controller]/search/douban")]
        public JsonResult SearchDouban(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return Json(new List<DoubanBook>());
            var bookList = DoubanUtil.GetDoubanBookList(keyword);
            
            var subjectIdList = new List<int>();
            bookList.ForEach(x => { subjectIdList.Add(x.SubjectId); });


            var dbDoubanBookList = _context.Books.Where(x => subjectIdList.Contains(x.DoubanId)).ToList();
            
            bookList.ForEach(x =>
            {
                var dbDoubanIdList = dbDoubanBookList.Select(o => o.DoubanId).ToList();
                if (dbDoubanIdList.Contains(x.SubjectId))
                {
                    x.BookId = dbDoubanBookList.FirstOrDefault(q => q.DoubanId == x.SubjectId).Id;
                }
                else
                {
                    x.BookId = 0;
                }
            });

            return Json(bookList);
        }

        [Route("[controller]/{id}")]
        public async Task<IActionResult> Detail(long id)
        {
            var book = await _context.Books
                .Include(b => b.BookEditions).ThenInclude(be => be.BookEditionComments)
                .Include(b => b.BookTags).ThenInclude(t => t.Tag)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (book != null)
            {
                var relatedBooks = await _context.Books
                    .Where(x => x.Id != book.Id)
                    .Include(b => b.BookTags).ThenInclude(t => t.Book)
                    .ToListAsync();

                var vm = new BookDetailViewModel
                {
                    Book = book,
                    RelatedBooks = relatedBooks
                };
                return View(vm);
            }
            TempData.Flash("danger", "该书籍不存在。");
            return NotFound();
        }

        [Route("[controller]/[action]/{id}/")]
        public async Task<IActionResult> Edition(long id)
        {
            var be = await _context.BookEditions
                .Include(q => q.BookEditionComments)
                .Include(u => u.User)
                .Include(x => x.Book).FirstOrDefaultAsync(x => x.Id == id);
            if (be != null)
            {
                var vm = new BookEditionViewModel
                {
                    BookEdition = be
                };
                return View(vm);
            }
            TempData.Flash("danger", "该书籍的版本不存在。");
            return NotFound();
        }

        [Route("[controller]/edition/d/{id}/{checksum}/{filename}")]
        public IActionResult EditionDownload(long id, string checksum, string filename)
        {
            var loginUser = _context.Users.FirstOrDefault(m => m.Username == HttpContext.User.Identity.Name);
            var be = _context.BookEditions.FirstOrDefault(b => b.Id == id);
            var bookFileDir = Path.Combine(_env.ContentRootPath, _appSettings.UploadBookDir);
            var filePath = Path.Combine(bookFileDir, be.Filename);
            return File(new FileStream(filePath, FileMode.Open), "application/x-stream", be.OriginalFilename);
        }


        public IActionResult Comment(long id)
        {
            throw new NotImplementedException();
        }

        public IActionResult Tag(long id)
        {
            throw new NotImplementedException();
        }

        public IActionResult FileUpload(long id)
        {
            throw new NotImplementedException();
        }
    }
}