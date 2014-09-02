using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameClasses {
    public interface LineOpener {
        string[] loadAllLines(string _filePath);
    }

    public class ConcreteLineOpener : LineOpener {
        public string[] loadAllLines(string _filePath) {
            List<string> lineList = new List<string>();
            try {
                using (StreamReader sr = new StreamReader(_filePath)) {
                    while (!sr.EndOfStream) {
                        lineList.Add(sr.ReadLine());
                    }
                }
            } catch (FileNotFoundException e) {
                throw new FileNotFoundException("Puzzle map at " + _filePath + " cannot be found.", e);
            } catch (IOException) {

            } catch {

            }
            string[] lineArr = new string[lineList.Count];
            for (int i = 0; i < lineArr.Length; i++) {
                lineArr[i] = lineList[i];
            }
            return lineArr;
        }
    }
}
