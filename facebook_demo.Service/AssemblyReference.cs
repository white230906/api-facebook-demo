using System.Reflection;

namespace facebook_demo.Service;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}