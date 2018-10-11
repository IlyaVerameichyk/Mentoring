using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace ExpressionTree.Tests
{
    public class ReplaceParametersVisitorTest
    {
        private readonly IDictionary<string, object> _dictionary;
        private readonly ReplaceParametersVisitor _visitor;

        public ReplaceParametersVisitorTest()
        {
            _dictionary = new Dictionary<string, object>()
            {
                { "i", 10 },
                { "a", "asd" },
                { "b", 10.0 },
                { "c", new object()},
            };

            _visitor = new ReplaceParametersVisitor(_dictionary);
        }

        [Fact]
        public void TestReplace_InsertExistingParams_MustReplaceWithConstants()
        {
            Expression<Action<int>> sourceExpression = i => Console.Write(i);
            Expression<Action> expectedExpression = () => Console.Write(10);

            var actualExpression = _visitor.Visit(sourceExpression);

            Assert.Equal(expectedExpression.ToString(), actualExpression.ToString());
        }

        [Fact]
        public void TestReplace_InsertMultipleExistingParams_MustReplaceAllWithConstants()
        {
            Expression<Action<int, string, double, object>> sourceExpression = (i, a, b, c) => PrintAll(i,a,b,c);
            Expression<Action> expectedExpression = () => PrintAll(10, "asd", 10.0, new object());

            var actualExpression = _visitor.Visit(sourceExpression);
            
            Assert.Equal(expectedExpression.ToString().Replace("new Object()", "value(System.Object)"), actualExpression.ToString());
        }

        [Fact]
        public void TestReplace_InsertNotExistingParams_MustNotReplaceParameters()
        {
            Expression<Func<string, string>> sourceExpression = s => s;
            
            var actualExpression = _visitor.Visit(sourceExpression);

            Assert.Equal(sourceExpression, actualExpression);
        }

        private void PrintAll(params object[] printObjects)
        {
            foreach (var printObject in printObjects)
            {
                Console.WriteLine(printObject);
            }
        }
    }
}
