﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossClone {
    public interface ITileCounter {
        string countRow(int _row);
        string countCol(int _col);
    }
}