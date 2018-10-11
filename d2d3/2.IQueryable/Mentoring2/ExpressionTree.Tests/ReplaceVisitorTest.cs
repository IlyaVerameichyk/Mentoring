using System;
using System.Linq.Expressions;
using Xunit;

namespace ExpressionTree.Tests
{
    public class ReplaceVisitorTest
    {
        [Fact]
        public void TestIncrement_Add1Before_MustReplaceWithIncrement()
        {
            Expression<Func<int, int>> sourceExpression = i => 1 + i;
            var expectedExpression = Expression.Lambda(typeof(Func<int, int>), Expression.Increment(Expression.Parameter(typeof(int), "i")), Expression.Parameter(typeof(int), "i"));

            var actualExpression = new ReplaceVisitor().VisitAndConvert(sourceExpression, string.Empty);

            Assert.Equal(expectedExpression.ToString(), actualExpression.ToString()); //replace with real comparison
        }

        [Fact]
        public void TestIncrement_Add1After_MustReplaceWithIncrement()
        {
            Expression<Func<int, int>> sourceExpression = i => i + 1;
            var expectedExpression = Expression.Lambda(typeof(Func<int, int>), Expression.Increment(Expression.Parameter(typeof(int), "i")), Expression.Parameter(typeof(int), "i"));

            var actualExpression = new ReplaceVisitor().VisitAndConvert(sourceExpression, string.Empty);

            Assert.Equal(expectedExpression.ToString(), actualExpression.ToString()); //replace with real comparison
        }

        [Fact]
        public void TestIncrement_Add2_MustNotReplaceWithIncrement()
        {
            Expression<Func<int, int>> sourceExpression = i => i + 2;
            var expectedExpression = Expression.Lambda(typeof(Func<int, int>), Expression.Increment(Expression.Parameter(typeof(int), "i")), Expression.Parameter(typeof(int), "i"));

            var actualExpression = new ReplaceVisitor().VisitAndConvert(sourceExpression, string.Empty);

            Assert.NotEqual(expectedExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            Assert.Equal(sourceExpression.ToString(), actualExpression.ToString()); //replace with real comparison
        }

        [Fact]
        public void TestDecrement_SubtractFrom1_MustNotReplaceWithDecrement()
        {
            Expression<Func<int, int>> sourceExpression = i => 1 - i;
            var expectedExpression = Expression.Lambda(typeof(Func<int, int>), Expression.Decrement(Expression.Parameter(typeof(int), "i")), Expression.Parameter(typeof(int), "i"));

            var actualExpression = new ReplaceVisitor().VisitAndConvert(sourceExpression, string.Empty);

            Assert.NotEqual(expectedExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            Assert.Equal(sourceExpression.ToString(), actualExpression.ToString()); //replace with real comparison
        }

        [Fact]
        public void TestDecrement_SubtractThe1_MustReplaceWithDecrement()
        {
            Expression<Func<int, int>> sourceExpression = i => i - 1;
            var expectedExpression = Expression.Lambda(typeof(Func<int, int>), Expression.Decrement(Expression.Parameter(typeof(int), "i")), Expression.Parameter(typeof(int), "i"));

            var actualExpression = new ReplaceVisitor().VisitAndConvert(sourceExpression, string.Empty);

            Assert.Equal(expectedExpression.ToString(), actualExpression.ToString()); //replace with real comparison
        }

        [Fact]
        public void TestDecrement_SubtractThe1_MustNotReplaceWithDecrement()
        {
            Expression<Func<int, int>> sourceExpression = i => i - 2;
            var expectedExpression = Expression.Lambda(typeof(Func<int, int>), Expression.Increment(Expression.Parameter(typeof(int), "i")), Expression.Parameter(typeof(int), "i"));

            var actualExpression = new ReplaceVisitor().VisitAndConvert(sourceExpression, string.Empty);

            Assert.NotEqual(expectedExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            Assert.Equal(sourceExpression.ToString(), actualExpression.ToString()); //replace with real comparison
        }


    }
}
