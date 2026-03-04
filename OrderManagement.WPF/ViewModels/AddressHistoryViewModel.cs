using System.Collections.ObjectModel;
using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Features.AddressHistories.Commands.CreateHistory;
using OrderManagement.Application.Features.AddressHistories.Commands.DeleteHistory;
using OrderManagement.Application.Features.AddressHistories.Commands.UpdateHistory;
using OrderManagement.Application.Features.AddressHistories.Queries.GetHistories;
using OrderManagement.WPF.Services;
using OrderManagement.WPF.ViewModels.Base;
using OrderManagement.WPF.ViewModels.Dialogs;

namespace OrderManagement.WPF.ViewModels;

public class AddressHistoryViewModel : ViewModelBase, IParameterReceiver
{
    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    private AddressDto _address = null!;
    public AddressDto Address
    {
        get => _address;
        private set => SetField(ref _address, value);
    }

    private ObservableCollection<AddressHistoryDto> _histories = [];
    public ObservableCollection<AddressHistoryDto> Histories
    {
        get => _histories;
        set => SetField(ref _histories, value);
    }

    private AddressHistoryDto? _selectedHistory;
    public AddressHistoryDto? SelectedHistory
    {
        get => _selectedHistory;
        set => SetField(ref _selectedHistory, value);
    }

    private DateTime? _dateFrom;
    public DateTime? DateFrom
    {
        get => _dateFrom;
        set
        {
            if (SetField(ref _dateFrom, value))
                _ = LoadHistoriesAsync();
        }
    }

    private DateTime? _dateTo;
    public DateTime? DateTo
    {
        get => _dateTo;
        set
        {
            if (SetField(ref _dateTo, value))
                _ = LoadHistoriesAsync();
        }
    }

    private bool _orderByDateDescending = true;
    public bool OrderByDateDescending
    {
        get => _orderByDateDescending;
        set
        {
            if (SetField(ref _orderByDateDescending, value))
                _ = LoadHistoriesAsync();
        }
    }

    public AsyncRelayCommand AddCommand { get; }
    public AsyncRelayCommand EditCommand { get; }
    public AsyncRelayCommand DeleteCommand { get; }
    public RelayCommand GoBackCommand { get; }
    public RelayCommand ToggleSortCommand { get; }
    public RelayCommand ClearFilterCommand { get; }

    public AddressHistoryViewModel(
        IMediator mediator,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _dialogService = dialogService;

        AddCommand = new AsyncRelayCommand(_ => AddHistoryAsync());
        EditCommand = new AsyncRelayCommand(_ => EditHistoryAsync(), _ => SelectedHistory is not null);
        DeleteCommand = new AsyncRelayCommand(_ => DeleteHistoryAsync(), _ => SelectedHistory is not null);
        GoBackCommand = new RelayCommand(_ => _navigationService.GoBack());
        ToggleSortCommand = new RelayCommand(_ => OrderByDateDescending = !OrderByDateDescending);
        ClearFilterCommand = new RelayCommand(_ =>
        {
            DateFrom = null;
            DateTo = null;
        });
    }

    public void ReceiveParameter(object? parameter)
    {
        if (parameter is AddressDto address)
        {
            Address = address;
            _ = LoadHistoriesAsync();
        }
    }

    private async Task LoadHistoriesAsync()
    {
        if (Address is null) return;

        var query = new GetHistoriesQuery(
            Address.Id,
            DateFrom.HasValue ? DateOnly.FromDateTime(DateFrom.Value) : null,
            DateTo.HasValue ? DateOnly.FromDateTime(DateTo.Value) : null,
            OrderByDateDescending);

        var result = await _mediator.Send(query);
        Histories = new ObservableCollection<AddressHistoryDto>(result);
    }

    private async Task AddHistoryAsync()
    {
        var dialog = new HistoryDialogViewModel();
        var result = _dialogService.ShowDialog(dialog);

        if (result == true)
        {
            await _mediator.Send(new CreateHistoryCommand(
                Address.Id,
                DateOnly.FromDateTime(dialog.PumpingDate),
                dialog.CubeAmount,
                dialog.PaymentType,
                dialog.Price));

            await LoadHistoriesAsync();
        }
    }

    private async Task EditHistoryAsync()
    {
        if (SelectedHistory is null) return;

        var dialog = new HistoryDialogViewModel(
            SelectedHistory.PumpingDate.ToDateTime(TimeOnly.MinValue),
            SelectedHistory.CubeAmount,
            SelectedHistory.PaymentType,
            SelectedHistory.Price);

        var result = _dialogService.ShowDialog(dialog);

        if (result == true)
        {
            await _mediator.Send(new UpdateHistoryCommand(
                SelectedHistory.Id,
                DateOnly.FromDateTime(dialog.PumpingDate),
                dialog.CubeAmount,
                dialog.PaymentType,
                dialog.Price));

            await LoadHistoriesAsync();
        }
    }

    private async Task DeleteHistoryAsync()
    {
        if (SelectedHistory is null) return;

        if (!_dialogService.Confirm("Удалить эту запись?"))
            return;

        await _mediator.Send(new DeleteHistoryCommand(SelectedHistory.Id));
        await LoadHistoriesAsync();
    }
}