using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Agency;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void VacanciesButton_Click(object? sender, RoutedEventArgs e)
    {
        var window = new VacanciesWindow();
        window.Show();
    }

    private void CandidatesButton_Click(object? sender, RoutedEventArgs e)
    {
        var window = new CandidatesWindow();
        window.Show();
    }

    private void InterviewsButton_Click(object? sender, RoutedEventArgs e)
    {
        var window = new InterviewsWindow();
        window.Show();
    }

    private void EmploymentButton_Click(object? sender, RoutedEventArgs e)
    {
        var window = new EmploymentWindow();
        window.Show();
    }
}