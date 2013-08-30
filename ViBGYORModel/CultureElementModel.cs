using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViBGYORModel
{
    public class CultureElementModel
    {
        public static List<List<CultureElementModel>> ModelStack = new List<List<CultureElementModel>>();
        public static bool isStackingOn = true;

        public CultureElementModel(string background, string pitch, string display, string font, double left, bool isNote, bool deleted, double width =15, string parent = "none")
        {
            this.Width = width;
            this.Background = background;
            this.Display = display;
            this.Pitch = pitch;
            this.Font = font;
            this.Left = left;
            this.isNote = isNote;
            this.Deleted = deleted;
            this.Parent = parent;
        }

        public double Width;
        public string Background;
        public string Pitch;
        public string Display;
        public string Font;
        public double Left;
        public bool isNote;
        public bool Deleted;
        public string Parent ;
    }
}
