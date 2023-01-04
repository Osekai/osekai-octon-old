using System.Linq.Expressions;
using Osekai.Octon.Database.Dtos;

namespace Osekai.Octon.Database.Repositories;

public interface IMedalRepository
{
    public readonly struct MedalFilter
    {
        public MedalFilter(string? name = null)
        {
            Name = name;
        }

        public string? Name { get; }
    } 
    
    Task<IAsyncEnumerable<MedalDto>> GetMedalsAsync(MedalFilter filter = default, int offset  = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default);
}   