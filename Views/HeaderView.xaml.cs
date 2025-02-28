using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Memory.ViewModels;
using System.ServiceModel.Channels;


namespace Memory.Views
{
    public partial class HeaderView : UserControl
    {
        // private MainWindow _mainWindow;
        private readonly HeaderViewModel _headerViewModel;
        public void RefreshBindings()
        {
            MonthTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
        }

        public void SetMonth(string month)
        {
            var _companyViewModel = App.ServiceProvider.GetRequiredService<CompanyViewModel>();
            var date = _companyViewModel.Month;
            MessageBox.Show($"month w companyviewmodel {date}");
        }

        public HeaderView()
        {
            InitializeComponent();
            _headerViewModel = App.ServiceProvider.GetRequiredService<HeaderViewModel>();
            DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
    // var binding = MonthTextBox.GetBindingExpression(TextBox.TextProperty);
    // binding?.UpdateTarget();
// Dispatcher.InvokeAsync(() =>
//     {
//         if (DataContext == null)
//         {
//             MessageBox.Show("❌ [HeaderView] DataContext jest NULL!");
//         }
//         else
//         {
//             MessageBox.Show($"✅jeeeeeeeeeeeeeeeeee [HeaderView] DataContext ustawiony na {DataContext.GetType().FullName}");
//         }
//     });
        }

        private void YearTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
        private void MonthTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateTarget();
        }
        private void MonthTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // var textBox = sender as TextBox;
            // if (textBox == null) return;

            // if (textBox.Text.Length == 2)
            // {
            //     TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
            //     textBox.MoveFocus(request);
            // }
        }

        private void YearTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // var textBox = sender as TextBox;
            // if (textBox == null) return;

            // if (e.Key == Key.Enter || textBox.Text.Length == 4)
            // {
            //     System.Windows.Data.BindingExpression binding = textBox.GetBindingExpression(TextBox.TextProperty);
            //     binding?.UpdateSource();
            // }
        }


        private void OnSubmitButtonClick2(object sender, RoutedEventArgs e)
        {
            // UpdateDateCommand.Execute(new Tuple<int, int>(2023, 5));
            // MessageBox.Show("ChangeDate OnSubmitButtonClick");
            _headerViewModel.ChangeDate(new DateTime(2025, 2, 1));
        }
    }
}