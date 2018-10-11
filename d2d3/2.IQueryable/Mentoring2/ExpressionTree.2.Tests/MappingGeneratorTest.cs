using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ExpressionTree._2.Tests.Models;
using Xunit;

namespace ExpressionTree._2.Tests
{
    public class MappingGeneratorTest
    {
        [Fact]
        public void TestMapping_MustMapObject_AssertProperties()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Source, Destination>();

            var expected = new Source()
            {
                ArrayField = new[] { 1.1, 2.2, 3.3 },
                IntField = 42,
                ListField = new List<int>() { 1, 2, 3 },
                SourceSpecificField = -1,
                StringField = "string"
            };

            var actual = mapper.Map(expected);

            Assert.True(expected.ArrayField.SequenceEqual(expected.ArrayField));
            //Assert.NotSame(expected.ArrayField, actual.ArrayField);
            Assert.Equal(expected.IntField, actual.IntField);
            Assert.Same(expected.ListField, actual.ListField);
            Assert.Equal(expected.StringField, actual.StringField);

            Assert.NotEqual(expected.SourceSpecificField, actual.DestinationSpecificField);
        }
    }
}
