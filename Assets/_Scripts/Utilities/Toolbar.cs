using Crosline.ToolbarExtender;
using Crosline.UnityTools;
using UnityEngine;

namespace Game.Utilities
{
    public class Toolbar
    {
        [Toolbar(ToolbarZone.LeftAlign, "It'll remove all playerprefs")]
        public static void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}