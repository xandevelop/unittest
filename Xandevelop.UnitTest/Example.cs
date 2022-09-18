using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace Xandevelop.UnitTest
{
    public class Example
    {
        [Theory]
        [JsonDataFile("SampleTests.js", "tests")]
        public void test(TestCase tc)
        {
            Assert.Equal(1, tc.a);
        }
    }

    public class TestCase
    {
        [JsonProperty("a")]
        public int a { get; set; }
    }
}
