using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using ViBGYOR.Controls;


namespace ViBGYOR.Adorners
{
    [ContentProperty("Children")]
    public partial class MidiStrip : UserControl
    {
        AdornerLayer aLayer;

        public static readonly DependencyPropertyKey ChildrenProperty = DependencyProperty.RegisterReadOnly(
          "Children",
          typeof(UIElementCollection),
          typeof(MidiStrip),
          new PropertyMetadata());

        public UIElementCollection Children
        {
            get { return (UIElementCollection)GetValue(ChildrenProperty.DependencyProperty); }
            private set { SetValue(ChildrenProperty, value); }
        }

        bool _isDown;
        bool _isDragging;
        bool selected = false;
        public UIElement selectedElement = null;

        Point _startPoint;
        private double _originalLeft;
        private double _originalTop;

        public MidiStrip()
        {
            InitializeComponent();
            Children = Part_Host.Children;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Window1_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
            Part_Host.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(myCanvas_PreviewMouseLeftButtonDown);
            Part_Host.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
            (Window.GetWindow((e.Source as FrameworkElement)) as FrameworkElement).MouseLeftButtonUp += (DragFinishedMouseHandler);
            (Window.GetWindow((e.Source as FrameworkElement)) as FrameworkElement).MouseMove += (Window1_MouseMove);
            (Window.GetWindow((e.Source as FrameworkElement)) as FrameworkElement).MouseLeave += (Window1_MouseLeave);
        }

        // Handler for drag stopping on leaving the window
        void Window1_MouseLeave(object sender, MouseEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }

        // Handler for drag stopping on user choise
        void DragFinishedMouseHandler(object sender, MouseButtonEventArgs e)
        {
            foreach (MidiStrip strip in (this.Parent as DockPanel).Children)
            {
                strip.StopDragging();
            }
            e.Handled = true;
        }

        // Method for stopping dragging
        public void StopDragging()
        {
            if (_isDown)
            {
                _isDown = false;
                _isDragging = false;
            }
        }

        // Hanler for providing drag operation with selected element
        void Window1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) &&
                    ((Math.Abs(e.GetPosition(Part_Host).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(Part_Host).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                    _isDragging = true;

                if (_isDragging && selectedElement != null)
                {
                    Point position = Mouse.GetPosition(Part_Host);
                    Canvas.SetTop(selectedElement, position.Y - (_startPoint.Y - _originalTop));
                    if (position.X - (_startPoint.X - _originalLeft) > 0)
                    {
                        Canvas.SetLeft(selectedElement, ResizingAdorner.GetRight(position.X - (_startPoint.X - _originalLeft)));
                        CanvasWidth =  Canvas.GetLeft(selectedElement);
                    }
                }
            }
        }

        // Handler for clearing element selection, adorner removal
        void Window1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selected)
            {
                selected = false;
                if (selectedElement != null)
                {
                    aLayer.Remove(aLayer.GetAdorners(selectedElement)[0]);
                    selectedElement = null;
                }
            }
        }

        // Handler for element selection on the canvas providing resizing adorner
        void myCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Remove selection on clicking anywhere the window
            if (selected)
            {
                selected = false;
                if (selectedElement != null)
                {
                    // Remove the adorner from the selected element
                    aLayer.Remove(aLayer.GetAdorners(selectedElement)[0]);
                    selectedElement = null;
                }
            }

            // If any element except canvas is clicked, 
            // assign the selected element and add the adorner
            if (e.Source != Part_Host)
            {
                _isDown = true;
                _startPoint = e.GetPosition(Part_Host);

                selectedElement = e.Source as UIElement;
                selectedElement.Focus();
                _originalLeft = Canvas.GetLeft(selectedElement);
                _originalTop = Canvas.GetTop(selectedElement);

                aLayer = AdornerLayer.GetAdornerLayer(selectedElement);
                aLayer.Add(new ResizingAdorner(selectedElement));
                selected = true;
                e.Handled = true;
            }
        }

        public double CanvasWidth
        {
            get
            {
                return _canvasWidth;
            }
            set
            {
                _canvasWidth = Math.Max(_canvasWidth, value + 1000);
                ((Window.GetWindow((this as MidiStrip)) as FrameworkElement).FindName("TimeLine") as FrameworkElement).Width = _canvasWidth; ;
                (this.Parent as DockPanel).Width = _canvasWidth;
            }
        }

        private static double _canvasWidth;
    }
}
