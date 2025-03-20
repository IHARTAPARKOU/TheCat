using System.Diagnostics;
using System.Windows.Input;
using TheCatApp.Constants;
using TheCatApp.Infrastructure.Services.Abstractions;
using TheCatApp.Models;
using TheCatApp.Presentation.Commands;
using TheCatApp.Presentation.ViewModels.Abstractions;

namespace TheCatApp.Presentation.ViewModels;

class CatBreedDetailsViewModel : BaseViewModel, ICatBreedDetailsViewModel
{
    private readonly IBreedsService breedsService;
    private CatBreed? model;

    public CatBreedDetailsViewModel(IBreedsService breedsService)
    {
        this.breedsService = breedsService;
        OpenLinkCommand = new RelayCommand(OnOpenLinkCommand);
        FavoriteCommand = new RelayCommand(_ => IsFavorite = !isFavorite);
        SaveCommand = new AsyncRelayCommand(OnSave, _ => isDirty);
    }

    #region Properties

    private string? name;
    public string? Name
    {
        get => name;
        set => SetValue(ref name, value);
    }

    private string? description;
    public string? Description
    {
        get => description;
        set
        {
            if (SetValue(ref description, value))
            {
                IsDirty = true;
            }
        }
    }

    private string? origin;
    public string? Origin
    {
        get => origin;
        set => SetValue(ref origin, value);
    }

    private string? lifeSpan;
    public string? LifeSpan
    {
        get => lifeSpan;
        set => SetValue(ref lifeSpan, value);
    }

    private int healthIssues;
    public int HealthIssues
    {
        get => healthIssues;
        set => SetValue(ref healthIssues, value);
    }

    private int intelligence;
    public int Intelligence
    {
        get => intelligence;
        set => SetValue(ref intelligence, value);
    }

    private int sheddingLevel;
    public int SheddingLevel
    {
        get => sheddingLevel;
        set => SetValue(ref sheddingLevel, value);
    }

    private string? photoUrl;
    public string? PhotoUrl
    {
        get => photoUrl;
        set => SetValue(ref photoUrl, value);
    }

    private string? wikipediaUrl;
    public string? WikipediaUrl
    {
        get => wikipediaUrl;
        set => SetValue(ref wikipediaUrl, value);
    }

    private bool isFavorite;
    public bool IsFavorite
    {
        get => isFavorite;
        set
        {
            if (SetValue(ref isFavorite, value))
            {
                IsDirty = true;
            }
        }
    }

    private bool isDirty;
    public bool IsDirty
    {
        get => isDirty;
        set
        {
            if (SetValue(ref isDirty, value))
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
    }

    #endregion

    #region Commands

    public ICommand OpenLinkCommand { get; }

    private void OnOpenLinkCommand()
    {
        if (!string.IsNullOrWhiteSpace(WikipediaUrl))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = WikipediaUrl,
                UseShellExecute = true
            });
        }
    }

    public ICommand FavoriteCommand { get; }

    public AsyncRelayCommand SaveCommand { get; }

    private async Task OnSave()
    {
        using var cts = new CancellationTokenSource(TheCatConstants.DefaultTimeout);
        var toSave = ToModel();
        await breedsService.InsertOrUpdateAsync(toSave, cts.Token);
        UpdateOriginalModel(toSave);
        IsDirty = false;
    }

    #endregion

    private void UpdateOriginalModel(CatBreed model)
    {
        if (this.model is CatBreed original)
        {
            original.Description = model.Description;
            original.IsFavorite = model.IsFavorite;
        }
    }

    public void LoadFromModel(CatBreed model)
    {
        UpdateUI(() =>
        {
            this.model = model;
            Name = model.Name;
            Description = model.Description;
            Origin = model.Origin;
            LifeSpan = model.LifeSpan;
            HealthIssues = model.HealthIssues;
            Intelligence = model.Intelligence;
            SheddingLevel = model.SheddingLevel;
            PhotoUrl = model.PhotoUrl;
            WikipediaUrl = model.WikipediaUrl;
            IsFavorite = model.IsFavorite;
            IsDirty = false;
        });
    }

    public CatBreed ToModel()
    {
        return new CatBreed
        {
            Id = model?.Id,
            Name = Name,
            Description = Description,
            Origin = Origin,
            LifeSpan = LifeSpan,
            HealthIssues = HealthIssues,
            Intelligence = Intelligence,
            SheddingLevel = SheddingLevel,
            PhotoUrl = PhotoUrl,
            WikipediaUrl = WikipediaUrl,
            IsFavorite = IsFavorite
        };
    }
}
