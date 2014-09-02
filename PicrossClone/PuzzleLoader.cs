using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClasses;

namespace PicrossClone {
    public class PuzzleLoader {
        private LineOpener lo;
        private const int BOARD_START_INDEX = 1; //will start reading the board at this line

        public PuzzleLoader() {
            lo = new ConcreteLineOpener();
        }

        public PuzzleData loadPuzzle(string _filePath) {
            //Creating a PuzzleData object
            PuzzleData pZ;
            pZ.puzzle = new int[16, 16];
            //Grab all lines from file
            String[] lineArr = new String[1];
            try{
                lineArr = lo.loadAllLines(_filePath);
            } catch (System.IO.FileNotFoundException e){
                throw e;
            }
            //Check if file opened is empty
            if (lineArr.Length <= 0) {
                //throw blank file error here
                throw new EmptyFileException("The file at " + _filePath + " is empty.");
            }
            //Make the first line the name
            pZ.name = lineArr[0].Split(new string[]{"--"}, StringSplitOptions.RemoveEmptyEntries)[0];
            //Now onto the blocks!
            //First let's determine the grid height by counting from line one to length of line array
            int gridHeight = lineArr.Length - BOARD_START_INDEX;
            //Then, we shall break up all the strings
            string[][] gridStrArr = new string[gridHeight][];
            for (int i = 0; i < gridHeight; i++) {
                gridStrArr[i] = lineArr[i + BOARD_START_INDEX].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
            //Now, let's figure out the grid width by counting how many blocks are on a line
            int gridWidth = gridStrArr[0].Length;
            //Now let's initialize int array representing the puzzle
            pZ.puzzle = new int[gridWidth, gridHeight];
            //Loop through int array and place appropriate tiles in
            for (int i = 0; i < pZ.puzzle.GetLength(0); i++) {
                for (int j = 0; j < pZ.puzzle.GetLength(1); j++) {
                    pZ.puzzle[i, j] = ParseHelper.ConvertToInt(gridStrArr[j][i]);
                }
            }
            return pZ;
        }
    }
}
