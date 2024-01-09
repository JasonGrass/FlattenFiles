using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlattenFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FlattenOptions Options { get; private set; }
        public MainWindow()
        {
            Options = new FlattenOptions();
            InitializeComponent();
        }

        private async void Execute_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
            {
                return;
            }

            try
            {
                btn.IsEnabled = false;
                var flatten = new Flatten(Options, PrintLog);
                PrintLog("开始执行");
                await Task.Run(() => { flatten.FlattenFiles(); });
                PrintLog("执行结束");
            }
            catch (Exception ex)
            {
                PrintLog($"{ex.GetType().Name} - {ex.Message}");
            }
            finally
            {
                btn.IsEnabled = true;
            }
        }

        private void PrintLog(string info)
        {
            this.Dispatcher.InvokeAsync(() =>
            {
                InfoTextBox.Text += $"[{DateTime.Now:HHmmss}] " + info + Environment.NewLine;
                InfoTextBox.ScrollToEnd();
            });
        }
    }
}