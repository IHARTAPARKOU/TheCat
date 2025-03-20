using System.Windows;
using System.Windows.Controls;
using TheCatApp.Presentation.ViewModels.Abstractions;

namespace TheCatApp.Presentation.View;

public partial class MainWindow : Window
{
    private readonly IMainViewModel viewModel;

    public MainWindow(IMainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = this.viewModel = viewModel;
    }

    private async void ListBoxScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (e.VerticalOffset + e.ViewportHeight >= e.ExtentHeight - 10)
        {
            await viewModel.LoadNextPageAsync();
        }
    }
}