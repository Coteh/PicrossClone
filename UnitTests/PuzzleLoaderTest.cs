using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;
using PicrossClone;

namespace UnitTests {
    public class PuzzleLoaderTest : IUseFixture<PuzzleLoader> {
        PuzzleData puzzData;

        public void SetFixture(PuzzleLoader puzzleLoader) {
            puzzleLoader = new PuzzleLoader();
            puzzData = puzzleLoader.loadPuzzle(@"Content/Test Files/testlevel.pic");
        }

        [Fact]
        public void TestLoadPuzzleName() {
            Assert.Equal<string>("Banana", puzzData.name);
        }

        [Fact]
        public void TestLoadPuzzleData() {
            int[,] bananaPuzzle = new int[15, 15] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0 },
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0 },
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0 },
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1 },
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
                                                   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
                                                   { 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1 },
                                                   { 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 },
                                                   { 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1 },
                                                   { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
                                                   { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0 },
                                                   { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 } };
            Assert.Equal<int>(bananaPuzzle.GetLength(0), puzzData.puzzle.GetLength(0));
            Assert.Equal<int>(bananaPuzzle.GetLength(0), puzzData.puzzle.GetLength(1));
            for (int i = 0; i < puzzData.puzzle.GetLength(0); i++) {
                for (int j = 0; j < puzzData.puzzle.GetLength(1); j++) {
                    Assert.Equal<int>(bananaPuzzle[j, i], puzzData.puzzle[i, j]);
                }
            }
        }
    }
}
