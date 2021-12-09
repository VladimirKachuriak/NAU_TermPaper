using BLL;
using DAL;
using System;
using System.Collections.Generic;
using Xunit;

namespace BLLTEST
{
    public class Mock<T> : IDataProvider<T>
    {
        public static bool flag=false;
        public T Data { get; set; }
        public T read(string connection)
        {
            return Data;
        }

        public void write(T data, string connection)
        {
            flag = true;
            Data = data;
        }
    }
    public class UnitTest1
    {
        [Fact]
        public void ISaddUser_should_add_user_when_passing_correctdata()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());

            string result = service.addUser("Kiril", "Boiko", "BA125", 3);
            Assert.Equal("user added successfully", result);
   
        }
        [Fact]
        public void ISaddUser_should_add_user_when_passing_incorrectname()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());

            string result = service.addUser("kiril", "Boiko", "BA125", 3);
            Assert.Equal("Incorrect firstname format", result);
        }
        [Fact]
        public void ISaddUser_should_add_user_when_passing_incorrectgroup()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());

            string result = service.addUser("Kiril", "Boiko", "BA12f5", 3);
            Assert.Equal("Incorrect group format", result);
        }
        [Fact]
        public void ISaddUser_should_add_user_when_passing_sameId()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());
            service.addUser("Kiril", "Boiko", "BA125", 3);
            string result = service.addUser("Kiril", "Boiko", "BA125", 3);
            Assert.Equal("user with such ID already exist", result);
        }
        [Fact]
        public void ISaddBook_should_return_bookadded_whenpassing_correctdata()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());

            string result = service.addBook("Tom", "Java", "blabla", 5);
            Assert.Equal("book added", result);
            Assert.Equal("Incorrect author name format", service.addBook("tom", "Java", "blabla", 1));
            Assert.Equal("Incorrect title name format", service.addBook("Tom", "java", "blabla", 1));
            Assert.Equal("book with such ID already exist", service.addBook("Tom", "Java", "blabla", 5));
            
        }
        [Fact]
        public void ISaddBook_should_return_Incorrectauthornameformat_whenpassing_incorrectNameFormat()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());

            string result = service.addBook("tom", "Java", "blabla", 1);
            Assert.Equal("Incorrect author name format", result);

        }
        [Fact]
        public void ISaddBook_should_return_IncorrectTitlernameformat_whenpassing_incorrectTitleFormat()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());

            string result = service.addBook("Tom", "java", "blabla", 1);
            Assert.Equal("Incorrect title name format", result);

        }
        [Fact]
        public void ISaddBook_should_return_bookWithsuchIdalreadyexist_whenpassing_sameId()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());
            service.addBook("Tom", "Java", "blabla", 1);
            string result = service.addBook("Tom", "Java", "blabla", 1);
            Assert.Equal("book with such ID already exist", result);

        }

        [Fact]
        public void ISdeleteBook__should_return_bookwassuccessfullydeleted()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());
            service.addBook("Tom", "Java", "blabla", 4);
            string result = service.deleteBookById(4);
            Assert.Equal("book was successfully  deleted",result);

        }
        [Fact]
        public void ISdeleteUser__should_return_userwassuccessfullydeleted()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());
            service.addUser("Tom", "Lipnenko", "BB123", 3);
            string result = service.deleteUserById(3);
            Assert.Equal("user was successfully  deleted",result);

        }
        [Fact]
        public void ISTime__should_return_correctTime()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());
            service.setCurrentTime("09.10.2022");
         
            Assert.Equal("09.10.2022", service.getCurrentTime());

        }
        [Fact]
        public void ISbookAddedtoUser__should_return_bookaddedsuccessfully()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());
            service.addUser("Tom", "Lipnenko", "BB123", 3);
            service.addBook("Tomas","Apples","asfd",4);
            service.UserAddBookByID(3, 4);
            string result = service.userDeleteBookById(3,4);
            Assert.Equal("book deleted", result);

        }
        [Fact]
        public void ISchnageuserGroup__should_return_userchanged()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());
            service.addUser("Tom", "Lipnenko", "BB123", 3);
            string result = service.changeUserGroupByID(3,"BB123");
            Assert.Equal("Group of the user changed", result);

        }
        [Fact]
        public void ISchnageBookText__should_return_BookTextchanged()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());
            service.addBook("Tom", "Lipnenko", "Blabal", 3);
            string result = service.changeBookTextById(3, "BB123");
            Assert.Equal("Book changed", result);

        }
        [Fact]
        public void ISUserAddBookToUserByID__should_return_BookAdded()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());
            service.addUser("Tom", "Lipnenko", "BB123", 3);
            service.addBook("Tom", "Lipnenko", "Blabal", 2);
            string result = service.UserAddBookByID(3,2);
            Assert.Equal("book added to user successfully", result);

        }
        [Fact]
        public void ISUserDeleteBookToUserByID__should_return_BookDeleted()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());
            service.addUser("Tom", "Lipnenko", "BB123", 3);
            service.addBook("Tom", "Lipnenko", "Blabal", 2);
            service.UserAddBookByID(3, 2);
            string result = service.userDeleteBookById(3, 2);
            Assert.Equal("book deleted", result);

        }
    }
}