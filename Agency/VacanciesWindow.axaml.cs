
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Agency;

public partial class VacanciesWindow : Window
{
    private readonly Window _mainWindow;

    private readonly ObservableCollection<Vacancy> _allVacancies = new();
    private readonly ObservableCollection<Vacancy> _visibleVacancies = new();

    public VacanciesWindow(Window mainWindow)
    {
        InitializeComponent();

        _mainWindow = mainWindow;

        VacanciesList.ItemsSource = _visibleVacancies;

        FilterStatusBox.ItemsSource = new[]
        {
            "Все статусы",
            "Открыта",
            "Приостановлена",
            "Закрыта"
        };

        FilterStatusBox.SelectedIndex = 0;

        StatusBox.ItemsSource = new[]
        {
            "Открыта",
            "Приостановлена",
            "Закрыта"
        };

        StatusBox.SelectedIndex = 0;

        // Демонстрационные записи
        _allVacancies.Add(new Vacancy(
            "Менеджер по продажам",
            "ООО «Перспектива»",
            80000,
            "5/2, 9:00–18:00",
            "Москва",
            "Опыт продаж, навыки общения",
            "Открыта"));

        _allVacancies.Add(new Vacancy(
            "Бухгалтер",
            "ООО «Финанс»",
            70000,
            "5/2, 9:00–18:00",
            "Москва",
            "Знание бухгалтерского учёта",
            "Открыта"));

        _allVacancies.Add(new Vacancy(
            "Офис-менеджер",
            "ООО «Вектор»",
            55000,
            "5/2, 10:00–19:00",
            "Санкт-Петербург",
            "Уверенное владение компьютером",
            "Приостановлена"));

        RefreshVacancies();
    }

    private void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        string title = TitleBox.Text?.Trim() ?? "";
        string company = CompanyBox.Text?.Trim() ?? "";
        string rateText = RateBox.Text?.Trim() ?? "";
        string schedule = ScheduleBox.Text?.Trim() ?? "";
        string city = CityBox.Text?.Trim() ?? "";
        string requirements = RequirementsBox.Text?.Trim() ?? "";
        string status = StatusBox.SelectedItem?.ToString() ?? "Открыта";

        if (string.IsNullOrWhiteSpace(title) ||
            string.IsNullOrWhiteSpace(company) ||
            string.IsNullOrWhiteSpace(rateText) ||
            string.IsNullOrWhiteSpace(schedule) ||
            string.IsNullOrWhiteSpace(city))
        {
            MessageText.Text = "Заполните название должности, компанию, ставку, график и город.";
            return;
        }

        string normalizedRate = rateText.Replace(',', '.');

        bool validRate = decimal.TryParse(
            normalizedRate,
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out decimal rate);

        if (!validRate || rate <= 0)
        {
            MessageText.Text = "Введите корректную ставку больше нуля.";
            return;
        }

        var vacancy = new Vacancy(
            title,
            company,
            rate,
            schedule,
            city,
            string.IsNullOrWhiteSpace(requirements)
                ? "Не указаны"
                : requirements,
            status);

        _allVacancies.Add(vacancy);

        ClearForm();
        RefreshVacancies();

        MessageText.Text = "Вакансия успешно добавлена.";
    }

    private void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        if (VacanciesList.SelectedItem is not Vacancy selectedVacancy)
        {
            MessageText.Text = "Сначала выберите вакансию в списке.";
            return;
        }

        _allVacancies.Remove(selectedVacancy);

        RefreshVacancies();

        MessageText.Text = "Вакансия удалена.";
    }

    private void SearchBox_TextChanged(object? sender, RoutedEventArgs e)
    {
        RefreshVacancies();
    }

    private void FilterStatusBox_SelectionChanged(
        object? sender,
        SelectionChangedEventArgs e)
    {
        RefreshVacancies();
    }

    private void RefreshVacancies()
    {
        string search = SearchBox?.Text?.Trim() ?? "";
        string statusFilter = FilterStatusBox?.SelectedItem?.ToString()
                              ?? "Все статусы";

        var filtered = _allVacancies.Where(v =>
            (v.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
             v.Company.Contains(search, StringComparison.OrdinalIgnoreCase)) &&
            (statusFilter == "Все статусы" || v.Status == statusFilter));

        _visibleVacancies.Clear();

        foreach (var vacancy in filtered)
        {
            _visibleVacancies.Add(vacancy);
        }
    }

    private void ClearForm()
    {
        TitleBox.Text = "";
        CompanyBox.Text = "";
        RateBox.Text = "";
        ScheduleBox.Text = "";
        CityBox.Text = "";
        RequirementsBox.Text = "";
        StatusBox.SelectedIndex = 0;
    }

    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        _mainWindow.Show();
        _mainWindow.Activate();
        Close();
    }
}

public class Vacancy
{
    public string Title { get; }
    public string Company { get; }
    public decimal Rate { get; }
    public string RateText => $"Ставка: {Rate:N0} руб./мес.";
    public string Schedule { get; }
    public string City { get; }
    public string Requirements { get; }
    public string Status { get; }

    public Vacancy(
        string title,
        string company,
        decimal rate,
        string schedule,
        string city,
        string requirements,
        string status)
    {
        Title = title;
        Company = company;
        Rate = rate;
        Schedule = $"График: {schedule}";
        City = $"Город: {city}";
        Requirements = $"Требования: {requirements}";
        Status = status;
    }
}
