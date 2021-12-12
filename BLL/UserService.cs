using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class UserService
    {
        IDataContext<List<User>> contextuser;
        IDataContext<List<Book>> contextbook;
        DateTime Time;
        delegate int UserOperation(User a, User b);
        public enum userEnum
        {
            FirstName,
            Lastname,
            Group,

        }
        public UserService(IDataContext<List<User>> context, IDataProvider<List<User>> userProvider,
                             IDataContext<List<Book>> bookcontext, IDataProvider<List<Book>> bookProvider)
        {
            this.contextuser = context;
            context.DataProvider = userProvider;
            contextbook = bookcontext;
            contextbook.DataProvider = bookProvider;
            contextuser.ConnectionString = "myfile";
            contextbook.ConnectionString = "book.xml";

            Time = DateTime.Now;

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
        private bool ValidatetName(String date)
        {
            Regex regex = new Regex(@"^[A-Z][a-z]{2,10}$");

            if (regex.IsMatch(date))
                return true;
            return false;

        }
        private void ValidateGroup(String date)
        {
            Regex regex = new Regex(@"^[A-Z]{2}[1-9]{3}$");

            if (!regex.IsMatch(date)) throw new MyException("Incorrect group format");


        }
        public string changeUserGroupByID(int id, string text)
        {
            string message = "";
            try
            {
                ValidateGroup(text);
            }
            catch (MyException ex)
            {
                return ex.Message;
            }
            List<User> users = contextuser.GetData();
            if (users == null)
            {
                return "you haven't added any user  yet";
            }
            foreach (User user in users)
            {
                if (user.Id == id)
                {
                    user.Academicgroup = text;
                    contextuser.SetData(users);
                    return "Group of the user changed";
                }
            }
            return "User doesn't exist";

        }

        public string UserAddBookByID(int userId, int bookID)
        {
            List<Book> books = contextbook.GetData();
            List<User> users = contextuser.GetData();
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
                                    if (books.Find((x) => x.ID == bookID) != null)
                                    {
                                        if (books.Find((x) => x.ID == bookID).Exist_status == true)
                                        {
                                            book.Exist_status = false;
                                            book.DeadLine = DateTime.Now.AddDays(3);
                                            user.Shelf.Add(book.ID);
                                            contextuser.SetData(users);
                                            contextbook.SetData(books);
                                            return "book added to user successfully";
                                        }
                                        return "This book already got another student";
                                    }
                                    return "there is no such book in the library ";
                                }
                                return "book already in your shelf";
                            }
                        }
                        return "No book with such id";
                    }
                    return "You can't add more than 4 books";
                }
            }
            return "The user with this ID doesn't exist";

        }

        public string showBooksOfUser(int userId)
        {
            string message = "";
            List<Book> books = contextbook.GetData();
            List<User> users = contextuser.GetData();
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
                    return message.Equals("") ? "You don't have any books yet" : message;
                }
            }
            return "The user with this ID doesn't exist";
        }
        public string addUser(string firstname, string lastname, string group, int id)
        {

            if (!ValidatetName(firstname)) return "Incorrect firstname format";
            if (!ValidatetName(lastname)) return "Incorrect lastname format";
            try
            {
                ValidateGroup(group);
            }
            catch (MyException ex)
            {
                return ex.Message;
            }
            List<User> entities = contextuser.GetData();
            if (entities == null)
            {
                entities = new List<User>();
            }
            if (entities.Find((x) => x.Id == id) != null) return "user with such ID already exist";
            User entity = new User();
            entity.Id = id;
            entity.Firstname = firstname;
            entity.Lastname = lastname;
            entity.Academicgroup = group;
            entity.Shelf = new List<int>();
            entities.Add(entity);
            contextuser.SetData(entities);
            return "user added successfully";


        }
        public string getAllUsers(userEnum op)
        {
            List<User> entities = contextuser.GetData();
            if (entities == null)
            {
                return "you haven't added any user  yet";
            }
            string message = "";
            entities.Sort(getUserOperation(op).Invoke);
            foreach (User entity in entities)
            {
                message += "Firstname: " + entity.Firstname + "\t Lastname: " + entity.Lastname + "\t Group: " + entity.Academicgroup + "\t ID: " + entity.Id;
                message += "\n";
            }
            return message;

        }
        public string userDeleteBookById(int userid, int bookid)
        {
            List<Book> books = contextbook.GetData();
            List<User> users = contextuser.GetData();
            if (users.Find((x) => x.Id == userid) != null)
            {
                if (users.Find((x) => x.Id == userid).Shelf.Find((x) => x == bookid) == bookid)
                {

                    users.Find((x) => x.Id == userid).Shelf.Remove(users.Find((x) => x.Id == userid).Shelf.Find((x) => x == bookid));
                    books.Find((x) => x.ID == bookid).Exist_status = true;
                    contextuser.SetData(users);
                    contextbook.SetData(books);
                    return "book deleted";
                }
                return "book with correspond id wasn't faund";
            }


            return "user wasn't find";

        }


        public string deleteUserById(int id)
        {
            List<User> users = contextuser.GetData();

            if (users == null)
            {
                return "you haven't added any user  yet";
            }
            if (users.FindAll((x) => x.Id == id).Count > 0)
            {
                contextuser.SetData(users.FindAll((x) => x.Id != id));
                return "user was successfully  deleted";
            }

            return "a user with this id does not exist";
        }
        public string getCurrentTime()
        {
            return Time.ToString("dd/MM/yyyy");
        }
        public string setCurrentTime(String str)
        {
            DateTime dt;
            DateTime.TryParse(str, out dt);
            if (dt.ToString("dd/MM/yyyy").Equals(str))
            {
                Time = dt;
                return "Time changed";
            }
            return "Incorrect time format";
        }
        public void updateBookInfo()
        {
            List<Book> books = contextbook.GetData();
            List<User> users = contextuser.GetData();
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
                foreach (int index in user.Shelf)
                {
                    if (Time.CompareTo(books.Find((x) => x.ID == index).DeadLine) == 1)
                    {
                        userDeleteBookById(user.Id, index);
                    }
                }
            }

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

    }
}
