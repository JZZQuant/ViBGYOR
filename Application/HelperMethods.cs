using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ViBGYOR.Controls;

namespace ViBGYOR
{
    public static class HelperMethods
    {
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
            var vcMain = thiswindow.LeftDock.Children[position] as CultureElement;
            //set attributes
            vc.Background = vcMain.Background;
            Canvas.SetLeft(vc, e.GetPosition(canvasstrip).X);
            HelperMethods.KeySetForCultureElements(FramelessWindow.ChangeColur, ref vc);
            vc.Height = 15;
            vc.Width = 45;
            vc.Curvature = 4;
            vc.Opacity = 10;
            canvasstrip.Children.Add(vc);
        }
    }
}
