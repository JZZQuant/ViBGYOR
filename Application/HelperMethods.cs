﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ViBGYOR.Adorners;
using ViBGYOR.Controls;

namespace ViBGYOR
{
    public static class HelperMethods
    {
        const double ScaleRate = 1.1;

        public static void SetInputBindings(RoutedCommand r, Key k, ref CultureElement c)
        {
            KeyBinding t = new KeyBinding
            {
                Key = k,
                Command = r,
            };
            t.CommandParameter = k;
            c.InputBindings.Add(t);
        }

        public static void SetInputBindings(RoutedCommand r, KeyGesture k, ref CultureElement c)
        {
            KeyBinding t = new KeyBinding(r, k);
            t.CommandParameter = k;
            c.InputBindings.Add(t);
        }

        public static void SetInputBindings(RoutedCommand r, Key k, ModifierKeys m, ref CultureElement c)
        {
            KeyBinding t = new KeyBinding(r, k, m);
            t.CommandParameter = k;
            c.InputBindings.Add(t);
        }

        public static void KeySetForCultureElements(RoutedCommand ChangeColur, ref Controls.CultureElement vc)
        {
            HelperMethods.SetInputBindings(ChangeColur, Key.V, ref vc);
            HelperMethods.SetInputBindings(ChangeColur, Key.I, ref vc);
            HelperMethods.SetInputBindings(ChangeColur, Key.B, ref vc);
            HelperMethods.SetInputBindings(ChangeColur, Key.G, ref vc);
            HelperMethods.SetInputBindings(ChangeColur, Key.Y, ref vc);
            HelperMethods.SetInputBindings(ChangeColur, Key.O, ref vc);
            HelperMethods.SetInputBindings(ChangeColur, Key.R, ref vc);
            HelperMethods.SetInputBindings(ChangeColur, Key.W, ref vc);
            HelperMethods.SetInputBindings(ChangeColur, Key.K, ref vc);
        }

        public static void AddMidiNotesToStrip(object sender, MouseButtonEventArgs e)
        {
            var midistrip = sender as MidiStrip;
            var canvasstrip = midistrip.Part_Host as Canvas;
            CultureElement vc = new CultureElement();
            //retrieve all parents and belonging objects
            var thiswindow = Window.GetWindow(midistrip) as FramelessWindow;
            var position = thiswindow.CenterDock.Children.IndexOf(midistrip) - 1;
            var vcMain = thiswindow.LeftDock.Children.OfType<CultureElement>().ElementAt(position - 1);
            //set Events
            HelperMethods.KeySetForCultureElements(FramelessWindow.ChangeColur, ref vc);
            vc.PreviewMouseDoubleClick += new MouseButtonEventHandler(DeleteNote);
            //set attributes
            vc.Background = vcMain.Background;
            Canvas.SetLeft(vc, ResizingAdorner.GetLeft(e.GetPosition(canvasstrip).X));
            vc.Height = 15;
            vc.BorderBrush = Brushes.Transparent;
            vc.Width = ResizingAdorner.LastWidth == 0 ? FramelessWindow.defaultNoteMeasure : ResizingAdorner.LastWidth;
            vc.Curvature = 4;
            vc.Opacity = 0.6;
            vc.Name = midistrip.Name + "_" + MidiStrip.noteCount;
            canvasstrip.Children.Add(vc);
            vc.Focus();
            MidiStrip.SelectedElementsChanged(vc, Keyboard.IsKeyDown(Key.LeftCtrl));
            midistrip.CanvasWidth = e.GetPosition(canvasstrip).X + vc.Width;
        }

        public static void DeleteNote(object sender, MouseButtonEventArgs e)
        {
            (((e.Source as CultureElement).Parent as Canvas).Parent as MidiStrip).selectedElement = null;
            ((e.Source as CultureElement).Parent as Canvas).Children.Remove(e.Source as CultureElement);
        }

        public static void Zoom(object sender, MouseWheelEventArgs e)
        {
            //get + or -1  and rasie invert the scale rate
            if (e.Delta != 0)
            {
                int sign = (Math.Abs(e.Delta) / e.Delta);
                double localScale = Math.Pow(ScaleRate, sign);
                FramelessWindow.defaultNoteMeasure *= localScale;
                ResizingAdorner.LastWidth *= localScale;
                BeatLine.ZoomFactor = FramelessWindow.defaultNoteMeasure / BeatLine.BeatWidth;
                var thiswindow = Window.GetWindow((e.Source as FrameworkElement)) as FramelessWindow;
                foreach (MidiStrip childs in (thiswindow.CenterDock).Children.OfType<MidiStrip>())
                {
                    foreach (var child in childs.Children)
                    {
                        var UIChile = child as FrameworkElement;
                        UIChile.Width *= localScale;
                        Canvas.SetLeft(UIChile, Canvas.GetLeft(UIChile) * localScale);
                    }
                }

                thiswindow.st.ScaleX *= localScale;
                e.Handled = true;
            }
        }

        internal static int TuplateTheArea(double selectedbeatLineStart, double selectedbeatLineEnd, int tupleNumber, ref Canvas cv, out double start, out double end)
        {
            start = BeatLine.LineSet.Where((x) => selectedbeatLineStart < x.Key).First().Key;
            end = BeatLine.LineSet.Where((x) => selectedbeatLineEnd > x.Key).Last().Key;
            var beat = BeatLine.LineSet;
            Canvas can = cv;
            var linestart = BeatLine.LineSet[start];
            var lineend = BeatLine.LineSet[end];
            var startC = DictionaryHelpers.RemoveRange(beat, ref can, start, end);
            SubBeatLine.CreateSubBeatSet(linestart, lineend, tupleNumber);
            return startC;
        }

