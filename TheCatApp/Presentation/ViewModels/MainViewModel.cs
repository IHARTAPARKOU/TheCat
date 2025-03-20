using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Input;
using TheCatApp.Constants;
using TheCatApp.Infrastructure.Factories.Abstractions;
using TheCatApp.Infrastructure.Services.Abstractions;
using TheCatApp.Models;
using TheCatApp.Presentation.Commands;
using TheCatApp.Presentation.View;
using TheCatApp.Presentation.ViewModels.Abstractions;

namespace TheCatApp.Presentation.ViewModels;

class MainViewModel : BaseViewModel, IMainViewModel
{
    private readonly IBreedsService breedsService;
    private readonly IWindowFactory windowFactory;
    private readonly ICollectionView filteredCats;
    private readonly Task initialization;
    private CancellationTokenSource? searchCts = null;
    private int currentPage = 0;
    private bool isLoading = false;
    private readonly HashSet<string> selectedCountries = [];

    public MainViewModel(IBreedsService breedsService, IWindowFactory windowFactory)
    {
        this.breedsService = breedsService;
        this.windowFactory = windowFactory;

        ShowDetailsCommand = new RelayCommand(OnShowDetails);

        ICollectionView countries = CollectionViewSource.GetDefaultView(Countries);
        countries.SortDescriptions.Add(new SortDescription(nameof(CountryModel.Name), ListSortDirection.Ascending));

        filteredCats = CollectionViewSource.GetDefaultView(Cats);
        filteredCats.SortDescriptions.Add(new SortDescription(nameof(CatBreed.IsFavorite), ListSortDirection.Descending));
        filteredCats.SortDescriptions.Add(new SortDescription(nameof(CatBreed.Name), ListSortDirection.Ascending));
        filteredCats.Filter = FilterForCats;

        initialization = LoadBreedsAsync();
    }

    #region Properties

    public ObservableCollection<CatBreed> Cats { get; } = [];
    public ObservableCollection<CountryModel> Countries { get; } = [];

    private Regex? filterCatRegex = null;
    private string filterCatQuery = string.Empty;
    public string FilterCatQuery
    {
        get => filterCatQuery;
        set
        {
            if (SetValue(ref filterCatQuery, value))
            {
                if (searchCts is not null)
                {
                    searchCts.Cancel();
                    searchCts.Dispose();
                    searchCts = null;
                }

                filterCatRegex = string.IsNullOrEmpty(filterCatQuery)
                    ? null
                    : new Regex(filterCatQuery, RegexOptions.IgnoreCase);

                filteredCats.Refresh();

                if (filteredCats.IsEmpty)
                {
                    searchCts = new CancellationTokenSource(TheCatConstants.DefaultTimeout);
                    Task.Run(() => TrySearchAsync(filterCatQuery, searchCts.Token), searchCts.Token);
                }
            }
        }
    }

    public bool IsLoading
    {
        get => Volatile.Read(ref isLoading);
        set => Volatile.Write(ref isLoading, value);
    }

    #endregion

    #region Commands

    public ICommand ShowDetailsCommand { get; }

    private void OnShowDetails(object? obj)
    {
        if (obj is CatBreed breed)
        {
            var window = windowFactory.CreateWindow<CatBreedDetailsWindow>();
            if (window.DataContext is ICatBreedDetailsViewModel detailsViewModel)
            {
                detailsViewModel.LoadFromModel(breed);
                window.Show();
            }
        }
    }

    #endregion

    private bool FilterForCats(object item)
    {
        var result = true;

        if (item is not CatBreed cat)
        {
            return result;
        }

        if (selectedCountries.Count > 0)
        {
            result &= string.IsNullOrWhiteSpace(cat.Origin) == false
                && selectedCountries.Contains(cat.Origin);
        }

        if (filterCatRegex is not null)
        {
            result &= string.IsNullOrWhiteSpace(cat.Name) == false
                && filterCatRegex.IsMatch(cat.Name);
        }

        return result;
    }

    private void CountryPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CountryModel.IsSelected)
            && sender is CountryModel country)
        {
            if (country.IsSelected)
            {
                selectedCountries.Add(country.Name);
            }
            else
            {
                selectedCountries.Remove(country.Name);
            }
            filteredCats.Refresh();
        }
    }

    private async Task TrySearchAsync(string name, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        var loadedItems = await breedsService.SearchAsync(name, token);
        await initialization;
        AddNewCats(loadedItems);
    }

    private async Task LoadBreedsAsync()
    {
        using var cts = new CancellationTokenSource(TheCatConstants.DefaultTimeout);
        var loadedItems = await breedsService.GetWithLimitAsync(TheCatConstants.PageLimit, currentPage, cts.Token);
        Interlocked.Increment(ref currentPage);

        AddNewCats(loadedItems);
    }

    public async Task LoadNextPageAsync()
    {
        await initialization;

        if (IsLoading)
        {
            return;
        }

        try
        {
            IsLoading = true;
            await LoadBreedsAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void AddNewCats(IReadOnlyCollection<CatBreed> cats)
    {
        if (cats.Count == 0)
        {
            return;
        }

        UpdateUI(() =>
        {
            var existingCountries = Countries.Select(_ => _.Name).ToHashSet();
            var existingCats = Cats.Where(_ => string.IsNullOrWhiteSpace(_.Id) == false).Select(_ => _.Id).ToHashSet();

            foreach (var cat in cats)
            {
                if (existingCats.Add(cat.Id))
                {
                    Cats.Add(cat);
                    if (cat.Origin is not null && existingCountries.Add(cat.Origin))
                    {
                        var country = new CountryModel(cat.Origin);
                        country.PropertyChanged += CountryPropertyChanged;
                        Countries.Add(country);
                    }
                }
            }
        });
    }
}
