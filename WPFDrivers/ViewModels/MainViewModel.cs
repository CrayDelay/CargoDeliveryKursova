using System;
using System.Windows.Input;
using WPFDrivers.Commands;
using static WPFDrivers.Commands.NavigateCommand;

namespace WPFDrivers.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ICommand NavigateCommand { get; set; }

    private readonly IServiceProvider _serviceProvider;
    //private readonly IDriversManager _driversManager;

    public MainViewModel(IServiceProvider serviceProvider)//IDriversManager driversManager)
    {
        //_driversManager = driversManager;
        _serviceProvider = serviceProvider;
        NavigateCommand = new NavigateCommand(this, _serviceProvider);
        NavigateCommand.Execute(ViewType.Drivers);
    }

    public ViewModelBase CurrentVM { get; set; }
    public DriversViewModel DriversVM { get; set; }


}
