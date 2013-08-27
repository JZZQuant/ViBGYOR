using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViBGYOR
{
    public class SubBeatLine
    {
        public BeatLine start;
        public BeatLine end;
        public Tuple<int, int> fraction;

        /// <summary>
        /// Split a range of beats into fractions
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="fraction"></param>
        public SubBeatLine(BeatLine start, BeatLine end, Tuple<int, int> fraction)
        {
            this.start = start;
            this.end = end;
            this.fraction = fraction;
        }

        /// <summary>
        /// Assume the end beatline as 1 more than the current one
        /// </summary>
        /// <param name="start"></param>
        /// <param name="fraction"></param>
        public SubBeatLine(BeatLine start, Tuple<int, int> fraction)
        {
            BeatLine end;
            BeatLine.LineSet.TryGetValue(start.X + BeatLine.BeatWidth, out end);
            this.start = start;
            this.end = end;
            this.fraction = fraction;
        }

        /// <summary>
        /// Assume the end beatline as 1 more than the current one
        /// </summary>
        /// <param name="start"></param>
        /// <param name="fraction"></param>
        public SubBeatLine(int x, Tuple<int, int> fraction)
        {
            BeatLine end;
            BeatLine start;
            BeatLine.LineSet.TryGetValue(x, out start);
            BeatLine.LineSet.TryGetValue(start.X + BeatLine.BeatWidth, out end);
            this.start = start;
            this.end = end;
            this.fraction = fraction;
        }

        public static void CreateSubBeatSet(BeatLine start, BeatLine end, int fraction)
        {
            for (int i = 0; i < fraction; i++)
            {
                SubBeatLine s = new SubBeatLine(start, end, new Tuple<int, int>(i, fraction));
                BeatLine currentline = new BeatLine(start.Beat, start.Measure, s, start.SignNumerator);
            }
        }

        public static void CreateSubBeatSet(BeatLine start, int fraction)
        {
            for (int i = 1; i < fraction; i++)
            {
                SubBeatLine s = new SubBeatLine(start, new Tuple<int, int>(i, fraction));
                BeatLine currentline = new BeatLine(start.Beat, start.Measure, s, start.SignNumerator);
            }
        }
    }
}
