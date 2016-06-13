using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;
using GameClasses;
using System.IO;

namespace UnitTests {
    public class LineOpenerTest {
        [Fact]
        public void TestLoadAllLines() {
            LineOpener lo = new ConcreteLineOpener();
            string[] result = new string[3];
            result = lo.loadAllLines(@"Content/Test Files/testfile.txt");
            Assert.Equal<int>(3, result.Length);
            Assert.Equal<string>("all of", result[0]);
            Assert.Equal<string>("these lines", result[1]);
            Assert.Equal<string>("should load", result[2]);
        }
    }
}
