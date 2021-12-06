using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class EntityService
    {
        IDataContext<List<User>> context;
        IDataContext<List<Book>> contextbook = new DataContext<List<Book>>("books.xml");
        delegate int BookOperation(Book a, Book b);
        delegate int UserOperation(User a, User b);
        public enum bookEnum{
            Name,
            Author
        }
        public enum userEnum
        {
            FirstName,
            Lastname,
            Group,

        }
        public EntityService(IDataContext<List<User>> context) { 
            this.context = context;
            context.DataProvider =new XMLProvider<List<User>>();
            contextbook.DataProvider = new XMLProvider<List<Book>>();
        }
        private BookOperation getBookOperation(bookEnum op) {
            switch (op) { 
            case bookEnum.Name:return (x, y) => x.Title.CompareTo(y.Title);
            case bookEnum.Author:return (x, y) => x.Author.CompareTo(y.Author);
                default:return null;
            }
        }
        private UserOperation getUserOperation(userEnum op)
        {
            switch (op)
            {
                case userEnum.FirstName: return (x, y) => x.Firstname.CompareTo(y.Firstname);
                case userEnum.Lastname: return (x, y) => x.Lastname.CompareTo(y.Lastname);
                case userEnum.Group: return (x, y) => x.Academicgroup.CompareTo(y.Academicgroup);
                default: return null;
            }
        }
        public void addBook(string author, string title, string text, int id)
        {

            List<Book> books = contextbook.GetData();
            if (books == null)
            {
                books = new List<Book>();
            }
            Book book = new Book();
            book.Author = author;
            book.Title = title;
            book.ID = id;
            book.Exist_status = true;
            books.Add(book);
            contextbook.SetData(books);
        }
        public string showBook(bookEnum sortmethod)
        {
           
            string message="";
            List<Book> books = contextbook.GetData();
            if (books == null)
            {
                books = new List<Book>();
            }
            books.Sort(getBookOperation(sortmethod).Invoke);
            foreach (Book book in books) {
                message += "Author: " + book.Author + "\tTitle: " + book.Title + "\tID:" + book.ID+"\t Exist in Library:"+ book.Exist_status+ "\n";
            }
            return message;
            
        }



        public string deleteBookById(int id) {
            List<Book> books = contextbook.GetData();

            if (books == null)
            {
                return "you haven't added any book to the library yet";
            }
            if (books.FindAll((x) => x.ID != id).Count > 0)
            {
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
                    message +="Author: "+ book.Author + "\t Titiel: "+book.Title+"\t Exist in Library:"+book.Exist_status+"\t Id:"+book.ID+"\n";
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
                    message += "Author: " + book.Author + "\n Titiel: " + book.Title + "\n Id:" + book.ID + "\n";
                    message += book.Data+"\n";
                    return message;
                }
            }
            return "Book doesn't exist";

        }

        public string UserAddBookByID(int userId, int bookID) {
            List<Book> books = contextbook.GetData();
            List<User> users = context.GetData();
            if (users == null)
            {
                return "you haven't added any user  yet";
            }
            if (books == null)
            {
                return "you haven't added any book to the library yet";
            }
            foreach (User user in users)
            {
                if (user.Id == userId)
                {
                    if (user.Shelf.Count < 5)
                    {
                        foreach (Book book in books)
                        {
                            if (book.ID == bookID)
                            {
                                book.Exist_status = false;

                                user.Shelf.Add(book.ID);
                                context.SetData(users);
                                contextbook.SetData(books);
                                return "book added to user successfully";
                            }
                        }
                    }return "You can't add more than 4 books";
                }
            }return "The user with this ID doesn't exist";
            
        }

        public string showBooksOfUser(int userId) {
            string message = "";
            List<Book> books = contextbook.GetData();
            List<User> users = context.GetData();
            if (users == null)
            {
                users = new List<User>();
            }
            if (books == null)
            {
                books = new List<Book>();
            }
            foreach (User user in users)
            {
                if (user.Id == userId)
                { 
                        foreach (Book book in books)
                        {
                            foreach (int index in user.Shelf)
                            {
                                if (book.ID == index)
                                {
                                    message += "Author: " + book.Author + "\n Title: " + book.Title + "\n Id:" + book.ID + "\n";
                                }
                            }
                        }
                        return "You don't have any books yet ";
                }
            }
            return "The user with this ID doesn't exist";
        }
        public string changeBookTextById(int id,string text)
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
                    book.Data = text;
                    return "Book changed";
                }
            }
            return "Book doesn't exist";

        }
        public void addUser(string firstname, string lastname, string group, int id) {

            List<User> entities = context.GetData();
            if (entities == null) { 
            entities = new List<User>();
            }
            User entity = new User();
            entity.Id = id;
            entity.Firstname = firstname; 
            entity.Lastname = lastname;
            entity.Academicgroup = group;
            entities.Add(entity);
            context.SetData(entities);
            

              
        }
        public string getAllUsers(userEnum op)
        {
            List<User> entities = context.GetData();
            if (entities == null)
            {
                return "you haven't added any user  yet";
            }
            string message="";
            entities.Sort(getUserOperation(op).Invoke);
            foreach (User entity in entities) {
                message += "Firstname: "+entity.Firstname+ "\t Lastname: " + entity.Lastname + "\t Group: " + entity.Academicgroup+ "\t ID: " + entity.Id;
                message += "\n";
            }
            return message;

        }
        public string userDeleteBookById(int userid,int bookid) {
            List<Book> books = contextbook.GetData();
            List<User> users = context.GetData();
            if (users.Find((x) => x.Id == userid) != null) {
                if (users.Find((x) => x.Id == userid).Shelf.Find((x) => x == bookid)==bookid) {

                            users.Find((x) => x.Id == userid).Shelf.Remove(users.Find((x) => x.Id == userid).Shelf.Find((x) => x == bookid));
                            books.Find  ((x) => x.ID == bookid).Exist_status = true;
                            context.SetData(users);
                            contextbook.SetData(books);
                            return "book deleted";
                }
                return "book with correspond id wasn't faund";
            }
            

                return "user wasn't find";
            
        }
        public string showUsersSortByFname(int id,bookEnum userEnum)
        {

            string message = "";
            List<User> peoples = context.GetData();
            if (peoples == null)
            {
                peoples = new List<User>();
            }
            peoples.Sort((x, y) => x.Firstname.CompareTo(y.Firstname));
            foreach (User entity in peoples) { 
                message+="First name= "+entity.Firstname+"Lastname = "+entity.Lastname +"\n";
                return message;
            }
            return "Entity doesn't exist";

        }

        public string deleteUserById(int id)
        {
            List<User> users = context.GetData();

            if (users == null)
            {
                return "you haven't added any user  yet";
            }
            if (users.FindAll((x) => x.Id == id).Count > 0)
            {
                context.SetData(users.FindAll((x) => x.Id != id));
                return "book was successfully  deleted";
            }

            return "a user with this id does not exist";
        }

        private bool ValidatetName(String date)
        {
            Regex regex = new Regex(@"^[A-Z][a-z]{2,10}$");

            if (regex.IsMatch(date))
                return true;
            return false;

        }
        private bool ValidatePassport(String date)
        {
            Regex regex = new Regex(@"^[A-Z]{2}[0-9]{4}$");

            if (regex.IsMatch(date))
                return true;
            return false;

        }

    }
}
