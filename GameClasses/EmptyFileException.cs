using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    public class EmptyFileException : Exception {
        public EmptyFileException() {
        }

        public EmptyFileException(string _message)
            : base(_message) {
        }

        public EmptyFileException(string _message, Exception _inner)
            : base(_message, _inner) {
        }
    }
}
