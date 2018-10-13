using System;
using System.Threading.Tasks;

namespace NugetXray
{
    public interface ICommand
    {
        Type CommandType { get; }
        Task<int> Execute(object command);
    }
}