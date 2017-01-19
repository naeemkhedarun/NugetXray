using System;
using System.Threading.Tasks;

namespace NugetXray
{
    public interface ICommand
    {
        Task<int> Execute(object command);
        Type CommandType { get; }
    }
}