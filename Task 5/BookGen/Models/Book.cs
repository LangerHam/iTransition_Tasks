using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookGen.Models
{
	public class Book
	{
        public int Index { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Publisher { get; set; }
        public string CoverImageUrl { get; set; }
        public List<Review> Reviews { get; set; }
        public int Likes { get; set; }
    }
}