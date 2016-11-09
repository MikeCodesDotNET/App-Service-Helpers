using System;
using NUnit.Framework;
using Test.Android.Models;


namespace Test.Android
{
    [TestFixture]
    public class TestsSample
    {

        [SetUp]
        public void Setup()
        {
            AppServiceHelpers.EasyMobileServiceClient.Current.Initialize("http://xamarin-todo-sample.azurewebsites.net");
            AppServiceHelpers.EasyMobileServiceClient.Current.RegisterTable<ToDo>();
            AppServiceHelpers.EasyMobileServiceClient.Current.FinalizeSchema();
        }


        [TearDown]
        public void Tear() { }

        [Test]
        public void RegisterTable()
        {
            Console.WriteLine("test1");

            var table = AppServiceHelpers.EasyMobileServiceClient.Current.Table<ToDo>();
            Assert.NotNull(table);
        }

        [Test]
        public void Fail()
        {
            Assert.False(true);
        }

        [Test]
        [Ignore("another time")]
        public void Ignore()
        {
            Assert.True(false);
        }

        [Test]
        public void Inconclusive()
        {
            Assert.Inconclusive("Inconclusive");
        }
    }
}