
using Avalonia.Controls;
using Avalonia.Interactivity;
using Agency;

namespace Agency;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void VacanciesButton_Click(object? sender, RoutedEventArgs e)
    {
        var window = new VacanciesWindow(this);
        window.Show();
        Hide();
    }

    private void CandidatesButton_Click(object? sender, RoutedEventArgs e)
    {
        var window = new CandidatesWindow(this);
        window.Show();
        Hide();
    }

    private void InterviewsButton_Click(object? sender, RoutedEventArgs e)
    {
        var window = new InterviewsWindow(this);
        window.Show();
        Hide();
    }

    private void EmploymentButton_Click(object? sender, RoutedEventArgs e)
    {
        var window = new EmploymentWindow(this);
        window.Show();
        Hide();
    }
}
