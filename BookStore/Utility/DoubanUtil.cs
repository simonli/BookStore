using BookStore.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace BookStore.Utility
{
    public static class DoubanUtil
    {
        public static List<DoubanBook> GetDoubanBookList(string keyword)
        {
            keyword = WebUtility.UrlEncode(keyword.NoHtml());
            var bookList = new List<DoubanBook>();
            var doubanUrl = $"https://book.douban.com/subject_search?search_text={keyword}";

            #region 通过PhantomJS获取douban动态列表内容

            var phantomJs = new PhantomJS
            {
                CustomArgs = "  --ignore-ssl-errors=true --load-images=false "
            };
            string doubanHtml;
            using (var outFs = new MemoryStream())
            {
                try
                {
                    phantomJs.RunScript(@"
            						var system = require('system');
            						var page = require('webpage').create();
            						page.open('" + doubanUrl + @"', function() {
            							system.stdout.writeLine(page.content);
            							phantom.exit();
            						});
            					", null, null, outFs);
                }
                finally
                {
                    phantomJs.Abort(); // ensure that phantomjs.exe is stopped
                }
                outFs.Position = 0;
                var reader = new StreamReader(outFs);
                doubanHtml = reader.ReadToEnd();
            }

            #endregion


            var doc = new HtmlDocument();
            doc.LoadHtml(doubanHtml);


            var itemList = doc.DocumentNode.SelectNodes("//div[contains(@class,'sc-bZQynM')]");
            if (itemList == null) return bookList;
            foreach (var item in itemList)
            {
                DoubanBook book = new DoubanBook
                {
                    Url = item.SelectSingleNode(".//a").Attributes["href"].Value
                };
                if (book.Url.IndexOf("book.douban.com/subject/", StringComparison.Ordinal) > 0)
                {
                    //书名
                    book.Title = GetHtmlNodeText(item.SelectSingleNode(".//div[@class='title']/a"));
                    //封面图片
                    book.Logo  = item.SelectSingleNode("//div/a/img").Attributes["src"].Value;
                  
                    //图书Id
                    if (int.TryParse(Regex.Replace(book.Url, @"[^\d]*", ""), out int subjectId))
                    {
                        book.SubjectId = subjectId;
                    }
                    //出版社
                    var publisherNode = item.SelectSingleNode(".//div[contains(@class,'abstract')]");
                    if (publisherNode != null)
                    {
                        book.Publisher = GetHtmlNodeText(publisherNode);
                    }
                    //评分
                    var ratingScoreNode =
                        item.SelectSingleNode(".//span[@class='rating_nums']");
                    if (ratingScoreNode != null)
                    {
                        if (float.TryParse(GetHtmlNodeText(ratingScoreNode), out float ratingScore))
                        {
                            book.RatingScore = ratingScore;
                        }
                    }
                    //评价人数
                    var ratingPeopleNode =
                        item.SelectSingleNode(".//span[@class='pl']");
                    if (ratingPeopleNode != null)
                    {
                        var ratingPeopleStr = Regex.Replace(GetHtmlNodeText(ratingPeopleNode), @"[^\d]*", "");
                        if (int.TryParse(ratingPeopleStr, out int ratingPeople))
                        {
                            book.RatingPeople = ratingPeople;
                        }
                    }
                    bookList.Add(book);
                }
            }
            return bookList;
        }

        public static DoubanBook GetDoubanBook(string doubanUrl)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(doubanUrl);
            var rootNode = htmlDoc.DocumentNode;
            var doubanId = GetDoubanId(doubanUrl);

            var titleNode = rootNode.SelectSingleNode("//*[@id='wrapper']/h1/span");
            if (titleNode == null) return null;

            var title = GetHtmlNodeText(rootNode.SelectSingleNode("//*[@id='wrapper']/h1/span"));

            var logoNode = rootNode.SelectSingleNode("//*[@id='mainpic']/a/img");
            var logo = logoNode != null ? logoNode.Attributes["src"].Value : "";

            var infoNode = rootNode.SelectSingleNode("//*[@id='info']");
            var infoSpanNodeList = infoNode.SelectNodes(".//span[@class='pl']");
            var author = "";
            var isbn = "";
            var translator = "";
            var publisher = "";
            foreach (var spanNode in infoSpanNodeList)
            {
                var spanNodeText = GetHtmlNodeText(spanNode);
                if (spanNodeText.IndexOf(":", StringComparison.Ordinal) > 0)
                {
                    spanNodeText = spanNodeText.Split(':')[0];
                }
                switch (spanNodeText)
                {
                    case "作者":
                        author = GetHtmlNodeText(spanNode.NextSibling.NextSibling);
                        break;
                    case "译者":
                        translator = GetHtmlNodeText(spanNode.NextSibling.NextSibling);
                        break;
                    case "出版社":
                        publisher = GetHtmlNodeText(spanNode.NextSibling);
                        break;
                    case "ISBN":
                        isbn = GetHtmlNodeText(spanNode.NextSibling);
                        break;
                }
            }

            #region 图书简介

            var bookIntroNode1 = rootNode.SelectSingleNode("//*[@id='link-report']/span[2]/div/div");
            var bookIntroNode2 = rootNode.SelectSingleNode("//*[@id='link-report']/span[1]/div");
            var bookIntroNode3 = rootNode.SelectSingleNode("//*[@id='link-report']/div[1]/div");
            var bookIntroNode = bookIntroNode1 ?? bookIntroNode2 ?? bookIntroNode3;
            var bookIntro = GetHtmlNodeHtml(bookIntroNode);

            #endregion

            #region 作者简介

            var authorIntroNode1 = rootNode.SelectSingleNode("//*[@id='content']/div/div[1]/div[3]/div[2]/span[2]/div");
            var authorIntroNode2 = rootNode.SelectSingleNode("//*[@id='content']/div/div[1]/div[3]/div[2]/span[1]/div");
            var authorIntroNode3 = rootNode.SelectSingleNode("//*[@id='content']/div/div[1]/div[3]/div[2]/div/div");
            var authorIntroNode = authorIntroNode1 ?? authorIntroNode2 ?? authorIntroNode3;
            var authorIntro = GetHtmlNodeHtml(authorIntroNode);

            #endregion

            var bookCatalog = GetHtmlNodeHtml(rootNode.SelectSingleNode($"//*[@id='dir_{doubanId}_full']"));
            var ratingScoreStr =
                GetHtmlNodeText(rootNode.SelectSingleNode("//strong[contains(@class,'ll rating_num')]"));
            float.TryParse(ratingScoreStr, out float ratingScore);
            var ratingPeopleStr = GetHtmlNodeText(rootNode.SelectSingleNode("//span[contains(@property,'v:votes')]"));
            int.TryParse(ratingPeopleStr, out int ratingPeople);

            #region Tags

            var tagNodeList = rootNode.SelectNodes("//a[contains(@class,'tag')]");
            var tagList = tagNodeList.Select(GetHtmlNodeText).ToList();

//            List<string> tagList = new List<string>();
//            foreach(var tagNode in tagNodeList)
//            {
//                tagList.Add(GetHtmlNodeText(tagNode));
//            }

            #endregion


            var book = new DoubanBook
            {
                SubjectId = doubanId,
                Url = doubanUrl,
                Title = title,
                Author = author,
                AuthorSummary = authorIntro,
                Logo = logo,
                Translator = translator,
                Isbn = isbn,
                Publisher = publisher,
                BookSummary = bookIntro,
                BookCatalog = bookCatalog,
                RatingScore = ratingScore,
                RatingPeople = ratingPeople,
                TagList = tagList
            };

            return book;
        }

        public static int GetDoubanId(string doubanUrl)
        {
            return int.TryParse(Regex.Replace(doubanUrl, @"[^\d]*", ""), out int doubanId) ? doubanId : 0;
        }

        public static int GetDigitalInString(string digitalAndString)
        {
            return int.TryParse(Regex.Replace(digitalAndString, @"[^\d]*", ""), out int digital) ? digital : 0;
        }

        private static string GetHtmlNodeText(HtmlNode node)
        {
            var text = "";
            try
            {
                text = node.InnerText.Trim();
            }
            catch
            {
                // ignored
            }
            return text;
        }

        private static string GetHtmlNodeHtml(HtmlNode node)
        {
            var html = "";
            try
            {
                html = node.InnerHtml.Trim();
            }
            catch
            {
                // ignored
            }
            return html;
        }
    }
}