using BLL;
using DAL;

namespace PL
{
    public class Menu
    {
        public Menu()
        {      
            //EntityService service = new EntityService(new DataContext<List<User>>(),new XMLProvider<List<User>>(), new DataContext<List<Book>>(), new XMLProvider<List<Book>>());
            BookService bookService = new BookService(new DataContext<List<User>>(),new XMLProvider<List<User>>(), new DataContext<List<Book>>(), new XMLProvider<List<Book>>());
            UserService userService = new UserService(new DataContext<List<User>>(),new XMLProvider<List<User>>(), new DataContext<List<Book>>(), new XMLProvider<List<Book>>());
            while (true)
            {
                int key;
                showOptions();



                key = getInteger("Enter option from list");

                switch (key)
                {

                    case 1:
                        {        
                            Console.WriteLine(userService.addUser(getString("Enter FirstName"), getString("Enter LastName"), getString("Enter group"), getInteger("Enter your Id")));
                        }
                        break;
                    case 2:
                        {

                            Console.WriteLine(userService.deleteUserById(getInteger("Enter  Id user which you wanna delete")));
                        }
                        break;
                    case 3:
                        {
                            Console.WriteLine("Choose sort method");
                            Console.WriteLine("1-sort by firstname");
                            Console.WriteLine("2-sort by lastname");
                            Console.WriteLine("3-sort by group");
                            key = getInteger("Enter code of the option");
                            switch (key)
                            {
                                case 1:
                                    Console.WriteLine(userService.getAllUsers(UserService.userEnum.FirstName));
                                    break;
                                case 2:
                                    Console.WriteLine(userService.getAllUsers(UserService.userEnum.Lastname));
                                    break;
                                case 3:
                                    Console.WriteLine(userService.getAllUsers(UserService.userEnum.Group));
                                    break;
                                default:
                                    Console.WriteLine("Incorrect command");
                                    break;
                            }
                        }
                        break;
                    case 5:
                        {
                            Console.WriteLine(bookService.addBook(getString("Enter author name"), getString("Enter title of the book"), getString("Enter description of the book"), getInteger("Enter unique book Id")));

                        }
                        break;
                    case 6:
                        {
                           Console.WriteLine(bookService.deleteBookById(getInteger("Enter id of the book which you wanna delete"),userService));
                        }
                        break;

                    case 7:
                        {
                            Console.WriteLine("Choose sort method");
                            Console.WriteLine("1-sort by title");
                            Console.WriteLine("2-sort by author");
                            switch (getInteger("Enter number of the option"))
                            {
                                case 1:
                                    Console.WriteLine(bookService.showBook(BookService.bookEnum.Title));
                                    break;
                                case 2:
                                    Console.WriteLine(bookService.showBook(BookService.bookEnum.Author));
                                    break;
                                default: Console.WriteLine("Incorrect command");
                                    break;
                            }

                        }
                        break;
                    case 8:
                        {
                            Console.WriteLine(bookService.findBookByID(getInteger("Enter ID of the book which you wanna find")));
                        }
                        break;
                    case 9:
                        {
                            Console.WriteLine(bookService.showDataOfBookByID(getInteger("Enter ID of the book which you wanna find")));
                        }
                        break;

                    case 10:
                        {
                            Console.WriteLine(userService.UserAddBookByID(getInteger("Enter ID of the user"), getInteger("Enter ID of the book which you wanna find")));
                        }
                        break;
                    case 11:
                        {
                            Console.WriteLine(userService.userDeleteBookById(getInteger("Enter ID of the user"), getInteger("Enter ID of the book which you wanna find")));
                        }
                        break;
                    case 12:
                        {
                            Console.WriteLine(userService.showBooksOfUser(getInteger("Enter ID of the user which you wanna find")));
                        }
                        break;
                    case 13:
                        {
                            Console.WriteLine(userService.SearchUser(getString("Enter keyword of the user which you wanna find")));
                        }
                        break;
                    case 14:
                        {
                            Console.WriteLine("Enter keyword of the book which you wanna find");
                            Console.WriteLine(bookService.SearchBook(getString("Enter keyword of the book which you wanna find")));
                        }
                        break;
                    case 15:
                        {   
                            Console.WriteLine(userService.getCurrentTime());
                        }
                        break;
                    case 16:
                        {
                            Console.WriteLine(userService.setCurrentTime(getString("Enter time which you wanna set")));
                        }
                        break;
                    case 17:
                        {
                            userService.updateBookInfo();
                        }
                        break;
                    case 18:
                        {
                            Console.WriteLine(bookService.changeBookTextById(getInteger("Enter ID of the book"), getString("Enter text of the book")));
                        }
                        break;
                    case 19:
                        {
                            Console.WriteLine(userService.changeUserGroupByID(getInteger("Enter ID of the user"),getString("Enter group of the user")));
                        }
                        break;
                }
            }

        }
        private int getInteger(string message) { 
            Console.WriteLine(message);
            while (true)
            {
                try
                {
                   return Convert.ToInt32(Console.ReadLine());
                    
                }
                catch (Exception e) { 
                    Console.WriteLine("That's not a number. Try again");
                }
               
            }
        }
        private string getString(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }
        private void showOptions() {
            Console.WriteLine("1 - add user");
            Console.WriteLine("2 - delete user by id");
            Console.WriteLine("3 - show all users");
            Console.WriteLine("4 - change user by id");
            Console.WriteLine("5 - add book to library");
            Console.WriteLine("6 - delete book by id");
            Console.WriteLine("7 - show all books");
            Console.WriteLine("8 - find book by id");
            Console.WriteLine("9 - show book text by id");
            Console.WriteLine("10 - add book to the user");
            Console.WriteLine("11 - delete book from user");
            Console.WriteLine("12 - show books of the user");
            Console.WriteLine("13 - search users by the key word");
            Console.WriteLine("14 - search books by the key word");
            Console.WriteLine("15 - show time");
            Console.WriteLine("16 - change time");
            Console.WriteLine("17 - update user shelf");
            Console.WriteLine("18 - change text of the book by ID");
            Console.WriteLine("19 - change group of the user by ID");
        }

    }
}