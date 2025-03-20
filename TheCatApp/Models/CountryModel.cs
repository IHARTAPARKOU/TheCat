using TheCatApp.Models.Abstractions;

namespace TheCatApp.Models;

class CountryModel(string name) : BaseModel
{
    public string Name { get; set; } = name;

    private bool isSelected;
    public bool IsSelected
    {
        get => isSelected;
        set => SetValue(ref isSelected, value);
    }
}
