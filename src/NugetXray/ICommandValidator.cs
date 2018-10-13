using System.Collections.Generic;

namespace NugetXray
{
    public interface ICommandValidator
    {
        IEnumerable<string> GetErrors();
    }
}