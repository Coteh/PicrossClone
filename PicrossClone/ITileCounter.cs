using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossClone {
    public interface ITileCounter {
        CountData countRow(int _row);
        CountData countCol(int _col);
        bool Update(int _x, int _y, int _value);
    }
}
