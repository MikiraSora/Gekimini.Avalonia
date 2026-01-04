using System;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Modules.Window.ViewModels;
using Gekimini.Avalonia.Platforms.Services.Miscellaneous;

namespace Gekimini.Avalonia.Modules.Shell.ViewModels.Windows;

public partial class AboutGekiminiWindowViewModel : WindowViewModelBase
{
    public string ProgramCommitId => ThisAssembly.GitCommitId;
    public string ProgramCommitIdShort => ProgramCommitId[..7];
    public string AssemblyVersion => ThisAssembly.AssemblyVersion;
    public string ProductVersion => ThisAssembly.AssemblyFileVersion.Split("+").FirstOrDefault();
    public DateTime ProgramCommitDate => ThisAssembly.GitCommitDate + TimeSpan.FromHours(8);
    public string ProgramBuildConfiguration => ThisAssembly.AssemblyConfiguration;
    public string ProgramBuildTime => BuildInfo.BuildTime;

    [GetServiceLazy]
    private partial IMiscellaneousFeature MiscellaneousFeature { get; }

    [RelayCommand]
    private void OpenURL(string url)
    {
        MiscellaneousFeature.OpenUrl(url);
    }
}