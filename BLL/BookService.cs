using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class BookService
    {
        IDataContext<List<User>> contextuser;
        IDataContext<List<Book>> contextbook;
        delegate int BookOperation(Book a, Book b);
        public enum bookEnum
        {
            Title,
            Author
        }
        public BookService(IDataContext<List<User>> context, IDataProvider<List<User>> userProvider,
                     IDataContext<List<Book>> bookcontext, IDataProvider<List<Book>> bookProvider)
        {
            this.contextuser = context;
            context.DataProvider = userProvider;
            contextbook = bookcontext;
            contextbook.DataProvider = bookProvider;
            contextuser.ConnectionString = "myfile";
            contextbook.ConnectionString = "book.xml";

        }

        private BookOperation getBookOperation(bookEnum op)
        {
            switch (op)
            {
                case bookEnum.Title: return (x, y) => x.Title.CompareTo(y.Title);
                case bookEnum.Author: return (x, y) => x.Author.CompareTo(y.Author);
                default: return null;
            }
        }

        public string addBook(string author, string title, string text, int id)
        {
            if (!ValidatetName(author)) return "Incorrect author name format";
            if (!ValidatetName(title)) return "Incorrect title name format";

            List<Book> books = contextbook.GetData();
            if (books == null)
            {
                books = new List<Book>();
            }
            if (books.Find((x) => x.ID == id) != null) return "book with such ID already exist";
            Book book = new Book();
            book.Author = author;
            book.Title = title;
            book.ID = id;
            book.Exist_status = true;
            book.Data = text;
            books.Add(book);
            contextbook.SetData(books);
            return "book added";
        }
        public string showBook(bookEnum sortmethod)
        {

            string message = "";
            List<Book> books = contextbook.GetData();
            if (books == null)
            {
                return "you haven't added any book to the library yet";
            }
            books.Sort(getBookOperation(sortmethod).Invoke);
            foreach (Book book in books)
            {
                message += "Author: " + book.Author + "\tTitle: " + book.Title + "\tID:" + book.ID + "\t Exist in Library:" + book.Exist_status + "\n";
            }
            return message;

        }



        public string deleteBookById(int id, UserService service)
        {
            List<Book> books = contextbook.GetData();
            List<User> users = contextuser.GetData();

            if (books == null)
            {
                return "you haven't added any book to the library yet";
            }
            if (books.Find((x) => x.ID == id) != null)
            {
                if (users != null)
                {
                    foreach (User user in users)
                    {
                        service.userDeleteBookById(user.Id, id);
                    }
                }
                contextbook.SetData(books.FindAll((x) => x.ID != id));
                return "book was successfully  deleted";
            }

            return "a book with this id does not exist";
        }

        public string findBookByID(int id)
        {
            string message = "";
            List<Book> books = contextbook.GetData();
            if (books == null)
            {
                books = new List<Book>();
            }
            foreach (Book book in books)
            {
                if (book.ID == id)
                {
                    message += "Author: " + book.Author + "\t Titiel: " + book.Title + "\t Exist in Library:" + book.Exist_status + "\t Id:" + book.ID + "\n";
                    return message;
                }
            }
            return "Book doesn't exist";

        }
        public string showDataOfBookByID(int id)
        {
            string message = "";
            List<Book> books = contextbook.GetData();
            if (books == null)
            {
                return "you haven't added any book to the library yet";
            }
            foreach (Book book in books)
            {
                if (book.ID == id)
                {
                    message += "Author: " + book.Author + "\t Titiel: " + book.Title + "\t Exist in Library:" + book.Exist_status + "\t Id:" + book.ID + "\n";
                    message += book.Data + "\n";
                    return message;
                }
            }
            return "Book doesn't exist";

        }
        public string changeBookTextById(int id, string text)
        {
            string message = "";
            List<Book> books = contextbook.GetData();
            if (books == null)
            {
                return "you haven't added any book to the library yet";
            }
            foreach (Book book in books)
            {
                if (book.ID == id)
                {
                    book.Data = text;
                    contextbook.SetData(books);
                    return "Book changed";
                }
            }
            return "Book doesn't exist";

        }
        public string SearchBook(string keyword)
        {
            String message = "";
            List<Book> books = contextbook.GetData();
            if (books == null)
            {
                return "you haven't added any book  yet";
            }
            foreach (Book book in books)
            {
                if (book.Author.Contains(keyword))
                {
                    message += "Author: " + book.Author + "\tTitle: " + book.Title + "\tID:" + book.ID + "\t Exist in Library:" + book.Exist_status + "\n";
                    continue;
                }
                if (book.Title.Contains(keyword))
                {
                    message += "Author: " + book.Author + "\tTitle: " + book.Title + "\tID:" + book.ID + "\t Exist in Library:" + book.Exist_status + "\n";
                    continue;
                }
                if (book.Data.Contains(keyword))
                {
                    message += "Author: " + book.Author + "\tTitle: " + book.Title + "\tID:" + book.ID + "\t Exist in Library:" + book.Exist_status + "\n";
                    continue;
                }
            }
            return message.Equals("") ? "nothing was found" : message;
        }
        public string SearchUser(string keyword)
        {
            String message = "";
            List<User> users = contextuser.GetData();
            if (users == null)
            {
                return "you haven't added any user  yet";
            }
            foreach (User user in users)
            {
                if (user.Firstname.Contains(keyword))
                {
                    message += "First name= " + user.Firstname + "\tLastname = " + user.Lastname + "\tGroup = " + user.Academicgroup + "\tID = " + user.Id + "\n";
                    continue;
                }
                if (user.Lastname.Contains(keyword))
                {
                    message += "First name= " + user.Firstname + "\tLastname = " + user.Lastname + "\tGroup = " + user.Academicgroup + "\tID = " + user.Id + "\n";
                    continue;
                }
                if (user.Academicgroup.Contains(keyword))
                {
                    message += "First name= " + user.Firstname + "\tLastname = " + user.Lastname + "\tGroup = " + user.Academicgroup + "\tID = " + user.Id + "\n";
                    continue;
                }
            }
            return message.Equals("") ? "nothing was found" : message;
        }
        private bool ValidatetName(String date)
        {
            Regex regex = new Regex(@"^[A-Z][a-z]{2,10}$");

            if (regex.IsMatch(date))
                return true;
            return false;

        }

    }
}
