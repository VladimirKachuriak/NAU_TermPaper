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
        IDataContext<List<Book>> contextbook;
        DateTime Time;
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
        public EntityService(IDataContext<List<User>> context,IDataProvider<List<User>> userProvider, IDataContext<List<Book>> bookcontext, IDataProvider<List<Book>> bookProvider) { 
            this.context = context;
            context.DataProvider = userProvider;
            contextbook = bookcontext;
            contextbook.DataProvider = bookProvider;

            Time = DateTime.Now;

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
            books.Add(book);
            contextbook.SetData(books);
            return "book added";
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
                    if (user.Shelf.Count < 4)
                    {
                        foreach (Book book in books)
                        {
                            if (book.ID == bookID)
                            {
                                if (user.Shelf.Find((x) => x == bookID) == 0)
                                {
                                    if (books.Find((x)=>x.ID==bookID).Exist_status==true){
                                        book.Exist_status = false;
                                        book.DeadLine = DateTime.Now.AddDays(3);
                                        user.Shelf.Add(book.ID);
                                        context.SetData(users);
                                        contextbook.SetData(books);
                                        return "book added to user successfully";
                                    } return "This book already got another student";
                                }return  "book already in your shelf";
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
                                    message += "Author: " + book.Author + "\t Title: " + book.Title + "\t Id:" + book.ID + "\n";
                                }
                            }
                        }
                        return message.Equals("")?"You don't have any books yet":message;
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
        public string addUser(string firstname, string lastname, string group, int id) {

            if (!ValidatetName(firstname)) return "Incorrect firstname format";
            if (!ValidatetName(lastname)) return "Incorrect lastname format";
            if (!ValidateGroup(group)) return "Incorrect group format";
            List<User> entities = context.GetData();
            if (entities == null) { 
            entities = new List<User>();
            }
            if (entities.Find((x) => x.Id == id)!=null) return "user with such ID already exist";
            User entity = new User();
            entity.Id = id;
            entity.Firstname = firstname; 
            entity.Lastname = lastname;
            entity.Academicgroup = group;
            entities.Add(entity);
            context.SetData(entities);
            return "user added successfully";

              
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
            List<User> users = context.GetData();
            if (users == null)
            {
                users = new List<User>();
            }
            users.Sort((x, y) => x.Firstname.CompareTo(y.Firstname));
            foreach (User user in users) { 
                message+="First name= "+user.Firstname+"Lastname = "+user.Lastname +"\n";
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
            List<User> users = context.GetData();
            if (users == null)
            {
               return "you haven't added any user  yet";
            }
            foreach (User user in users) {
                if (user.Firstname.Contains(keyword)) {
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
            return message.Equals("")?"nothing was found":message;
        }
        public string getCurrentTime() {
            return Time.ToString("dd/MM/yyyy");
        }
        public string setCurrentTime(String str)
        {
            DateTime dt;
            DateTime.TryParse(str,out dt);
            if (dt.ToString("dd/MM/yyyy").Equals(str)) {
                Time = dt;
                return "Time changed";
            }
            return "Incorrect time format";
        }
        public void updateBookInfo() {
            List<Book> books = contextbook.GetData();
            List<User> users = context.GetData();
            if (users == null)
            {
                return;
            }
            if (books == null)
            {
                return;
            }
            foreach (User user in users)
            {
                foreach (int index in user.Shelf) {
                    if (Time.CompareTo(books.Find((x) => x.ID == index).DeadLine)==1)
                    {
                        userDeleteBookById(user.Id, index);
                    }
                }
            }

        }
        private void ValidateDate(String date)
        {
            Regex regex = new Regex(@"(0[1-9]|[12][0-9]|3[01])[-.](0[1-9]|1[012])[-.](19|20)\d\d$");

            if (!regex.IsMatch(date))
                throw new MyException(date);

        }
        private bool ValidatetName(String date)
        {
            Regex regex = new Regex(@"^[A-Z][a-z]{2,10}$");

            if (regex.IsMatch(date))
                return true;
            return false;

        }
        private bool ValidateGroup(String date)
        {
            Regex regex = new Regex(@"^[A-Z]{2}[1-9]{3}$");

            if (regex.IsMatch(date))
                return true;
            return false;

        }

    }
}
