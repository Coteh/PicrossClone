using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameClasses {
    public interface LineSaver {
        void saveAllLines(string _filePath, string[] _lines);
    }

    public class ConcreteLineSaver : LineSaver {
        public void saveAllLines(string _filePath, string[] _lines) {
            try {
                File.WriteAllLines(_filePath, _lines);
            } catch (IOException e) {
                Console.WriteLine("ERROR: Couldn't save file due to IO error");
                throw new IOException("Couldn't save file at " + _filePath + "due to IO error.", e);
            }
        }
    }
}
