using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClasses {
    public class ScreenManager {
        private List<Screen> screenList;
        private int currScreen;

        public ScreenManager() {
            screenList = new List<Screen>();
        }

        public Screen ChangeScreen(int _screenID) {
            currScreen = _screenID;
            screenList[currScreen].Start();
            return screenList[currScreen];
        }

        public int AddScreen(Screen _screen) {
            screenList.Add(_screen);
            return screenList.Count - 1; //returns int id of screen
        }

        public int[] AddScreen(Screen[] _screenArr) {
            screenList.AddRange(_screenArr);
            int[] idsToReturn = new int[_screenArr.Length];
            for (int i = 0; i < idsToReturn.Length; i++) {
                idsToReturn[i] = screenList.Count - 1 - (idsToReturn.Length - i);
            }
            return idsToReturn;
        }
    }
}
