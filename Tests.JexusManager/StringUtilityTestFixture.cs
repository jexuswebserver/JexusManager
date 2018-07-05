using JexusManager;
using Xunit;

namespace Tests
{
    public class StringUtilityTestFixture
    {
        [Fact]
        public void TestIsWildcard()
        {
            Assert.True("*.test.com".IsWildcard());
            Assert.True("*".IsWildcard());
            
            Assert.False("test.*.com".IsWildcard());
            Assert.False("*.*.com".IsWildcard());
        }

        [Fact]
        public void TestIsValidHost()
        {
            Assert.True("*.test.com".IsValidHost(true));
            Assert.True("*".IsValidHost(true));
            
            Assert.False("*.test.com".IsValidHost());
            Assert.False("*".IsValidHost());
            
            Assert.False("test.*.com".IsValidHost());
            Assert.False("*.*.com".IsValidHost());
        }

        [Fact]
        public void TestMatchHostName()
        {
            Assert.True("*.test.com".MatchHostName("www.test.com"));
            Assert.True("*.test.com".MatchHostName("*.test.com"));
            Assert.True("localhost".MatchHostName("localhost"));

            Assert.False("*.test.com".MatchHostName("test.com"));
            Assert.False("www.test.com".MatchHostName("*.test.com"));
        }
    }
}
