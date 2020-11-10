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
                {}
                else if(userInput == "4")
                {}

                

          
            }while(userInput != "q");
            logger.Info("Program ended");
        }
    }
}