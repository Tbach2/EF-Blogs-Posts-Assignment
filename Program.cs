using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace BlogsConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            var db = new BloggingContext();
            var userInput = "0";
            do
            {
                Console.WriteLine("Enter your selection:");
                Console.WriteLine("1) Display all blogs");
                Console.WriteLine("2) Add Blog");
                Console.WriteLine("3) Create Post");
                Console.WriteLine("4) Display Posts");
                Console.WriteLine("Enter q to quit");
                userInput = Console.ReadLine();
                logger.Info("Option {userInput} selected", userInput);

                // Display all Blogs from the database
                if(userInput == "1")
                {     
                    var blogs = db.Blogs.OrderBy(b => b.Name);
                    Console.WriteLine(db.Blogs.Count() + " Blogs returned");
                    foreach(var item in blogs){ Console.WriteLine(item.Name); }
                }

                // Create and save a new Blog
                else if(userInput == "2")
                {
                    Console.Write("Enter a name for a new Blog: ");
                    var name = Console.ReadLine();
                    if(name != "")
                    {
                        var blog = new Blog { Name = name };
                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }
                    else{ logger.Error("Blog name cannot be null"); }
                }

                else if(userInput == "3")
                {
                    if(db.Blogs.Count() != 0)
                    {
                        int blogId = 0; 
                        Console.WriteLine("Select the blog you would like to post to:");
                        var blogs = db.Blogs.OrderBy(b => b.BlogId);
                        foreach(var item in blogs){ Console.WriteLine(item.BlogId + ")" + " " + item.Name); }

                        try
                        {
                            blogId = Int32.Parse(Console.ReadLine());
                            if(blogId > db.Blogs.Count()){ logger.Error("There are no Blogs saved with that Id"); }
                            else 
                            {
                                Console.WriteLine("Enter the post title: ");
                                string title = Console.ReadLine();
                                if (title != "")
                                {
                                    Console.WriteLine("Enter the post content:");
                                    string content = Console.ReadLine();
                                    var post = new Post{ Title = title, Content = content, BlogId = blogId };

                                    db.AddPost(post);
                                    logger.Info("Post added - {title}", title);
                                }
                                else{ logger.Error("Post title cannot be null"); }
                            }
                        }catch(FormatException){ logger.Error("Invalid Blog Id"); }

                    }else{ Console.WriteLine(db.Blogs.Count() + " Blogs returned"); }
                }

                else if(userInput == "4")
                {}

                Console.WriteLine();//spacer
          
            }while(userInput != "q");
            logger.Info("Program ended");
        }
    }
}