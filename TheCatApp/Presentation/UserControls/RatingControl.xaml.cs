using System.Windows;
using System.Windows.Controls;

namespace TheCatApp.Presentation.UserControls;

public partial class RatingControl : UserControl
{
    public RatingControl()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(RatingControl), new PropertyMetadata("Rating:"));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly DependencyProperty RatingProperty =
        DependencyProperty.Register("Rating", typeof(int), typeof(RatingControl), new PropertyMetadata(0));

    public int Rating
    {
        get => (int)GetValue(RatingProperty);
        set => SetValue(RatingProperty, value);
    }
}
