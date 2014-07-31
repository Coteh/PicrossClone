using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    public enum SelectEventState {
        NONE = 0,
        LEFT_SELECT = 1,
        MIDDLE_SELECT = 2,
        RIGHT_SELECT = 4
    }
    public class SelectEventArgs : EventArgs {
        private SelectEventState selectState;
        public SelectEventArgs(SelectEventState _state) {
            selectState = _state;
        }

        public SelectEventState SelectState { get { return selectState; } }
    }
}
