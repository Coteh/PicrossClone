using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;
using GameClasses;
using Microsoft.Xna.Framework;

namespace UnitTests {
    public class CursorTest {
        [Fact]
        public void TestCursorInitPosition() {
            Vector2 testPos = new Vector2(2, 2);
            Cursor cursor = new Cursor(testPos);
            Assert.Equal<Vector2>(testPos, cursor.Position);
        }

        [Fact]
        public void TestCursorSetPosition() {
            Vector2 expectedPosition = new Vector2(9, 3);
            Cursor cursor = new Cursor(Vector2.Zero);
            
            cursor.setCursorPoints(expectedPosition);
            Assert.Equal(expectedPosition, cursor.Position);
        }
    }
}
