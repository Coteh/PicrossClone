using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameClasses {
    public abstract class LineOpener {
        public abstract string[] loadAllLines(string _filePath);
    }

    public class ConcreteLineOpener : LineOpener {
        public override string[] loadAllLines(string _filePath) {
            List<string> lineList = new List<string>();
            try {
                using (StreamReader sr = new StreamReader(_filePath)) {
                    while (!sr.EndOfStream) {
                        lineList.Add(sr.ReadLine());
                    }
                }
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
