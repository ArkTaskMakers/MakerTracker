namespace MakerTracker
{
    using System;

    /// <summary>
    ///     Attribute class for decorating classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TypeScriptModelAttribute : Attribute
    {
    }
}
