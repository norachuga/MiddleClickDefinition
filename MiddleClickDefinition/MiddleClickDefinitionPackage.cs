using Microsoft.VisualStudio.Shell;

namespace MiddleClickDefinition
{
    [ProvideBindingPath]        
    [PackageRegistration(UseManagedResourcesOnly=true)]
    [ProvideOptionPage(typeof(OptionsPage), "MiddleClickDefinition", "General", 0, 0, true)]    
    public class MiddleClickDefinitionPackage : Package
    {
    }
}
