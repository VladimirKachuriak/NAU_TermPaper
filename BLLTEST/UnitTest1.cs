using BLL;
using DAL;
using System.Collections.Generic;
using Xunit;

namespace BLLTEST
{
    public class Mock<T> : IDataProvider<T>
    {
        public T Data { get; set; }
        public T read(string connection)
        {
            return Data;
        }

        public void write(T data, string connection)
        {
            Data = data;
        }
    }
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());

            Assert.Equal("user added successfully", service.addUser("Kiril","Boiko","BA125",3));
            Assert.Equal("Incorrect firstname format", service.addUser("kiril","Boiko","BA125",1));
            Assert.Equal("Incorrect lastname format", service.addUser("Kiril","boiko","BA125",2));
            Assert.Equal("user with such ID already exist", service.addUser("Kiril","Boiko","BA125",3));
        }
        [Fact]
        public void Test2()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());

            Assert.Equal("book added", service.addBook("Tom", "Java", "blabla", 5));
            Assert.Equal("Incorrect author name format", service.addBook("tom", "Java", "blabla", 1));
            Assert.Equal("Incorrect title name format", service.addBook("Tom", "java", "blabla", 1));
            Assert.Equal("book with such ID already exist", service.addBook("Tom", "Java", "blabla", 5));
            
        }
        [Fact]
        public void Test3()
        {
            EntityService service = new EntityService(new DataContext<List<User>>("myfile"), new Mock<List<User>>(), new DataContext<List<Book>>("books.xml"), new Mock<List<Book>>());
            service.addUser("Kiril", "Boiko", "BA125", 3);
            service.addUser("Boiko", "Kiril", "BA125", 4);
            service.deleteUserById(3);

            //Assert.Equal("",service.);

        }
    }
}