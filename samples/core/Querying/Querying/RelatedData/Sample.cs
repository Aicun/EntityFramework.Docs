using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EFQuerying.RelatedData
{
    public class Sample
    {
        public static void Run()
        {
            using (var context = new BloggingContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                SetupDatabase();
            }

            #region SingleInclude

            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                    .ToList();
            }

            #endregion

            #region IgnoredInclude

            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                    .Select(
                        blog => new
                        {
                            Id = blog.BlogId,
                            Url = blog.Url
                        })
                    .ToList();
            }

            #endregion

            #region MultipleIncludes

            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                    .Include(blog => blog.Owner)
                    .ToList();
            }

            #endregion

            #region SingleThenInclude

            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                    .ThenInclude(post => post.Author)
                    .ToList();
            }

            #endregion

            #region MultipleThenIncludes

            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                    .ThenInclude(post => post.Author)
                    .ThenInclude(author => author.Photo)
                    .ToList();
            }

            #endregion

            #region MultipleLeafIncludes

            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                    .ThenInclude(post => post.Author)
                    .Include(blog => blog.Posts)
                    .ThenInclude(post => post.Tags)
                    .ToList();
            }

            #endregion

            #region IncludeTree

            using (var context = new BloggingContext())
            {
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                    .ThenInclude(post => post.Author)
                    .ThenInclude(author => author.Photo)
                    .Include(blog => blog.Owner)
                    .ThenInclude(owner => owner.Photo)
                    .ToList();
            }

            #endregion

            #region Eager

            using (var context = new BloggingContext())
            {
                var blog = context.Blogs
                    .Single(b => b.BlogId == 1);

                context.Entry(blog)
                    .Collection(b => b.Posts)
                    .Load();

                context.Entry(blog)
                    .Reference(b => b.Owner)
                    .Load();
            }

            #endregion

            #region NavQueryAggregate

            using (var context = new BloggingContext())
            {
                var blog = context.Blogs
                    .Single(b => b.BlogId == 1);

                var postCount = context.Entry(blog)
                    .Collection(b => b.Posts)
                    .Query()
                    .Count();
            }

            #endregion

            #region NavQueryFiltered

            using (var context = new BloggingContext())
            {
                var blog = context.Blogs
                    .Single(b => b.BlogId == 1);

                var goodPosts = context.Entry(blog)
                    .Collection(b => b.Posts)
                    .Query()
                    .Where(p => p.Rating > 3)
                    .ToList();
            }

            #endregion
        }

        private static void SetupDatabase()
        {
            /*using (var context = new BloggingContext())
            {
                var person = new Person
                {
                    Name = "Great"
                };

                var owner = new Person
                {
                    Name = "Giant"
                };

                context.Persons.Add(person); context.Persons.Add(owner);
                context.SaveChanges();
            }*/

            using (var db = new BloggingContext())
            {
                db.Blogs.Add(
                    new Blog
                    {
                        Url = "http://sample.com/blogs/fish",
                        Posts = new List<Post>
                        {
                            new Post
                            {
                                Title = "Fish care 101",
                                Author = new Person
                                {
                                    Name = "Great1"
                                }
                            },
                            new Post
                            {
                                Title = "Caring for tropical fish",
                                Author = new Person
                                {
                                    Name = "Great2"
                                }
                            },
                            new Post
                            {
                                Title = "Types of ornamental fish", Author = new Person
                                {
                                    Name = "Great3"
                                }
                            }
                        },
                        Owner = new Person
                        {
                            Name = "Giant1"
                        }
                    });

                db.Blogs.Add(
                    new Blog
                    {
                        Url = "http://sample.com/blogs/cats",
                        Posts = new List<Post>
                        {
                            new Post
                            {
                                Title = "Cat care 101",
                                Author = new Person
                                {
                                    Name = "Great4"
                                }
                            },
                            new Post
                            {
                                Title = "Caring for tropical cats",
                                Author = new Person
                                {
                                    Name = "Great5"
                                }
                            },
                            new Post
                            {
                                Title = "Types of ornamental cats",
                                Author = new Person
                                {
                                    Name = "Great6"
                                }
                            }
                        },
                        Owner = new Person
                        {
                            Name = "Giant2"
                        }
                    });

                db.Blogs.Add(
                    new Blog
                    {
                        Url = "http://sample.com/blogs/catfish",
                        Posts = new List<Post>
                        {
                            new Post
                            {
                                Title = "Catfish care 101",
                                Author = new Person
                                {
                                    Name = "Great7"
                                }
                            },
                            new Post
                            {
                                Title = "History of the catfish name",
                                Author = new Person
                                {
                                    Name = "Great8"
                                }
                            }
                        },
                        Owner = new Person
                        {
                            Name = "Giant3"
                        }
                    });

                db.SaveChanges();
            }
        }
    }
}