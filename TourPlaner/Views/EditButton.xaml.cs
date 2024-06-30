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

namespace TourPlaner.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class EditButton : UserControl
    {
        public static readonly DependencyProperty AddCommandProperty = DependencyProperty.Register(
            "AddCommand", typeof(ICommand), typeof(EditButton), new PropertyMetadata(null));

        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(
            "DeleteCommand", typeof(ICommand), typeof(EditButton), new PropertyMetadata(null));

        public static readonly DependencyProperty ModifyCommandProperty = DependencyProperty.Register(
            "ModifyCommand", typeof(ICommand), typeof(EditButton), new PropertyMetadata(null));

        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }

        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }
        public ICommand ModifyCommand
        {
            get { return (ICommand)GetValue(ModifyCommandProperty); }
            set { SetValue(ModifyCommandProperty, value); }
        }
        public EditButton()
        {
            InitializeComponent();
        }
    }
}
