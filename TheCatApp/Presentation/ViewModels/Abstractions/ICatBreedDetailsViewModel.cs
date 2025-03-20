using TheCatApp.Models;

namespace TheCatApp.Presentation.ViewModels.Abstractions;

public interface ICatBreedDetailsViewModel
{
    void LoadFromModel(CatBreed model);
    CatBreed ToModel();
}