
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Agency;

public partial class EmploymentWindow : Window
{
    private readonly Window _mainWindow;

    public EmploymentWindow(Window mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
    }

    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        _mainWindow.Show();
        _mainWindow.Activate();
        Close();
    }
}
