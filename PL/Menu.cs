using BLL;
using DAL;

namespace PL
{
    public class Menu
    {
        public Menu()
        {
            Console.WriteLine("WriteLine");
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"),new XMLProvider<List<User>>(), new DataContext<List<Book>>("books.xml"), new XMLProvider<List<Book>>());
            //service.setAllEntity("Mark","Kirilov","Ba21",1234);
          /*  service.addBook("AAAAA", "java", "blablab", 1);
            service.addBook("CCCCC", "java", "blablab", 2);
            service.addBook("BBBBB", "java", "blablab", 3);*/

            //Console.WriteLine(service.showBookSortByTitle());
            while (true)
            {
                int key;
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


                key = Convert.ToInt32(Console.ReadLine());

                switch (key)
                {

                    case 1:
                        {
                            string FirstName, LastName, Group, birth_date;
                            int ID;
                            Console.WriteLine("Enter FirstName");
                            FirstName = Console.ReadLine();
                            Console.WriteLine("Enter LastName");
                            LastName = Console.ReadLine();
                            Console.WriteLine("Enter your Id");
                            ID = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter group");
                            Group = Console.ReadLine();

                            service.addUser(FirstName, LastName, Group, ID);
                        }
                        break;
                    case 2:
                        {

                            Console.WriteLine("Enter  Id user which you wanna delete");
                            Console.WriteLine(service.deleteUserById(Convert.ToInt32(Console.ReadLine())));
                        }
                        break;
                    case 3:
                        {
                            Console.WriteLine("Choose sort method");
                            Console.WriteLine("1-sort by firstname");
                            Console.WriteLine("2-sort by lastname");
                            Console.WriteLine("2-sort by group");
                            key = Convert.ToInt32(Console.ReadLine());
                            switch (key)
                            {
                                case 1:
                                    Console.WriteLine(service.getAllUsers(EntityService.userEnum.FirstName));
                                    break;
                                case 2:
                                    Console.WriteLine(service.getAllUsers(EntityService.userEnum.Lastname));
                                    break;
                                case 3:
                                    Console.WriteLine(service.getAllUsers(EntityService.userEnum.Group));
                                    break;
                                default:
                                    Console.WriteLine("Incorrect command");
                                    break;
                            }
                        }
                        break;
                    case 5:
                        {
                            string author, title, data;
                            int ID;
                            Console.WriteLine("Enter author name");
                            author = Console.ReadLine();
                            Console.WriteLine("Enter title of the book");
                            title = Console.ReadLine();
                            Console.WriteLine("Enter description of the book");
                            data = Console.ReadLine();
                            Console.WriteLine("Enter unique book Id");
                            ID = Convert.ToInt32(Console.ReadLine());
                            service.addBook(author, title, data, ID);

                        }
                        break;
                    case 6:
                        {
                            Console.WriteLine("Enter id of the book which you wanna delete");
                            Console.WriteLine(service.deleteBookById(Convert.ToInt32(Console.ReadLine())));
                        }
                        break;

                    case 7:
                        {

                            Console.WriteLine("Choose sort method");
                            Console.WriteLine("1-sort by name");
                            Console.WriteLine("2-sort by author");
                            key = Convert.ToInt32(Console.ReadLine());
                            switch (key)
                            {
                                case 1:
                                    Console.WriteLine(service.showBook(EntityService.bookEnum.Name));
                                    break;
                                case 2:
                                    Console.WriteLine(service.showBook(EntityService.bookEnum.Author));
                                    break;
                                default: Console.WriteLine("Incorrect command");
                                    break;
                            }

                        }
                        break;
                    case 8:
                        {
                            Console.WriteLine("Enter ID of the book which you wanna find");
                            Console.WriteLine(service.findBookByID(Convert.ToInt32(Console.ReadLine())));
                        }
                        break;

                    case 10:
                        {
                            int userId, bookid;
                            Console.WriteLine("Enter ID of the user");
                            userId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter ID of the book which you wanna find");
                            bookid = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine(service.UserAddBookByID(userId, bookid));
                        }
                        break;
                    case 11:
                        {
                            int userId, bookid;
                            Console.WriteLine("Enter ID of the user");
                            userId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter ID of the book which you wanna delete");
                            bookid = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine(service.userDeleteBookById(userId, bookid));
                        }
                        break;
                    case 12:
                        {
                            Console.WriteLine("Enter ID of the user which you wanna find");
                            Console.WriteLine(service.showBooksOfUser(Convert.ToInt32(Console.ReadLine())));
                        }
                        break;
                    case 13:
                        {
                            Console.WriteLine("Enter keyword of the user which you wanna find");
                            Console.WriteLine(service.SearchUser(Console.ReadLine()));
                        }
                        break;
                    case 14:
                        {
                            Console.WriteLine("Enter keyword of the book which you wanna find");
                            Console.WriteLine(service.SearchBook(Console.ReadLine()));
                        }
                        break;

                }
            }

        }

    }
}