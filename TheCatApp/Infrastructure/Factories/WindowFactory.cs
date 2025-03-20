using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using TheCatApp.Infrastructure.Factories.Abstractions;

namespace TheCatApp.Infrastructure.Factories;

class WindowFactory(IServiceProvider serviceProvider) : IWindowFactory
{
    public T CreateWindow<T>() where T : Window
    {
        return serviceProvider.GetRequiredService<T>();
    }
}
