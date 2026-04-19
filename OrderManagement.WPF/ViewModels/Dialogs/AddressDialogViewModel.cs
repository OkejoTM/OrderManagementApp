using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Features.Addresses.Queries.GetAddressSuggestions;
using OrderManagement.WPF.ViewModels.Base;

namespace OrderManagement.WPF.ViewModels.Dialogs;

public class AddressDialogViewModel : ViewModelBase
{
    private readonly IMediator? _mediator;
    private readonly Guid? _areaId;
    private readonly bool _suggestionsEnabled;
    private readonly DispatcherTimer? _debounceTimer;

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set
        {
            if (!SetField(ref _name, value)) return;
            RestartSuggestionDebounce();
        }
    }

    public string Title { get; }
    public bool IsEdit { get; }

    private ObservableCollection<AddressDto> _suggestions = [];
    public ObservableCollection<AddressDto> Suggestions
    {
        get => _suggestions;
        private set => SetField(ref _suggestions, value);
    }

    private bool _hasSuggestions;
    public bool HasSuggestions
    {
        get => _hasSuggestions;
        private set => SetField(ref _hasSuggestions, value);
    }

    public Guid? SelectedSuggestionId { get; private set; }
    public string? SelectedSuggestionName { get; private set; }

    public RelayCommand SaveCommand { get; }
    public RelayCommand CancelCommand { get; }
    public RelayCommand PickSuggestionCommand { get; }

    /// <summary>
    /// Конструктор для режима "добавление" с подсказками.
    /// </summary>
    public AddressDialogViewModel(IMediator mediator, Guid areaId)
        : this(existingName: null)
    {
        _mediator = mediator;
        _areaId = areaId;
        _suggestionsEnabled = true;

        _debounceTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(300)
        };
        _debounceTimer.Tick += async (_, _) =>
        {
            _debounceTimer!.Stop();
            await LoadSuggestionsAsync();
        };
    }

    /// <summary>
    /// Конструктор для режима "редактирование" (или добавление без подсказок).
    /// </summary>
    public AddressDialogViewModel(string? existingName = null)
    {
        IsEdit = existingName is not null;
        Title = IsEdit ? "Редактировать адрес" : "Новый адрес";
        _name = existingName ?? string.Empty;

        SaveCommand = new RelayCommand(Save, _ => !string.IsNullOrWhiteSpace(Name));
        CancelCommand = new RelayCommand(Cancel);
        PickSuggestionCommand = new RelayCommand(PickSuggestion);
    }

    private void RestartSuggestionDebounce()
    {
        if (!_suggestionsEnabled || _debounceTimer is null) return;

        _debounceTimer.Stop();
        _debounceTimer.Start();
    }

    private async Task LoadSuggestionsAsync()
    {
        if (!_suggestionsEnabled || _mediator is null || _areaId is null)
        {
            UpdateSuggestions([]);
            return;
        }

        var query = new GetAddressSuggestionsQuery(_areaId.Value, Name);
        var result = await _mediator.Send(query);
        UpdateSuggestions(result);
    }

    private void UpdateSuggestions(IReadOnlyList<AddressDto> list)
    {
        Suggestions = new ObservableCollection<AddressDto>(list);
        HasSuggestions = Suggestions.Count > 0;
    }

    private void Save(object? parameter)
    {
        if (parameter is Window window)
        {
            window.DialogResult = true;
            window.Close();
        }
    }

    private void Cancel(object? parameter)
    {
        if (parameter is Window window)
        {
            window.DialogResult = false;
            window.Close();
        }
    }

    private void PickSuggestion(object? parameter)
    {
        if (parameter is not object[] args || args.Length != 2) return;
        if (args[0] is not AddressDto dto) return;
        if (args[1] is not Window window) return;

        SelectedSuggestionId = dto.Id;
        SelectedSuggestionName = dto.Name;

        // DialogResult оставляем false — сигнал "переход" несёт SelectedSuggestionId
        window.DialogResult = false;
        window.Close();
    }
}