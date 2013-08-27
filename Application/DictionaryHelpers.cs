using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using WPFHelpers;

namespace ViBGYOR
{
    public class DictionaryHelpers
    {
        public static void RemoveRange(ref SortedDictionary<double, BeatLine> liness, double start, double end)
        {
            var keys = liness.Keys.Where((x) => x > start && x < end).ToArray();
            foreach (var key in keys)
            {
                liness.Remove(key);
            }
        }

        public static void RemoveRange(ref SortedDictionary<double, BeatLine> liness, ref Canvas timeline, ref TextBox textbox, out double onset, out int offmeasure)
        {
            var line = UIHelper.FindVisualParent<ContextMenu>(textbox).PlacementTarget as Line;
            onset = line.X1;
            offmeasure = liness[onset].Measure;
            var startC = timeline.Children.IndexOf(line);
            var endC = timeline.Children.Count;
            timeline.Children.RemoveRange(startC, endC);
            double start = line.X1;
            var keys = liness.Keys.Where((x) => x >= start).ToArray();
            foreach (var key in keys)
            {
                liness.Remove(key);
            }
        }

        public static int RemoveRange(SortedDictionary<double, BeatLine> liness, ref Canvas timeline, double start, double end)
        {
            var keys = liness.Keys.Where((x) => x >= start && x < end).ToArray();
            var startC = timeline.Children.IndexOf(liness[start].Line);
            var endC = timeline.Children.IndexOf(liness[end].Line);
            foreach (var key in keys)
            {
                liness.Remove(key);
            }
            timeline.Children.RemoveRange(startC, endC - startC);
            return startC;
        }
    }
}
