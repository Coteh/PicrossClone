using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    public class ParseHelper {
        public static int ConvertToInt(string _value) {
            int value;
            if (!int.TryParse(_value, out value)) { value = 0; }
            return value;
        }
    }
}
