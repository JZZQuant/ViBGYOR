using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViBGYORModel;

namespace ViBGYOR.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CultureSetButton"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CultureSetButton;assembly=CultureSetButton"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class CultureElement : Button
    {
        public CultureElementModel Model;
        public List<DependencyProperty> WatchList = new List<DependencyProperty>() {
            CultureElement.WidthProperty,
            CultureElement.BackgroundProperty,
            CultureElement.PitchProperty,
            CultureElement.DisplayTxtProperty,
            CultureElement.FontProperty,
            Canvas.LeftProperty};

        static CultureElement()
        {
            CurvatureProperty = DependencyProperty.Register("Curvature", typeof(int), typeof(CultureElement), new UIPropertyMetadata(4));
            PitchProperty = DependencyProperty.Register("Pitch", typeof(string), typeof(CultureElement), new UIPropertyMetadata("C3"));
            FontProperty = DependencyProperty.Register("Font", typeof(string), typeof(CultureElement), new UIPropertyMetadata("Arial"));
            DisplayTxtProperty = DependencyProperty.Register("DisplayTxt", typeof(string), typeof(CultureElement), new UIPropertyMetadata("C"));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CultureElement), new FrameworkPropertyMetadata(typeof(CultureElement)));
        }

        public static DependencyProperty DisplayTxtProperty;

        public string DisplayTxt
        {
            get { return (string)GetValue(DisplayTxtProperty); }
            set { SetValue(DisplayTxtProperty, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (WatchList.Contains(e.Property))
            {

            }
            base.OnPropertyChanged(e);
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
        }

        public static DependencyProperty CurvatureProperty;

        public int Curvature
        {
            get { return (int)GetValue(CurvatureProperty); }
            set { SetValue(CurvatureProperty, value); }
        }

        public static DependencyProperty PitchProperty;

        public string Pitch
        {
            get { return (string)GetValue(PitchProperty); }
            set { SetValue(PitchProperty, value); }
        }

        public static DependencyProperty FontProperty;

        public string Font
        {
            get { return (string)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }
    }
}
