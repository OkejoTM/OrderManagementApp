using System.Collections.ObjectModel;
using MediatR;
using OrderManagement.Application.DTOs;
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

    public AsyncRelayCommand LoadCommand { get; }
    public AsyncRelayCommand AddCommand { get; }
    public AsyncRelayCommand EditCommand { get; }
    public AsyncRelayCommand DeleteCommand { get; }
    public RelayCommand OpenAddressesCommand { get; }

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

        _ = LoadAreasAsync();
    }

    private async Task LoadAreasAsync()
    {
        var result = await _mediator.Send(new GetAreasQuery());
        Areas = new ObservableCollection<AreaDto>(result);
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
}