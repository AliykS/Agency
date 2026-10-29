
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Agency;

public partial class CandidatesWindow : Window
{
    private readonly Window _mainWindow;

    private readonly ObservableCollection<Candidate> _allCandidates = new();
    private readonly ObservableCollection<Candidate> _visibleCandidates = new();

    public CandidatesWindow(Window mainWindow)
    {
        InitializeComponent();

        _mainWindow = mainWindow;

        CandidatesList.ItemsSource = _visibleCandidates;

        GenderBox.ItemsSource = new[]
        {
            "Мужской",
            "Женский"
        };
        GenderBox.SelectedIndex = 0;

        StatusBox.ItemsSource = new[]
        {
            "Новый",
            "Приглашён на собеседование",
            "Принят на работу",
            "Отказ"
        };
        StatusBox.SelectedIndex = 0;

        FilterStatusBox.ItemsSource = new[]
        {
            "Все статусы",
            "Новый",
            "Приглашён на собеседование",
            "Принят на работу",
            "Отказ"
        };
        FilterStatusBox.SelectedIndex = 0;

        // Демонстрационные кандидаты
        _allCandidates.Add(new Candidate(
            "Иванов Иван Иванович",
            "Мужской",
            "+7 (900) 111-22-33",
            "ivanov@example.com",
            "Менеджер по продажам",
            "Новый"));

        _allCandidates.Add(new Candidate(
            "Петрова Анна Сергеевна",
            "Женский",
            "+7 (900) 222-33-44",
            "petrova@example.com",
            "Бухгалтер",
            "Приглашён на собеседование"));

        _allCandidates.Add(new Candidate(
            "Сидоров Алексей Петрович",
            "Мужской",
            "+7 (900) 333-44-55",
            "sidorov@example.com",
            "Офис-менеджер",
            "Принят на работу"));

        RefreshCandidates();
    }

    private void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        string fullName = FullNameBox.Text?.Trim() ?? "";
        string gender = GenderBox.SelectedItem?.ToString() ?? "";
        string phone = PhoneBox.Text?.Trim() ?? "";
        string email = EmailBox.Text?.Trim() ?? "";
        string position = PositionBox.Text?.Trim() ?? "";
        string status = StatusBox.SelectedItem?.ToString() ?? "Новый";

        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(phone) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(position))
        {
            MessageText.Text =
                "Заполните ФИО, телефон, электронную почту и желаемую должность.";
            return;
        }

        if (!email.Contains('@') ||
            !email.Contains('.') ||
            email.StartsWith("@") ||
            email.EndsWith("@"))
        {
            MessageText.Text = "Проверьте правильность электронной почты.";
            return;
        }

        _allCandidates.Add(new Candidate(
            fullName,
            gender,
            phone,
            email,
            position,
            status));

        ClearForm();
        RefreshCandidates();

        MessageText.Text = "Кандидат успешно добавлен.";
    }

    private void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        if (CandidatesList.SelectedItem is not Candidate selected)
        {
            MessageText.Text = "Сначала выберите кандидата в списке.";
            return;
        }

        _allCandidates.Remove(selected);
        RefreshCandidates();

        MessageText.Text = "Кандидат удалён.";
    }

    private void SearchBox_TextChanged(object? sender, RoutedEventArgs e)
    {
        RefreshCandidates();
    }

    private void FilterStatusBox_SelectionChanged(
        object? sender,
        SelectionChangedEventArgs e)
    {
        RefreshCandidates();
    }

    private void RefreshCandidates()
    {
        string search = SearchBox?.Text?.Trim() ?? "";
        string statusFilter =
            FilterStatusBox?.SelectedItem?.ToString() ?? "Все статусы";

        var filtered = _allCandidates.Where(candidate =>
            (candidate.FullName.Contains(
                 search, StringComparison.OrdinalIgnoreCase) ||
             candidate.Position.Contains(
                 search, StringComparison.OrdinalIgnoreCase)) &&
            (statusFilter == "Все статусы" ||
             candidate.Status == statusFilter));

        _visibleCandidates.Clear();

        foreach (var candidate in filtered)
        {
            _visibleCandidates.Add(candidate);
        }
    }

    private void ClearForm()
    {
        FullNameBox.Text = "";
        PhoneBox.Text = "";
        EmailBox.Text = "";
        PositionBox.Text = "";

        GenderBox.SelectedIndex = 0;
        StatusBox.SelectedIndex = 0;
    }

    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        _mainWindow.Show();
        _mainWindow.Activate();
        Close();
    }
}

public class Candidate
{
    public string FullName { get; }
    public string Gender { get; }
    public string GenderText => $"Пол: {Gender}";
    public string PhoneText => $"Телефон: {Phone}";
    public string EmailText => $"Почта: {Email}";
    public string PositionText => $"Должность: {Position}";
    public string Phone { get; }
    public string Email { get; }
    public string Position { get; }
    public string Status { get; }

    public Candidate(
        string fullName,
        string gender,
        string phone,
        string email,
        string position,
        string status)
    {
        FullName = fullName;
        Gender = gender;
        Phone = phone;
        Email = email;
        Position = position;
        Status = status;
    }
}
