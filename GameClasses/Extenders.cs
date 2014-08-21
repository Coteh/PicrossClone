using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    /// <summary>
    /// Using a snippet from Hugoware's Enumeration Extensions
    /// Source: http://stackoverflow.com/questions/93744/most-common-c-sharp-bitwise-operations-on-enums
    /// </summary>
    public static class EnumerationExtensions {
        public static bool Has<T>(this System.Enum type, T value) {
            try {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            } catch {
                return false;
            }
        }
    }
}
