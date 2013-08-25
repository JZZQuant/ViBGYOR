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
            var canvasstrip = e.OriginalSource as Canvas;
            var midistrip = e.Source as MidiStrip;
            CultureElement vc = new CultureElement();
            //retrieve all parents and belonging objects
            var thiswindow = Window.GetWindow((e.Source as MidiStrip)) as FramelessWindow;
            var position = thiswindow.CenterDock.Children.IndexOf(midistrip);
            var vcMain = thiswindow.LeftDock.Children[position - 1] as CultureElement;
            //set Events
            HelperMethods.KeySetForCultureElements(FramelessWindow.ChangeColur, ref vc);
            vc.PreviewMouseDoubleClick += new MouseButtonEventHandler(DeleteNote);
            //set attributes
            vc.Background = vcMain.Background;
            Canvas.SetLeft(vc, GetLeft(e.GetPosition(canvasstrip).X));
            vc.Height = 15;
            vc.BorderBrush = Brushes.Transparent;
            vc.Width = ResizingAdorner.LastWidth == 0 ? FramelessWindow.defaultNoteMeasure : (FramelessWindow.defaultNoteMeasure / BeatLine.BeatWidth) * ResizingAdorner.LastWidth;
            vc.Curvature = 0;
            vc.Opacity = 0.6;
            canvasstrip.Children.Add(vc);
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

                var thiswindow = Window.GetWindow((e.Source as FrameworkElement)) as FramelessWindow;
                foreach (MidiStrip childs in (thiswindow.CenterDock).Children)
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

        private static double GetLeft(double left)
        {
            var t = BeatLine.LineSet.Where((x) => left > x.Key);
            return t.Last().Key;
        }
    }
}
