using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ViBGYOR
{
    public class BeatLine
    {
        public static double ZoomFactor = 1;
        public static double BeatWidth = 90;
        public const int TotalMeasures = 200;
        public static SortedDictionary<double, BeatLine> LineSet = new SortedDictionary<double, BeatLine>();

        private double offset;

        public double Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        private int signNumerator;
        public int SignNumerator
        {
            get { return signNumerator; }
            set { signNumerator = value; }
        }

        private double x;
        public double X
        {
            get
            {
                if (x == 0.0)
                {
                    if (subbeatDivision == null)
                    {
                        x = (Measure * SignNumerator + Beat) * BeatWidth + offset;
                    }
                    else
                    {
                        x = subbeatDivision.start.X +( (subbeatDivision.end.X - subbeatDivision.start.X) * ((double)subbeatDivision.fraction.Item1 / subbeatDivision.fraction.Item2)) + offset;
                    }
                }
                return x ;
            }
            private set { X = value; }
        }

        private double y;
        public double Y
        {
            get
            {
                if (y == 0.0)
                {
                    if (subbeatDivision != null)
                    {
                        y = 16;
                    }
                    else if (beat == 0)
                    {
                        y = 0;
                    }
                    else if (beat == 2)
                    {
                        y = 8;
                    }
                    else
                    {
                        y = 12;
                    }
                }
                return y;
            }
            set { y = value; }
        }

        private string name;
        public string Name
        {
            get
            {
                if (name == null)
                {
                    name = "Beat" + Measure.ToString() + "_" + Beat.ToString();
                }
                return name;
            }
            set { name = value; }
        }

        private int measure;
        public int Measure
        {
            get { return measure; }
            set { measure = value; }
        }

        private int beat;
        public int Beat
        {
            get { return beat; }
            set { beat = value; }
        }

        private SubBeatLine subbeatDivision;
        public SubBeatLine SubbeatDivision
        {
            get { return subbeatDivision; }
            set { subbeatDivision = value; }
        }

        private Line line;
        public Line Line
        {
            get
            {
                if (line == null)
                {
                    var l = new Line();
                    l.Name = this.Name;
                    l.X1 = this.X;
                    l.X2 = this.X;
                    l.Y1 = this.Y;
                    l.Y2 = 10000;
                    l.Stroke = this.Color;
                    l.Fill = this.Color;
                    l.StrokeThickness = 1;
                    l.Opacity = 0.8;
                    if (beat == 0 && subbeatDivision == null)
                    {
                        l.StrokeThickness = 2;
                        l.Opacity = 1;
                    }
                    line = l;
                }
                return line;
            }
            set { line = value; }
        }

        private TextBlock textBlock;
        public TextBlock TextBlock
        {
            get
            {
                if (textBlock == null)
                {
                    if (SubbeatDivision == null)
                    {
                        textBlock = new TextBlock() { Text = Measure.ToString() + "." + Beat.ToString(), Foreground = this.Color };
                    }
                    else
                    {
                        textBlock = new TextBlock() { Text = "" };
                    }
                }
                Canvas.SetLeft(textBlock, X + 2);
                return textBlock;
            }
            set { textBlock = value; }
        }

        private Brush color;
        public Brush Color
        {
            get
            {
                if (color == null)
                {
                    if (SubbeatDivision != null)
                    {
                        color = Brushes.DarkGray;
                    }
                    else if (Beat == 0)
                    {
                        color = Brushes.Yellow;
                    }
                    else if (Beat % 2 == 0)
                    {
                        color = Brushes.GreenYellow;
                    }
                    else
                    {
                        color = Brushes.DeepSkyBlue;
                    }
                }
                return color;
            }
            set { color = value; }
        }

        public BeatLine(int beat, int measure, SubBeatLine subbeatDivision, int signNumerator)
        {
            this.beat = beat;
            this.measure = measure;
            this.subbeatDivision = subbeatDivision;
            this.signNumerator = signNumerator;
            LineSet.Add(this.X, this);
        }

        public BeatLine(int beat, int measure, int signNumerator)
        {
            this.beat = beat;
            this.measure = measure;
            this.signNumerator = signNumerator;
            LineSet.Add(this.X, this);
        }

        public BeatLine(int beat, int measure, SubBeatLine subbeatDivision, int signNumerator, double offset)
        {
            this.offset = offset;
            this.beat = beat;
            this.measure = measure;
            this.subbeatDivision = subbeatDivision;
            this.signNumerator = signNumerator;
            LineSet.Add(this.X, this);
        }

        public BeatLine(int beat, int measure, int signNumerator, double offset)
        {
            this.offset = offset;
            this.beat = beat;
            this.measure = measure;
            this.signNumerator = signNumerator;
            LineSet.Add(this.X, this);
        }

        public static void ColorMapping(int i, Line l, TextBlock t)
        {
            double beat = i / 4;
            int localbeat = i % 4;

            if (i % 4 == 0)
            {
                l.Stroke = Brushes.GreenYellow; l.Fill = Brushes.GreenYellow;
                t.Text = (beat + 1).ToString();
            }
            else if (i % 2 == 0)
            {
                l.Stroke = Brushes.Black; l.Fill = Brushes.Black;
                t.Text = (beat + 1).ToString() + "." + localbeat.ToString();
            }
            else
            {
                l.Stroke = Brushes.Gray; l.Fill = Brushes.Gray;
            }
        }
    }
}
