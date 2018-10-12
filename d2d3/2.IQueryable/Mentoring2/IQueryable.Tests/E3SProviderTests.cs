using System;
using Sample03.E3SClient.Entities;
using Sample03.E3SClient;
using System.Linq;
using FakeItEasy;
using IQueryable;
using Xunit;

namespace Sample03
{
    public class E3SProviderTests
    {
        private IHttpClient _fakeClient;

        public E3SProviderTests()
        {
            _fakeClient = A.Fake<IHttpClient>();

            A.CallTo(() => _fakeClient.GetStringAsync(A<Uri>.Ignored)).Returns("{items:[{data:null}]}");
        }


        [Fact]
        public void WhereBaseTest_AssertParameter()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(_fakeClient);

            foreach (var emp in employees.Where(e => e.workstation == "EPRUIZHW0249"))
            {
                Console.WriteLine("{0}", emp);
            }
            var expectedString =
                @"https://e3s.epam.com/eco/rest/e3s-eco-scripting-impl/0.1.0/data/searchFts?" + @"metaType=meta:people-suite:people-api:com.epam.e3s.app.people.api.data.EmployeeEntity&" +
                @"query={""statements"":[{""query"":""workstation:(EPRUIZHW0249)""}]%2c""filters"":null%2c""sorting"":null%2c""start"":0%2c""limit"":0}";
            A.CallTo(() => _fakeClient.GetStringAsync(A<Uri>.That.Matches(uri => uri.ToString() == expectedString))).MustHaveHappened();
        }

        [Fact]
        public void WhereRevertTest_AssertParameter()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(_fakeClient);

            foreach (var emp in employees.Where(e => "EPRUIZHW0249" == e.workstation))
            {
                Console.WriteLine("{0}", emp);
            }
            var expectedString =
                @"https://e3s.epam.com/eco/rest/e3s-eco-scripting-impl/0.1.0/data/searchFts?" + @"metaType=meta:people-suite:people-api:com.epam.e3s.app.people.api.data.EmployeeEntity&" +
                @"query={""statements"":[{""query"":""workstation:(EPRUIZHW0249)""}]%2c""filters"":null%2c""sorting"":null%2c""start"":0%2c""limit"":0}";
            A.CallTo(() => _fakeClient.GetStringAsync(A<Uri>.That.Matches(uri => uri.ToString() == expectedString))).MustHaveHappened();
        }

        [Fact]
        public void WhereStartWithtTest_AssertParameter()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(_fakeClient);

            foreach (var emp in employees.Where(e => e.workstation.StartsWith("EPRUIZHW0249")))
            {
                Console.WriteLine("{0}", emp);
            }
            var expectedString =
                @"https://e3s.epam.com/eco/rest/e3s-eco-scripting-impl/0.1.0/data/searchFts?" + @"metaType=meta:people-suite:people-api:com.epam.e3s.app.people.api.data.EmployeeEntity&" +
                @"query={""statements"":[{""query"":""workstation:(EPRUIZHW0249*)""}]%2c""filters"":null%2c""sorting"":null%2c""start"":0%2c""limit"":0}";
            A.CallTo(() => _fakeClient.GetStringAsync(A<Uri>.That.Matches(uri => uri.ToString() == expectedString))).MustHaveHappened();
        }

        [Fact]
        public void WhereEndsWithtTest_AssertParameter()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(_fakeClient);

            foreach (var emp in employees.Where(e => e.workstation.EndsWith("EPRUIZHW0249")))
            {
                Console.WriteLine("{0}", emp);
            }
            var expectedString =
                @"https://e3s.epam.com/eco/rest/e3s-eco-scripting-impl/0.1.0/data/searchFts?" + @"metaType=meta:people-suite:people-api:com.epam.e3s.app.people.api.data.EmployeeEntity&" +
                @"query={""statements"":[{""query"":""workstation:(*EPRUIZHW0249)""}]%2c""filters"":null%2c""sorting"":null%2c""start"":0%2c""limit"":0}";
            A.CallTo(() => _fakeClient.GetStringAsync(A<Uri>.That.Matches(uri => uri.ToString() == expectedString))).MustHaveHappened();
        }

        [Fact]
        public void WhereContainsTest_AssertParameter()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(_fakeClient);

            foreach (var emp in employees.Where(e => e.workstation.Contains("EPRUIZHW0249")))
            {
                Console.WriteLine("{0}", emp);
            }
            var expectedString =
                @"https://e3s.epam.com/eco/rest/e3s-eco-scripting-impl/0.1.0/data/searchFts?" + @"metaType=meta:people-suite:people-api:com.epam.e3s.app.people.api.data.EmployeeEntity&" +
                @"query={""statements"":[{""query"":""workstation:(*EPRUIZHW0249*)""}]%2c""filters"":null%2c""sorting"":null%2c""start"":0%2c""limit"":0}";
            A.CallTo(() => _fakeClient.GetStringAsync(A<Uri>.That.Matches(uri => uri.ToString() == expectedString))).MustHaveHappened();
        }

        [Fact]
        public void AndTest_AssertParameter()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(_fakeClient);

            foreach (var emp in employees.Where(e => e.workstation.Contains("EPRUIZHW0249") && e.workstation.StartsWith("E49")))
            {
                Console.WriteLine("{0}", emp);
            }
            var expectedString =
                @"https://e3s.epam.com/eco/rest/e3s-eco-scripting-impl/0.1.0/data/searchFts?" + @"metaType=meta:people-suite:people-api:com.epam.e3s.app.people.api.data.EmployeeEntity&" +
                @"query={""statements"":[{""query"":""workstation:(E49*)""}%2c{""query"":""workstation:(*EPRUIZHW0249*)""}]%2c""filters"":null%2c""sorting"":null%2c""start"":0%2c""limit"":0}";
            A.CallTo(() => _fakeClient.GetStringAsync(A<Uri>.That.Matches(uri => uri.ToString() == expectedString))).MustHaveHappened();
        }

        [Fact]
        public void AndTest_PassThreeParameters_AssertParameter()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(_fakeClient);

            foreach (var emp in employees.Where(e => e.workstation.Contains("EPRUIZHW0249") && e.workstation.StartsWith("E49") && e.workstation.EndsWith("EQW11")))
            {
                Console.WriteLine("{0}", emp);
            }
            var expectedString =
                @"https://e3s.epam.com/eco/rest/e3s-eco-scripting-impl/0.1.0/data/searchFts?" + @"metaType=meta:people-suite:people-api:com.epam.e3s.app.people.api.data.EmployeeEntity&" +
                @"query={""statements"":[{""query"":""workstation:(*EQW11)""}%2c{""query"":""workstation:(*EPRUIZHW0249*)""}%2c{""query"":""workstation:(E49*)""}]%2c""filters"":null%2c""sorting"":null%2c""start"":0%2c""limit"":0}";
            A.CallTo(() => _fakeClient.GetStringAsync(A<Uri>.That.Matches(uri => uri.ToString() == expectedString))).MustHaveHappened();
        }
    }
}
