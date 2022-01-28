using System;
using System.Linq;
using aaHelpers.Collections;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;

namespace aaHelpers.Tests
{
    [TestFixture]
    public class aaListTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }
        
        [Test]
        public void New_Default_ShouldNotBeNull()
        {
            var list = new aaList();
            
            list.Should().NotBeNull();
        }
        
        [Test]
        public void New_Default_ShouldBeEmpty()
        {
            var list = new aaList();
            
            list.Should().BeEmpty();
        }
        
        [Test]
        public void New_ValidArray_ShouldNotBeNull()
        {
            var array = new object[] { 1, 2, 3, 4, 5 };
            
            var list = new aaList(array);
            
            list.Should().NotBeNull();
        }
        
        [Test]
        public void New_ValidArray_NotBeEmpty()
        {
            var array = _fixture.CreateMany<object>().ToArray();
            
            var list = new aaList(array);
            
            list.Should().NotBeEmpty();
        }

        [Test]
        public void FromCsv_ValidCsv_ShouldHaveExpectedCount()
        {
            var array = _fixture.CreateMany<string>().ToArray();
            var csv = string.Join(",", array);

            var list = aaList.FromCsv(csv);

            list.Should().HaveCount(array.Length);
        }

        [Test]
        public void Join_WhenCalled_ShouldBeExpected()
        {
            var array = _fixture.CreateMany<string>().ToArray();
            var list = new  aaList(array);

            var value = list.Join();

            value.Should().NotBeNull();
            value.Should().Contain(",");
        }

        [Test]
        public void Where_ValidCondition_ShouldBeExpected()
        {
            var array = _fixture.CreateMany<object>().ToArray();
            var first = array.First();

            var list = new aaList(array);

            var items = list.Where(first);

            items.Should().HaveCount(1);
        }
        
        [Test]
        public void Where_Contains_ShouldBeExpected()
        {
            var array = _fixture.CreateMany<string>().ToArray();
            var first = array.First();

            var list = new aaList(array);

            var items = list.Where(first, aaListMatchOption.Contains);

            items.Should().HaveCount(1);
        }

        [Test]
        public void IsHomogenous_DifferentTypes_ShouldBeFalse()
        {
            var array = new object[] { 1, "Value", 1.45, DateTime.Now };

            var list = new aaList(array);

            list.IsHomogenous().Should().BeFalse();
        }
        
        [Test]
        public void IsHomogenous_SameTypes_ShouldBeTure()
        {
            var array = _fixture.CreateMany<object>().ToArray();

            var list = new aaList(array);

            list.IsHomogenous().Should().BeTrue();
        }
    }
}