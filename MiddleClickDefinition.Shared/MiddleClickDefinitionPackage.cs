using MiddleClickDefinition.Shared.Options;
using Microsoft.VisualStudio.Shell;

namespace MiddleClickDefinition
{
    [ProvideBindingPath]        
    [PackageRegistration(UseManagedResourcesOnly=true, AllowsBackgroundLoading = true)]
    [ProvideOptionPage(typeof(OptionsPage), "MiddleClickDefinition", "General", 0, 0, true)]    
    public class MiddleClickDefinitionPackage : AsyncPackage
    {
    }
}
