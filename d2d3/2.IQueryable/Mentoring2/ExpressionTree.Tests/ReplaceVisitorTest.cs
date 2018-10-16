using System;
using System.Linq.Expressions;
using Xunit;

namespace ExpressionTree.Tests
{
    public class ReplaceVisitorTest
    {
        private readonly int[] testParameters = {-1, 0, 1, 5};
        private readonly ParameterExpression _intParameter = Expression.Parameter(typeof(int), "i");

        [Fact]
        public void TestIncrement_Add1Before_MustReplaceWithIncrement()
        {
            Expression<Func<int, int>> sourceExpression = i => 1 + i;
            var expectedExpression = Expression.Lambda(typeof(Func<int, int>), Expression.Increment(_intParameter), _intParameter);

            var actualExpression = new ReplaceVisitor().VisitAndConvert(sourceExpression, string.Empty);

            Assert.Equal(expectedExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            AssertExpressionsResults((Func<int,int>)expectedExpression.Compile(), (Func<int, int>)actualExpression.Compile(), testParameters);
        }

        [Fact]
        public void TestIncrement_Add1After_MustReplaceWithIncrement()
        {
            Expression<Func<int, int>> sourceExpression = i => i + 1;
            var expectedExpression = Expression.Lambda(typeof(Func<int, int>), Expression.Increment(_intParameter), _intParameter);

            var actualExpression = new ReplaceVisitor().VisitAndConvert(sourceExpression, string.Empty);

            Assert.Equal(expectedExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            AssertExpressionsResults((Func<int, int>)expectedExpression.Compile(), (Func<int, int>)actualExpression.Compile(), testParameters);
        }

        [Fact]
        public void TestIncrement_Add2_MustNotReplaceWithIncrement()
        {
            Expression<Func<int, int>> sourceExpression = i => i + 2;
            var expectedExpression = Expression.Lambda(typeof(Func<int, int>), Expression.Increment(_intParameter), _intParameter);

            var actualExpression = new ReplaceVisitor().VisitAndConvert(sourceExpression, string.Empty);

            Assert.NotEqual(expectedExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            Assert.Equal(sourceExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            AssertExpressionsResults((Func<int, int>)sourceExpression.Compile(), (Func<int, int>)actualExpression.Compile(), testParameters);
        }

        [Fact]
        public void TestDecrement_SubtractFrom1_MustNotReplaceWithDecrement()
        {
            Expression<Func<int, int>> sourceExpression = i => 1 - i;
            var expectedExpression = Expression.Lambda(typeof(Func<int, int>), Expression.Decrement(_intParameter), _intParameter);

            var actualExpression = new ReplaceVisitor().VisitAndConvert(sourceExpression, string.Empty);

            Assert.NotEqual(expectedExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            Assert.Equal(sourceExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            AssertExpressionsResults((Func<int, int>)sourceExpression.Compile(), (Func<int, int>)actualExpression.Compile(), testParameters);
        }

        [Fact]
        public void TestDecrement_SubtractThe1_MustReplaceWithDecrement()
        {
            Expression<Func<int, int>> sourceExpression = i => i - 1;
            var expectedExpression = Expression.Lambda(typeof(Func<int, int>), Expression.Decrement(_intParameter), _intParameter);

            var actualExpression = new ReplaceVisitor().VisitAndConvert(sourceExpression, string.Empty);

            Assert.Equal(expectedExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            AssertExpressionsResults((Func<int, int>)expectedExpression.Compile(), (Func<int, int>)actualExpression.Compile(), testParameters);
        }

        [Fact]
        public void TestDecrement_SubtractThe1_MustNotReplaceWithDecrement()
        {
            Expression<Func<int, int>> sourceExpression = i => i - 2;
            var notExpectedExpression = Expression.Lambda(typeof(Func<int, int>), Expression.Increment(_intParameter), _intParameter);

            var actualExpression = new ReplaceVisitor().VisitAndConvert(sourceExpression, string.Empty);

            Assert.NotEqual(notExpectedExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            Assert.Equal(sourceExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            AssertExpressionsResults((Func<int, int>)sourceExpression.Compile(), (Func<int, int>)actualExpression.Compile(), testParameters);
        }

        [Fact]
        public void TestDecrement_Subtract1From1_MustReplaceWithDecrement()
        {
            Expression<Func<int, int>> sourceExpression = i => ((i - 2) + (i - 1)) + ((1 + i) - 1);
            var expectedExpression = Expression.Lambda(typeof(Func<int, int>),
                Expression.Add(
                    Expression.Add(
                        Expression.Subtract(_intParameter, Expression.Constant(2)), 
                        Expression.Decrement(_intParameter)
                        ),
                    Expression.Decrement(
                        Expression.Increment(_intParameter)
                        )
                ),
                _intParameter);

            var actualExpression = new ReplaceVisitor().VisitAndConvert(sourceExpression, string.Empty);
            
            Assert.NotEqual(sourceExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            Assert.Equal(expectedExpression.ToString(), actualExpression.ToString()); //replace with real comparison
            AssertExpressionsResults((Func<int, int>)expectedExpression.Compile(), (Func<int, int>)actualExpression.Compile(), testParameters);
        }

        private void AssertExpressionsResults(Func<int, int>expectedExpression,
            Func<int, int> actualExpression, params int[] parameters)
        {
            foreach (var i in parameters)
            {
                var expected = expectedExpression(i);
                var actual = actualExpression(i);
                Assert.Equal(expected, actual);
            }
        }
    }
}
