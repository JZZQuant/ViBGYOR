﻿#pragma checksum "..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F6A635C1DA44497BAA155DDD18F31BA0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Expression.Controls;
using Microsoft.Expression.Media;
using Microsoft.Expression.Shapes;
using Microsoft.Windows.Themes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using ViBGYOR;
using ViBGYOR.Controls;


namespace ViBGYOR {
    
    
    /// <summary>
    /// FramelessWindow
    /// </summary>
    public partial class FramelessWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 149 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainGrid;
        
        #line default
        #line hidden
        
        
        #line 160 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ViBGYOR.Controls.RoundButton CloseButton;
        
        #line default
        #line hidden
        
        
        #line 184 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel QuickMinbon;
        
        #line default
        #line hidden
        
        
        #line 186 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button TopMenu;
        
        #line default
        #line hidden
        
        
        #line 221 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BottomMenu;
        
        #line default
        #line hidden
        
        
        #line 253 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer MainScrollAreaVertical;
        
        #line default
        #line hidden
        
        
        #line 255 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel LeftDock;
        
        #line default
        #line hidden
        
        
        #line 262 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer MainScrollAreaHorizontal;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ViBGYOR;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\MainWindow.xaml"
            ((ViBGYOR.FramelessWindow)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.Maximize);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 147 "..\..\MainWindow.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.AddNewCultureElement);
            
            #line default
            #line hidden
            return;
            case 3:
            this.MainGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.CloseButton = ((ViBGYOR.Controls.RoundButton)(target));
            
            #line 160 "..\..\MainWindow.xaml"
            this.CloseButton.Click += new System.Windows.RoutedEventHandler(this.OnClose);
            
            #line default
            #line hidden
            return;
            case 5:
            this.QuickMinbon = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 6:
            this.TopMenu = ((System.Windows.Controls.Button)(target));
            
            #line 186 "..\..\MainWindow.xaml"
            this.TopMenu.Click += new System.Windows.RoutedEventHandler(this.ExpandMainContextMenu);
            
            #line default
            #line hidden
            return;
            case 7:
            this.BottomMenu = ((System.Windows.Controls.Button)(target));
            
            #line 222 "..\..\MainWindow.xaml"
            this.BottomMenu.Click += new System.Windows.RoutedEventHandler(this.ExpandMainContextMenu);
            
            #line default
            #line hidden
            return;
            case 8:
            this.MainScrollAreaVertical = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 9:
            this.LeftDock = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 10:
            this.MainScrollAreaHorizontal = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

