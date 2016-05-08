using System;

namespace PluginService.Infrastructure
{
    /// <summary>
    /// This attribute uses for marking entry point method in plugin assembly.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class EntryPointAttribute : Attribute
    {             
    }
}
