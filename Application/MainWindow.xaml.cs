using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ViBGYOR.Adorners;
using ViBGYOR.Controls;

namespace ViBGYOR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FramelessWindow : Window
    {
        public static double defaultNoteMeasure = BeatLine.BeatWidth;
        public static int i = 0;
        public static RoutedCommand AddNewCultureElementCommand = new RoutedCommand();
        public static RoutedCommand ChangeColur = new RoutedCommand();

        public FramelessWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += delegate { DragMove(); };
        }

        private void ExpandMainContextMenu(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddNewCultureElement(object sender, ExecutedRoutedEventArgs e)
        {
            var vc = new ViBGYOR.Controls.CultureElement();
            var color = this.Resources["G"] as Brush;
            vc.Background = color;
            vc.Height = 15;
            vc.Curvature = 4;
            vc.Opacity = 0.7;
            vc.Name = "Element_" + i++.ToString();
            DockPanel.SetDock(vc, Dock.Top);
            vc.InputBindings.Add(new MouseBinding(AddNewCultureElementCommand, new MouseGesture(MouseAction.LeftDoubleClick)));
            HelperMethods.KeySetForCultureElements(ChangeColur, ref vc);
            CreateCorrespondingMidiStrip(ref vc);
            var position = this.LeftDock.Children.IndexOf(e.OriginalSource as UIElement);
            if (position > 0) this.LeftDock.Children.Insert(position, vc);
            else this.LeftDock.Children.Add(vc);
        }

        private void ChangeColorHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var resKey = e.Parameter.ToString();
            var color = this.Resources[resKey] as Brush;
            var cultureEl = e.OriginalSource as CultureElement;
            cultureEl.Background = color;
        }

        private void CreateCorrespondingMidiStrip(ref CultureElement vc)
        {
            var midiStrip = new MidiStrip();
            midiStrip.Part_Host.Height = vc.Height;
            midiStrip.HorizontalAlignment = HorizontalAlignment.Stretch;
            midiStrip.Name = vc.Name + "_Strip";
            midiStrip.MouseDoubleClick += HelperMethods.AddMidiNotesToStrip;
            midiStrip.BorderBrush = Brushes.Black;
            DockPanel.SetDock(midiStrip, Dock.Top);
            this.CenterDock.Children.Add(midiStrip);
        }

        private void Zoom(object sender, MouseWheelEventArgs e)
        {
            HelperMethods.Zoom(sender, e);
        }

        private void VerticalScrollSync(object sender, ScrollChangedEventArgs e)
        {
            MainScrollAreaHorizontal.ScrollToVerticalOffset(MainScrollAreaVertical.VerticalOffset);
        }

        private void HorizontalScrollSync(object sender, ScrollChangedEventArgs e)
        {
            TimeLineScrollSync.ScrollToHorizontalOffset(MainScrollAreaHorizontal.HorizontalOffset);
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            //create an empty midi strip in bottom to fill space for the UI
            MidiStrip m = new MidiStrip();
            m.Height = 16;
            m.VerticalAlignment = VerticalAlignment.Bottom;
            DockPanel.SetDock(m, Dock.Bottom);
            CenterDock.Children.Add(m);
            m.HorizontalAlignment = HorizontalAlignment.Stretch;
            TimeLineCreate();

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send,new ThreadStart(() =>
                {
                    foreach (var dict in BeatLine.LineSet)
                    {
                        TimeLine.Children.Add(dict.Value.Line);
                        TimeLine.Children.Add(dict.Value.TextBlock);
                    }
                }
                ));
        }

        private void TimeLineCreate()
        {
            var measureSign = Convert.ToInt32(measurelength.Text);
            for (int measure = 0; measure < BeatLine.TotalMeasures; measure++)
            {
                for (int beat = 0; beat < measureSign; beat++)
                {
                    BeatLine currentline = new BeatLine(beat, measure, measureSign);
                }
            }
            var count = BeatLine.LineSet.Count;
            for (int i = 0; i < count - 1; i++)
            {
                var dict = BeatLine.LineSet[i * BeatLine.BeatWidth];
                SubBeatLine.CreateSubBeatSet(dict, 4);
            }
        }
    }
}

