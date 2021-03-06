using System;
using System.Collections.Generic;
using BookBorrowService;
using BookBorrowService.Models;
using BookBorrowService.Domain.Enums;
using BookBorrowService.Repositiories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BooksBorrowService.IntegrationTests.Helpers
{


    public class FakeStartup : Startup
    {
        public FakeStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<BookRequestContext>();
                if(dbContext == null)
                {
                    throw new NullReferenceException("Cannot get instance of dbContext");
                }

                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                for (int i = 1; i < 6; i++)
                {
                    dbContext.Users.Add(CreateUser(i, UserType.Customer));
                    dbContext.Books.Add(CreateBook(i, 3));
                }
                for (int i = 6; i < 11; i++)
                {
                    dbContext.Users.Add(CreateUser(i, UserType.Librarian));
                }

                dbContext.SaveChanges();
            }
        }

        private User CreateUser(int userId, UserType userType)
        {
            return new User
                {UserId = userId, FirstName = $"user{userId}", LastName = $"userLast{userId}", Type = userType};
        }
        private Book CreateBook(int bookId, int numOFCopies)
        {

            var book = new Book { BookId = bookId, NumOFCopies = numOFCopies, Title = $"book {bookId} " };
            var authorJhon = new Author
            {
                AuthorId = bookId ,
                FirstName = $"Jhon {bookId}",
                LastName = "Zeus",
                Books = new List<Book> { book }
            };
            
            book.Authors = new List<Author> { authorJhon };

            return book;
        }


    }
}