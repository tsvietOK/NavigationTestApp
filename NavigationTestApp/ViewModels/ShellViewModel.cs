using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using NavigationTestApp.Contracts.Services;
using NavigationTestApp.Helpers;
using NavigationTestApp.Views;

namespace NavigationTestApp.ViewModels;

public class ShellViewModel : ObservableRecipient
{
    private bool _isBackEnabled;
    private object? _selected;

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }

    public bool IsBackEnabled
    {
        get => _isBackEnabled;
        set => SetProperty(ref _isBackEnabled, value);
    }

    public object? Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public ObservableCollection<NavigationViewItemBase> NavItems
    {
        get; set;
    }

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavItems = new ObservableCollection<NavigationViewItemBase>();
        var mainNavItem = new NavigationViewItem()
        {
            Content = "Main",
            Icon = new SymbolIcon(Symbol.Home),
        };
        NavigationHelper.SetNavigateTo(mainNavItem, typeof(MainViewModel).FullName);

        var contentNavItem = new NavigationViewItem()
        {
            Content = "ContentGrid",
            Icon = new SymbolIcon(Symbol.ViewAll),
        };
        NavigationHelper.SetNavigateTo(contentNavItem, typeof(ContentGridViewModel).FullName);

        NavItems.Add(mainNavItem);
        NavItems.Add(contentNavItem);

        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
}
