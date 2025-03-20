using System.Windows;

namespace TheCatApp.Infrastructure.Factories.Abstractions;

internal interface IWindowFactory
{
    T CreateWindow<T>() where T : Window;
}