        internal static void Copy()
        {
            MidiStrip.CopyBuffer.Clear();
            foreach (var cult in MidiStrip.CtrlSelected.OfType<CultureElement>())
            {
                CultureElement vc = new CultureElement();
                HelperMethods.KeySetForCultureElements(FramelessWindow.ChangeColur, ref vc);
                vc.PreviewMouseDoubleClick += new MouseButtonEventHandler(DeleteNote);
                //set attributes
                vc.Background = cult.Background;
                Canvas.SetLeft(vc, Canvas.GetLeft(cult));
                vc.Height = cult.Height;
                vc.BorderBrush = cult.BorderBrush;
                vc.Width = cult.Width;
                vc.Curvature = cult.Curvature;
                vc.Opacity = cult.Opacity;
                vc.Name = cult.Name + "_Copy";
                MidiStrip.CopyBuffer.Add(new Tuple<CultureElement, MidiStrip>(vc, (cult.Parent as Canvas).Parent as MidiStrip));
            }
        }

        internal static void Paste(double onset, DockPanel fr)
        {
            if (MidiStrip.CopyBuffer.Count > 0)
            {
                var initial = (MidiStrip.CopyBuffer[0].Item1 as CultureElement);
                var offset = onset - Canvas.GetLeft(initial);
                foreach (var tuple in MidiStrip.CopyBuffer)
                {
                    if (fr.Children.Contains(tuple.Item2 as MidiStrip))
                    {
                        var cult = tuple.Item1 as CultureElement;
                        var left = Math.Max(offset + Canvas.GetLeft(cult), 0) * BeatLine.ZoomFactor;
                        var leftNormalized = ResizingAdorner.GetLeft(left);
                        Canvas.SetLeft(cult, leftNormalized);
                        (tuple.Item2 as MidiStrip).Part_Host.Children.Add(cult);
                        cult.Focus();
                        (tuple.Item2 as MidiStrip).CanvasWidth = Canvas.GetLeft(cult) + cult.Width;
                    }
                }
                List<Tuple<CultureElement, MidiStrip>> temp = new List<Tuple<CultureElement, MidiStrip>>();
                foreach (var cult1 in MidiStrip.CopyBuffer)
                {
                    var cult = cult1.Item1 as CultureElement;
                    CultureElement vc = new CultureElement();
                    HelperMethods.KeySetForCultureElements(FramelessWindow.ChangeColur, ref vc);
                    vc.PreviewMouseDoubleClick += new MouseButtonEventHandler(DeleteNote);
                    //set attributes
                    vc.Background = cult.Background;
                    Canvas.SetLeft(vc, Canvas.GetLeft(cult));
                    vc.Height = cult.Height;
                    vc.BorderBrush = cult.BorderBrush;
                    vc.Width = cult.Width;
                    vc.Curvature = cult.Curvature;
                    vc.Opacity = cult.Opacity;
                    vc.Name = cult.Name + "_Copy";
                    temp.Add(new Tuple<CultureElement, MidiStrip>(vc, (cult.Parent as Canvas).Parent as MidiStrip));
                }

                MidiStrip.CopyBuffer.Clear();
                MidiStrip.CopyBuffer.AddRange(temp);
            }
        }

        internal static void Cut()
        {
            Copy();
            Delete();
        }

        internal static void Delete()
        {
            foreach (var cult in MidiStrip.CtrlSelected.OfType<CultureElement>())
            {
                ((cult.Parent as Canvas).Parent as MidiStrip).selectedElement = null;
                (cult.Parent as Canvas).Children.Remove(cult as CultureElement);
            }
        }

        internal static void DeleteMidiStripAndCultureElement(CultureElement cultureElement)
        {
            var thiswindow = Window.GetWindow(cultureElement) as FramelessWindow;
            var position = thiswindow.LeftDock.Children.IndexOf(cultureElement) + 1;
            var midistrip = thiswindow.CenterDock.Children.OfType<MidiStrip>().ElementAt(position);
            midistrip.selectedElement = null;
            midistrip.Children.Clear();
            thiswindow.LeftDock.Children.Remove(cultureElement);
            thiswindow.CenterDock.Children.Remove(midistrip);
        }

        internal static void SetFocusToCultureElements(CultureElement cultureElement)
        {
            var thiswindow = Window.GetWindow(cultureElement) as FramelessWindow;
            foreach (var c in thiswindow.LeftDock.Children.OfType<CultureElement>())
            {
                c.Opacity = 0.6;
            }
            cultureElement.Opacity = 0.9;
        }

        internal static void MoveStrip(CultureElement cultureElement, int p)
        {
            var thiswindow = Window.GetWindow(cultureElement) as FramelessWindow;
            var position = thiswindow.LeftDock.Children.IndexOf(cultureElement) + 1;
            var midistrip = thiswindow.CenterDock.Children.OfType<MidiStrip>().ElementAt(position);
            if (position + p > 0 && position + p <= thiswindow.LeftDock.Children.Count)
            {
                thiswindow.LeftDock.Children.Remove(cultureElement);
                thiswindow.CenterDock.Children.Remove(midistrip);
                thiswindow.LeftDock.Children.Insert(position - 1 + p, cultureElement);
                thiswindow.CenterDock.Children.Insert(position + 1 + p, midistrip);
            }
        }
    }
}
