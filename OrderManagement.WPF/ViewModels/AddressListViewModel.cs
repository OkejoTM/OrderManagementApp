using System.Collections.ObjectModel;
using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Features.Addresses.Commands.CreateAddress;
using OrderManagement.Application.Features.Addresses.Commands.DeleteAddress;
using OrderManagement.Application.Features.Addresses.Commands.UpdateAddress;
using OrderManagement.Application.Features.Addresses.Queries.GetAddresses;
using OrderManagement.WPF.Services;
using OrderManagement.WPF.ViewModels.Base;
using OrderManagement.WPF.ViewModels.Dialogs;

namespace OrderManagement.WPF.ViewModels;

public class AddressListViewModel : ViewModelBase, IParameterReceiver
{
    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    private const int PageSize = 9;

    private AreaDto _area = null!;
    public AreaDto Area
    {
        get => _area;
        private set => SetField(ref _area, value);
    }

    private ObservableCollection<AddressDto> _addresses = [];
    public ObservableCollection<AddressDto> Addresses
    {
        get => _addresses;
        set => SetField(ref _addresses, value);
    }

    private AddressDto? _selectedAddress;
    public AddressDto? SelectedAddress
    {
        get => _selectedAddress;
        set => SetField(ref _selectedAddress, value);
    }

    private string _nameFilter = string.Empty;
    public string NameFilter
    {
        get => _nameFilter;
        set
        {
            if (SetField(ref _nameFilter, value))
            {
                _currentPage = 1;
                OnPropertyChanged(nameof(CurrentPage));
                _ = LoadAddressesAsync();
            }
        }
    }

    private bool _orderByNameDescending;
    public bool OrderByNameDescending
    {
        get => _orderByNameDescending;
        set
        {
            if (SetField(ref _orderByNameDescending, value))
                _ = LoadAddressesAsync();
        }
    }

    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        private set => SetField(ref _currentPage, value);
    }

    private int _totalPages = 1;
    public int TotalPages
    {
        get => _totalPages;
        private set => SetField(ref _totalPages, value);
    }

    private int _totalCount;
    public int TotalCount
    {
        get => _totalCount;
        private set => SetField(ref _totalCount, value);
    }

    public AsyncRelayCommand LoadCommand { get; }
    public AsyncRelayCommand AddCommand { get; }
    public AsyncRelayCommand EditCommand { get; }
    public AsyncRelayCommand DeleteCommand { get; }
    public RelayCommand OpenHistoryCommand { get; }
    public RelayCommand ToggleSortCommand { get; }
    public RelayCommand GoBackCommand { get; }
    public AsyncRelayCommand NextPageCommand { get; }
    public AsyncRelayCommand PrevPageCommand { get; }

    public AddressListViewModel(
        IMediator mediator,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _dialogService = dialogService;

        LoadCommand = new AsyncRelayCommand(_ => LoadAddressesAsync());
        AddCommand = new AsyncRelayCommand(_ => AddAddressAsync());
        EditCommand = new AsyncRelayCommand(_ => EditAddressAsync(), _ => SelectedAddress is not null);
        DeleteCommand = new AsyncRelayCommand(_ => DeleteAddressAsync(), _ => SelectedAddress is not null);
        OpenHistoryCommand = new RelayCommand(_ => OpenHistory(), _ => SelectedAddress is not null);
        ToggleSortCommand = new RelayCommand(_ => OrderByNameDescending = !OrderByNameDescending);
        GoBackCommand = new RelayCommand(_ => _navigationService.GoBack());
        NextPageCommand = new AsyncRelayCommand(_ => GoToNextPageAsync(), _ => CurrentPage < TotalPages);
        PrevPageCommand = new AsyncRelayCommand(_ => GoToPrevPageAsync(), _ => CurrentPage > 1);
    }

    public void ReceiveParameter(object? parameter)
    {
        if (parameter is AreaDto area)
        {
            Area = area;
            _ = LoadAddressesAsync();
        }
    }

    private async Task LoadAddressesAsync()
    {
        if (Area is null) return;

        var query = new GetAddressesQuery(
            Area.Id,
            string.IsNullOrWhiteSpace(NameFilter) ? null : NameFilter,
            OrderByNameDescending,
            CurrentPage,
            PageSize);

        var result = await _mediator.Send(query);
        Addresses = new ObservableCollection<AddressDto>(result.Items);
        TotalCount = result.TotalCount;
        TotalPages = result.TotalPages == 0 ? 1 : result.TotalPages;

        if (CurrentPage > TotalPages)
        {
            CurrentPage = TotalPages;
            await LoadAddressesAsync();
        }
    }

    private async Task GoToNextPageAsync()
    {
        CurrentPage++;
        await LoadAddressesAsync();
    }

    private async Task GoToPrevPageAsync()
    {
        CurrentPage--;
        await LoadAddressesAsync();
    }

    private async Task AddAddressAsync()
    {
        var dialog = new AddressDialogViewModel();
        var result = _dialogService.ShowDialog(dialog);

        if (result == true)
        {
            await _mediator.Send(new CreateAddressCommand(Area.Id, dialog.Name));
            await LoadAddressesAsync();
        }
    }

    private async Task EditAddressAsync()
    {
        if (SelectedAddress is null) return;

        var dialog = new AddressDialogViewModel(SelectedAddress.Name);
        var result = _dialogService.ShowDialog(dialog);

        if (result == true)
        {
            await _mediator.Send(new UpdateAddressCommand(SelectedAddress.Id, dialog.Name));
            await LoadAddressesAsync();
        }
    }

    private async Task DeleteAddressAsync()
    {
        if (SelectedAddress is null) return;

        if (!_dialogService.Confirm($"Удалить адрес \"{SelectedAddress.Name}\" и всю его историю?"))
            return;

        await _mediator.Send(new DeleteAddressCommand(SelectedAddress.Id));
        await LoadAddressesAsync();
    }

    private void OpenHistory()
    {
        if (SelectedAddress is null) return;
        _navigationService.NavigateTo<AddressHistoryViewModel>(SelectedAddress);
    }
}