﻿#pragma checksum "..\..\MainEquipos.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "8A1C2770E1E8EC1E4E2F68D2253C27F5DA11CC1CE232D0B3E593A0A9AB529AEB"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using Prop;
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


namespace Prop {
    
    
    /// <summary>
    /// MainEquipos
    /// </summary>
    public partial class MainEquipos : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\MainEquipos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbEquipos;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\MainEquipos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNombreEquipo;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\MainEquipos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCreaEquipo;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\MainEquipos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lvUsers;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\MainEquipos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNombreJugador;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\MainEquipos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txApellidoPaterno;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\MainEquipos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtApellidoMaterno;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\MainEquipos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNumero;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\MainEquipos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAgregarJugador;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\MainEquipos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEliminaEquipo;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\MainEquipos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnGenerarJornadas;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\MainEquipos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgSave;
        
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
            System.Uri resourceLocater = new System.Uri("/Prop;component/mainequipos.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainEquipos.xaml"
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
            
            #line 8 "..\..\MainEquipos.xaml"
            ((Prop.MainEquipos)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cmbEquipos = ((System.Windows.Controls.ComboBox)(target));
            
            #line 16 "..\..\MainEquipos.xaml"
            this.cmbEquipos.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbEquipos_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtNombreEquipo = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.btnCreaEquipo = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\MainEquipos.xaml"
            this.btnCreaEquipo.Click += new System.Windows.RoutedEventHandler(this.btnCreaEquipo_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.lvUsers = ((System.Windows.Controls.ListView)(target));
            
            #line 19 "..\..\MainEquipos.xaml"
            this.lvUsers.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.lvUsers_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtNombreJugador = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.txApellidoPaterno = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.txtApellidoMaterno = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.txtNumero = ((System.Windows.Controls.TextBox)(target));
            
            #line 63 "..\..\MainEquipos.xaml"
            this.txtNumero.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtNumero_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnAgregarJugador = ((System.Windows.Controls.Button)(target));
            
            #line 64 "..\..\MainEquipos.xaml"
            this.btnAgregarJugador.Click += new System.Windows.RoutedEventHandler(this.btnAgregarJugador_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.btnEliminaEquipo = ((System.Windows.Controls.Button)(target));
            
            #line 80 "..\..\MainEquipos.xaml"
            this.btnEliminaEquipo.Click += new System.Windows.RoutedEventHandler(this.btnEliminaEquipo_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 83 "..\..\MainEquipos.xaml"
            ((System.Windows.Controls.Image)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Image_MouseDown);
            
            #line default
            #line hidden
            return;
            case 13:
            this.btnGenerarJornadas = ((System.Windows.Controls.Button)(target));
            
            #line 95 "..\..\MainEquipos.xaml"
            this.btnGenerarJornadas.Click += new System.Windows.RoutedEventHandler(this.btnGenerarJornadas_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.imgSave = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

