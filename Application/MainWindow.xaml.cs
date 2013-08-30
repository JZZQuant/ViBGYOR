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
using System.Timers;

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
        bool mouseDown = false; // Set to 'true' when mouse is held down.
        Point mouseDownPos;
        private double selectedbeatLineStart;
        private double selectedbeatLineEnd;

        public FramelessWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += new MouseButtonEventHandler(MouseDownOnTimeLine);
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
            vc.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(HandleAllCultureElementChanges);
            HelperMethods.KeySetForCultureElements(ChangeColur, ref vc);
            CreateCorrespondingMidiStrip(ref vc);
            var position = this.LeftDock.Children.IndexOf(e.OriginalSource as UIElement);
            if (position > 0) this.LeftDock.Children.Insert(position, vc);
            else this.LeftDock.Children.Add(vc);
        }

        private void HandleAllCultureElementChanges(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    HelperMethods.DeleteMidiStripAndCultureElement(sender as CultureElement);
                }
                else if (Keyboard.IsKeyDown(Key.D))
                {
                    HelperMethods.MoveStrip(sender as CultureElement,1);
                }
                else if (Keyboard.IsKeyDown(Key.U))
                {
                    HelperMethods.MoveStrip(sender as CultureElement, -1);
                }
                else
                {
                    HelperMethods.SetFocusToCultureElements(sender as CultureElement);
                }
            }
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
            //m.Part_Host.Name = "other";
            m.Height = 16;
            m.VerticalAlignment = VerticalAlignment.Bottom;
            DockPanel.SetDock(m, Dock.Bottom);
            CenterDock.Children.Add(m);
            m.HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        private void TimingChangedFromMeasure(object sender, TextChangedEventArgs e)
        {
            try
            {
                var textbox = e.Source as TextBox;
                double onset;
                int offmeasure;
                DictionaryHelpers.RemoveRange(ref BeatLine.LineSet, ref this.TimeLine, ref textbox, out onset, out offmeasure);
                var measureSign = Convert.ToInt32((e.Source as TextBox).Text);
                for (int measure = offmeasure; measure < BeatLine.TotalMeasures; measure++)
                {
                    for (int beat = 0; beat < measureSign; beat++)
                    {

                        BeatLine currentline = new BeatLine(beat, measure, measureSign, onset - (BeatLine.BeatWidth * measureSign * offmeasure));
                    }
                }

                var count = BeatLine.LineSet.Last().Key;
                for (double i = onset; i < count; i += BeatLine.BeatWidth)
                {

                    var dict = BeatLine.LineSet[i];
                    SubBeatLine.CreateSubBeatSet(dict, 4);
                }

                foreach (var dict in BeatLine.LineSet)
                {
                    if (dict.Key >= onset)
                    {
                        var Menu = this.Resources["LineMenu"] as ContextMenu;
                        dict.Value.Line.ContextMenu = Menu;
                        TimeLine.Children.Add(dict.Value.Line);
                        TimeLine.Children.Add(dict.Value.TextBlock);
                    }
                }
            }
            catch
            {

            }
        }

        private void TimingChange(object sender, TextChangedEventArgs e)
        {
            BeatLine.LineSet.Clear();
            TimeLine.Children.Clear();
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

            foreach (var dict in BeatLine.LineSet)
            {
                var Menu = this.Resources["LineMenu"] as ContextMenu;
                dict.Value.Line.ContextMenu = Menu;
                TimeLine.Children.Add(dict.Value.Line);
                TimeLine.Children.Add(dict.Value.TextBlock);
            }
        }

        private void MouseDownOnTimeLine(object sender, MouseButtonEventArgs e)
        {
            if ((e.OriginalSource as FrameworkElement).Name != "Part_Host")
            {
                try
                {
                    DragMove();
                }
                catch
                {
                }
                return;
            };
            // Capture and track the mouse.
            mouseDown = true;
            mouseDownPos = e.GetPosition(TimeLine);
            TimeLine.CaptureMouse();

            // Initial placement of the drag selection box.         
            Canvas.SetLeft(selectionBox, mouseDownPos.X * BeatLine.ZoomFactor);
            Canvas.SetTop(selectionBox, mouseDownPos.Y);
            selectionBox.Width = 0;
            selectionBox.Height = 0;

            // Make the drag selection box visible.
            selectionBox.Visibility = Visibility.Visible;
            e.Handled = true;
        }

        private void MouseMoveOnTimeLine(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                // When the mouse is held down, reposition the drag selection box.
                Point mousePos = e.GetPosition(TimeLine);

                if (mouseDownPos.X < mousePos.X)
                {
                    Canvas.SetLeft(selectionBox, mouseDownPos.X * BeatLine.ZoomFactor);
                    selectionBox.Width = (mousePos.X - mouseDownPos.X) * BeatLine.ZoomFactor;
                    selectedbeatLineStart = mouseDownPos.X;
                    selectedbeatLineEnd = mousePos.X;
                }
                else
                {
                    Canvas.SetLeft(selectionBox, mousePos.X * BeatLine.ZoomFactor);
                    selectionBox.Width = (mouseDownPos.X - mousePos.X) * BeatLine.ZoomFactor;
                    selectedbeatLineEnd = mouseDownPos.X;
                    selectedbeatLineStart = mousePos.X;
                }

                if (mouseDownPos.Y < mousePos.Y)
                {
                    Canvas.SetTop(selectionBox, mouseDownPos.Y - 16);
                    selectionBox.Height = mousePos.Y - mouseDownPos.Y;
                }
                else
                {
                    Canvas.SetTop(selectionBox, mousePos.Y - 16);
                    selectionBox.Height = mouseDownPos.Y - mousePos.Y;
                }
            }
            e.Handled = false;
        }

        private void MouseUpOnTimeLine(object sender, MouseButtonEventArgs e)
        {
            // Release the mouse capture and stop tracking it.
            mouseDown = false;
            TimeLine.ReleaseMouseCapture();

            // Hide the drag selection box.
            selectionBox.Visibility = Visibility.Collapsed;

            Point mouseUpPos = e.GetPosition(TimeLine);
            e.Handled = false;

            foreach (var el in MidiStrip.CtrlSelected)
            {
                MidiStrip.LogicalUnFocusElement(el);
            }
            MidiStrip.CtrlSelected.Clear();
            foreach (var midiStrip in CenterDock.Children.OfType<MidiStrip>())
            {
                var i = CenterDock.Children.IndexOf(midiStrip);
                foreach (var cult in midiStrip.Children.OfType<CultureElement>().Where((x) =>
                    Canvas.GetLeft(x) + x.Width > selectedbeatLineStart * BeatLine.ZoomFactor && Canvas.GetLeft(x) < selectedbeatLineEnd * BeatLine.ZoomFactor
                    && 15 * (i - 2) < Canvas.GetTop(selectionBox) + selectionBox.Height && (15 * (i - 1)) > Canvas.GetTop(selectionBox)))
                {
                    MidiStrip.SelectedElementsChanged(cult, true);
                }
            }
        }

        private void TimeLineScrollSync_KeyUp(object sender, KeyEventArgs e)
        {
            int tupleNumber;
            int.TryParse(e.Key.ToString().TrimStart('D'), out tupleNumber);
            if (tupleNumber != 0)
            {
                double start; double end;
                var startC = HelperMethods.TuplateTheArea(selectedbeatLineStart, selectedbeatLineEnd, tupleNumber, ref TimeLine, out start, out end);
                foreach (var dict in BeatLine.LineSet.Where((x) => x.Key >= start && x.Key < end))
                {
                    var Menu = this.Resources["LineMenu"] as ContextMenu;
                    dict.Value.Line.ContextMenu = Menu;
                    TimeLine.Children.Insert(startC++, dict.Value.Line);
                    TimeLine.Children.Insert(startC++, dict.Value.TextBlock);
                }
            }
        }

        private void Grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Width = SystemParameters.PrimaryScreenWidth;
            var windowWidth = (double)GetValue(WidthProperty);
            Left = (SystemParameters.PrimaryScreenWidth / 2) - (windowWidth / 2);
            e.Handled = true;
        }

        private void WindowKeyHandles(object sender, KeyEventArgs e)
        {
            if (selectionBox.Width > 2 && selectionBox.Height > 2) TimeLineScrollSync_KeyUp(sender, e);
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                switch (e.Key.ToString())
                {
                    case "C":
                        HelperMethods.Copy();
                        break;
                    case "V":
                        HelperMethods.Paste(Mouse.GetPosition(TimeLine).X, CenterDock);
                        break;
                    case "X":
                        HelperMethods.Cut();
                        break;
                    case "D":
                        HelperMethods.Delete();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Key.Delete:
                        HelperMethods.Delete();
                        break;
                    default:
                        break;
                }
            }
            e.Handled = false;
        }
    }
}

