using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;

namespace PicrossClone {
    public class PuzzleSaver {
        LineSaver ls;
        string titleDecorator = "--";

        public PuzzleSaver() {
            ls = new ConcreteLineSaver();
        }

        public void savePuzzle(PuzzleData _puzzleData, string _filePath) {
            int puzzleHeight = _puzzleData.puzzle.GetLength(1), puzzleWidth = _puzzleData.puzzle.GetLength(0);
            string[] stuffToSave = new string[puzzleHeight + 1]; //creating string array big enough to hold board plus title string
            stuffToSave[0] = titleDecorator + _puzzleData.name + titleDecorator;
            for (int i = 0; i < puzzleHeight; i++){
                for (int j = 0; j < puzzleWidth; j++) {
                    stuffToSave[1 + i] += _puzzleData.puzzle[j,i];
                    if (j < puzzleWidth) stuffToSave[1 + i] += " ";
                }
            }
            ls.saveAllLines(_filePath, stuffToSave);
        }
    }
}
