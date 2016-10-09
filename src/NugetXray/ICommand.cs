using System;
using System.Threading.Tasks;

namespace NugetXray
{
    public interface ICommand
    {
        bool CanHandle(Type command);
        Task<CommandResult> Execute(object command);
        Type CommandType { get; }
    }
}