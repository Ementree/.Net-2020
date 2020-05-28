using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DotNet2020.Domain._1
{
    class WarningsExample
    {
        /// <summary>
        /// какой-то комментарий
        /// some comment
        /// комментарий with latin and cyrillic
        /// comment с латиницей и кириллицей
        /// </summary>
        void LinqChainAndLineLength()
        {
            var col = new List<int>() { 1, 2 };
            col.Where(x => x > 0).Select(x => x * x).ToList();
            Console.WriteLine("11111111111111111111111111111111111111111111111111111111");
            var q = new Queue<int>()
                .Select(i => $"some");
        }
        protected int PropertyEnc { get; private set; }

        [HttpPost]
        public IActionResult Post(UpdateInfoDto command)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Post(UpdateSomeInfo command)
        {
            throw new NotImplementedException();
        }

        void MethodParams(int nq, int n2)
        {

        }
    }

    public interface ITest2
    {
        string Name { get; set; }
        string Name2 { get; set; }

    }

    public interface IHasTest2
    {
        //test
        bool IsTest { get; set; }
    }

    public interface IHasTest
    {
        bool TestBool { get; set; }
    }

    enum NumebrsJs
    {
        One,
        Two
    }

    //class DTOClass
    //{
    //    private void Method()
    //    {

    //    }

    //    private void PublicMethod()
    //    {

    //    }

    //}

    class Program
    {

        private class NestedClass
        {

        }

        protected class ProtectedClass
        {

        }

        public string foo2;


        protected enum Tes23
        {
            One,
            Two
        }

        public bool IsTest { get; set; }

        static void Main(string[] args)
        {

            Console.WriteLine(UniueEnum.Test);
            ///test3
        }

        public class Book
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public List<User> Users { get; set; }
        }
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }

            public List<Book> Books { get; set; }
        }
        public class ApplicationContext : DbContext
        {
            public DbSet<User> Users { get; set; }
            public DbSet<Book> Books { get; set; }


            public ApplicationContext()
            {
                Database.EnsureCreated();
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
            }
        }

        /// <summary>test
        /// <para>f'sow you could make a second paragraph in a description. <see cref="System.Console.WriteLine(System.String)"/> for information about output statements.</para>
        /// <seealso cref="TestClass.Main"/>
        /// </summary>
        private void LinqExample(IEnumerable<int> query)
        {

            ///test
            query.Where(x => x.ToString() == "12");

            var result = query
                //.Select(x => x.ToString())
                .Select(x => query.Select(t => t.ToString()))
                .Where(x => x.ToString() == "12")
                .Count();

            var db = new ApplicationContext();
            db.Users.Select(u => u.Books.Select(b => b.Name).ToList());

            db.Users.Select(u => u.Books.Select(b => b.Name));

            db.Users.FirstOrDefault(u => u.Books.Select(b => b.Name).Any(bookName => bookName == "War and Peace"));

            db.Users.Where(u => u.Books.Select(b => b.Name).Any(bookName => bookName == "War and Peace"));


            db.Users.Select(u => u.Age > 18);

            db.Users.Select(u => u.Books
                                      .Where(b => b.Id != 1)
                                      .Select(b => b.Name)
                         );

            db.Users.Select(u => u.Books
                                        .Where(b => b.Id != 1)
                                        .Select(b => b.Name)
                                        .ToList()
                           );

            db.Users.Select(u => u.Books.Select(b => b.Users.Select(u2 => u2.Name).ToList()).ToList());

        }
    }
}