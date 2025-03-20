using System.Windows;
using TheCatApp.Presentation.ViewModels.Abstractions;

namespace TheCatApp.Presentation.View;

public partial class CatBreedDetailsWindow : Window
{
    public CatBreedDetailsWindow(ICatBreedDetailsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
