using System.Collections.ObjectModel;
using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Features.Addresses.Queries.SearchAddresses;
using OrderManagement.Application.Features.Areas.Commands.CreateArea;
using OrderManagement.Application.Features.Areas.Commands.DeleteArea;
using OrderManagement.Application.Features.Areas.Commands.UpdateArea;
using OrderManagement.Application.Features.Areas.Queries.GetAreas;
using OrderManagement.WPF.Services;
using OrderManagement.WPF.ViewModels.Base;
using OrderManagement.WPF.ViewModels.Dialogs;

namespace OrderManagement.WPF.ViewModels;

public class AreaListViewModel : ViewModelBase
{
    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;

    private const int SearchPageSize = 8;

    private ObservableCollection<AreaDto> _areas = [];
    public ObservableCollection<AreaDto> Areas
    {
        get => _areas;
        set => SetField(ref _areas, value);
    }

    private AreaDto? _selectedArea;
    public AreaDto? SelectedArea
    {
        get => _selectedArea;
        set => SetField(ref _selectedArea, value);
    }

    private string _searchFilter = string.Empty;
    public string SearchFilter
    {
        get => _searchFilter;
        set
        {
            if (!SetField(ref _searchFilter, value)) return;

            var nowSearchMode = !string.IsNullOrWhiteSpace(value);
            if (nowSearchMode != IsSearchMode)
            {
                IsSearchMode = nowSearchMode;
            }

            _searchPage = 1;
            OnPropertyChanged(nameof(SearchPage));

            if (IsSearchMode)
            {
                _ = LoadSearchResultsAsync();
            }
            else
            {
                SearchResults = [];
                SearchTotalCount = 0;
                SearchTotalPages = 1;
            }
        }
    }

    private bool _isSearchMode;
    public bool IsSearchMode
    {
        get => _isSearchMode;
        private set => SetField(ref _isSearchMode, value);
    }

    private ObservableCollection<AddressSearchResultDto> _searchResults = [];
    public ObservableCollection<AddressSearchResultDto> SearchResults
    {
        get => _searchResults;
        set => SetField(ref _searchResults, value);
    }

    private AddressSearchResultDto? _selectedSearchResult;
    public AddressSearchResultDto? SelectedSearchResult
    {
        get => _selectedSearchResult;
        set => SetField(ref _selectedSearchResult, value);
    }

    private int _searchPage = 1;
    public int SearchPage
    {
        get => _searchPage;
        private set => SetField(ref _searchPage, value);
    }

    private int _searchTotalPages = 1;
    public int SearchTotalPages
    {
        get => _searchTotalPages;
        private set => SetField(ref _searchTotalPages, value);
    }

    private int _searchTotalCount;
    public int SearchTotalCount
    {
        get => _searchTotalCount;
        private set => SetField(ref _searchTotalCount, value);
    }

    public AsyncRelayCommand LoadCommand { get; }
    public AsyncRelayCommand AddCommand { get; }
    public AsyncRelayCommand EditCommand { get; }
    public AsyncRelayCommand DeleteCommand { get; }
    public RelayCommand OpenAddressesCommand { get; }
    public RelayCommand OpenSearchResultCommand { get; }
    public RelayCommand ClearSearchCommand { get; }
    public AsyncRelayCommand NextSearchPageCommand { get; }
    public AsyncRelayCommand PrevSearchPageCommand { get; }

    public AreaListViewModel(
        IMediator mediator,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _dialogService = dialogService;

        LoadCommand = new AsyncRelayCommand(_ => LoadAreasAsync());
        AddCommand = new AsyncRelayCommand(_ => AddAreaAsync());
        EditCommand = new AsyncRelayCommand(_ => EditAreaAsync(), _ => SelectedArea is not null);
        DeleteCommand = new AsyncRelayCommand(_ => DeleteAreaAsync(), _ => SelectedArea is not null);
        OpenAddressesCommand = new RelayCommand(_ => OpenAddresses(), _ => SelectedArea is not null);

        OpenSearchResultCommand = new RelayCommand(
            OpenSearchResult,
            p => (p as AddressSearchResultDto ?? SelectedSearchResult) is not null);

        ClearSearchCommand = new RelayCommand(_ => SearchFilter = string.Empty);

        NextSearchPageCommand = new AsyncRelayCommand(
            _ => GoToNextSearchPageAsync(),
            _ => SearchPage < SearchTotalPages);

        PrevSearchPageCommand = new AsyncRelayCommand(
            _ => GoToPrevSearchPageAsync(),
            _ => SearchPage > 1);

        _ = LoadAreasAsync();
    }

    private async Task LoadAreasAsync()
    {
        var result = await _mediator.Send(new GetAreasQuery());
        Areas = new ObservableCollection<AreaDto>(result);
    }

    private async Task LoadSearchResultsAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchFilter))
        {
            SearchResults = [];
            SearchTotalCount = 0;
            SearchTotalPages = 1;
            return;
        }

        var query = new SearchAddressesQuery(SearchFilter, SearchPage, SearchPageSize);
        var result = await _mediator.Send(query);

        SearchResults = new ObservableCollection<AddressSearchResultDto>(result.Items);
        SearchTotalCount = result.TotalCount;
        SearchTotalPages = result.TotalPages == 0 ? 1 : result.TotalPages;

        if (SearchPage > SearchTotalPages)
        {
            SearchPage = SearchTotalPages;
            await LoadSearchResultsAsync();
        }
    }

    private async Task GoToNextSearchPageAsync()
    {
        SearchPage++;
        await LoadSearchResultsAsync();
    }

    private async Task GoToPrevSearchPageAsync()
    {
        SearchPage--;
        await LoadSearchResultsAsync();
    }

    private async Task AddAreaAsync()
    {
        var dialog = new AreaDialogViewModel();
        var result = _dialogService.ShowDialog(dialog);

        if (result == true)
        {
            await _mediator.Send(new CreateAreaCommand(dialog.Name));
            await LoadAreasAsync();
        }
    }

    private async Task EditAreaAsync()
    {
        if (SelectedArea is null) return;

        var dialog = new AreaDialogViewModel(SelectedArea.Name);
        var result = _dialogService.ShowDialog(dialog);

        if (result == true)
        {
            await _mediator.Send(new UpdateAreaCommand(SelectedArea.Id, dialog.Name));
            await LoadAreasAsync();
        }
    }

    private async Task DeleteAreaAsync()
    {
        if (SelectedArea is null) return;

        if (!_dialogService.Confirm($"Удалить район \"{SelectedArea.Name}\" и все его адреса?"))
            return;

        await _mediator.Send(new DeleteAreaCommand(SelectedArea.Id));
        await LoadAreasAsync();
    }

    private void OpenAddresses()
    {
        if (SelectedArea is null) return;
        _navigationService.NavigateTo<AddressListViewModel>(SelectedArea);
    }

    private void OpenSearchResult(object? parameter)
    {
        var target = parameter as AddressSearchResultDto ?? SelectedSearchResult;
        if (target is null) return;

        var addressDto = new AddressDto
        {
            Id = target.AddressId,
            Name = target.AddressName
        };

        _navigationService.NavigateTo<AddressHistoryViewModel>(addressDto);
    }
}