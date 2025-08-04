using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bogus;
using BookGen.Models;

namespace BookGen.Controllers
{
    public class HomeController : Controller
    {
        private static readonly object genLock = new object();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GenerateBooks(string locale = "en_US", int seed = 0, double likesAvg = 0, double reviewsAvg = 0, int page = 0)
        {
            var supportedLocales = new List<string> { "en_US", "ja", "ru", "ko" };
            string generationLocale = locale;
            if (!supportedLocales.Contains(generationLocale))
            {
                generationLocale = "en_US";
            }

            int rec = (page == 0) ? 20 : 10;
            int sIndex = (page == 0) ? 1 : (20 + (page - 1) * 10) + 1;

            List<Book> books;

            lock (genLock)
            {
                int genSeed = seed + page;
                Randomizer.Seed = new Random(genSeed);

                var random = new Random(genSeed);

                var bookFaker = new Faker<Book>(generationLocale)
                    .RuleFor(b => b.Index, (f, b) => sIndex + f.IndexFaker)
                    .RuleFor(b => b.Isbn, f => f.Commerce.Ean13())
                    .RuleFor(b => b.Title, f =>
                    {
                        if (generationLocale == "ja" || generationLocale == "ko")
                        {
                            return f.Lorem.Sentence(f.Random.Number(2, 5));
                        }
                        return f.Commerce.ProductName();
                    })
                    .RuleFor(b => b.Authors, f =>
                    {
                        int authorCount = f.Random.Number(1, 3);
                        var authors = new List<string>();
                        for (int i = 0; i < authorCount; i++)
                        {
                            authors.Add(f.Name.FullName());
                        }
                        return string.Join(", ", authors);
                    })
                    .RuleFor(b => b.Publisher, f => f.Company.CompanyName())
                    .RuleFor(b => b.Likes, f =>
                    {
                        int baseLikes = (int)likesAvg;
                        double fractionalLike = likesAvg - baseLikes;
                        if (random.NextDouble() < fractionalLike)
                        {
                            baseLikes++;
                        }
                        return baseLikes;
                    })
                    .RuleFor(b => b.Reviews, f =>
                    {
                        int reviewCount = (int)reviewsAvg;
                        double fractionalReview = reviewsAvg - reviewCount;
                        if (random.NextDouble() < fractionalReview)
                        {
                            reviewCount++;
                        }

                        var generatedReviews = new List<Review>();
                        for (var i = 0; i < reviewCount; i++)
                        {
                            string reviewText;
                            if (locale == "en_US")
                            {
                                reviewText = f.Rant.Review();
                            }
                            else
                            {
                                reviewText = f.Lorem.Sentence(10, 5);
                            }
                                generatedReviews.Add(new Review
                                {
                                    Author = f.Name.FullName(),

                                    Text = reviewText,
                                });
                        }
                        return generatedReviews;
                    })
                    .RuleFor(b => b.CoverImageUrl, (f, b) =>
                    {
                        string authUrl = Uri.EscapeDataString(b.Authors.Split(',')[0].Trim());
                        string titleUrl = Uri.EscapeDataString(b.Title);
                        return $"https://placehold.co/200x300/e2e8f0/475569?text={titleUrl}\\n\\nby\\n{authUrl}";
                    });
                books = bookFaker.Generate(rec);
            }

            return Json(books, JsonRequestBehavior.AllowGet);
        }
    }
}