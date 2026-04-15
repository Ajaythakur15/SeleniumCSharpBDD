namespace SeleniumCSharpBDD.Pages.Modules
{
    public interface IModulePage
    {
        string ModuleName { get; }

        bool IsLoaded();
    }
}
