using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class DoubanBook
    {
        public int SubjectId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string AuthorIntroduction { get; set; }
        public string Introduction { get; set; }
        public string Url { get; set; }
        public string Logo { get; set; }
        public string Publisher { get; set; }
        public string Translator { get; set; }
        public string Isbn { get; set; }
        public string BookCatelog { get; set; }
        public float RatingScore { get; set; }
        public int RatingPeople { get; set; }

        public List<string> TagList { get; set; }
    }
}
