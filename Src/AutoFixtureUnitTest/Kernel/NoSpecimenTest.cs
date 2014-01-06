using System;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class NoSpecimenTest
    {
        [Fact]
        public void SutIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = NoSpecimen.Instance;
            // Verify outcome
            Assert.IsAssignableFrom<IEquatable<NoSpecimen>>(sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = NoSpecimen.Instance;
            object other = null;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualNullSut()
        {
            // Fixture setup
            var sut = NoSpecimen.Instance;
            NoSpecimen other = null;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Fixture setup
            var sut = NoSpecimen.Instance;
            var anonymousObject = new object();
            // Exercise system
            var result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.False(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWhenBothRequestsAreNull()
        {
            // Fixture setup
            var sut = NoSpecimen.Instance;
            object other = NoSpecimen.Instance;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWhenBothRequestsAreNull()
        {
            // Fixture setup
            var sut = NoSpecimen.Instance;
            var other = NoSpecimen.Instance;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherObjectWhenRequestsAreEqual()
        {
            // Fixture setup
            var request = new object();
            var sut = NoSpecimen.Instance;
            object other = NoSpecimen.Instance;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }

        [Fact]
        public void SutEqualsOtherSutWhenRequestsAreEqual()
        {
            // Fixture setup
            var request = new object();
            var sut = NoSpecimen.Instance;
            var other = NoSpecimen.Instance;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result, "Equals");
            // Teardown
        }
    }
}
