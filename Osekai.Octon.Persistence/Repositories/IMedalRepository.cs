using Osekai.Octon.Objects;

namespace Osekai.Octon.Persistence.Repositories;

public interface IMedalRepository
{
    
    public readonly struct MedalFilter
    {
        public enum OrderByEnum
        {
            Id,
            Name
        }
        
        public MedalFilter(string? name = null, OrderByEnum orderBy = OrderByEnum.Id, ICollection<int>? beatmapPackIds = null)
        {
            if (beatmapPackIds is { Count: 0 })
                throw new ArgumentException("The collection is empty (Count = 0)", nameof(beatmapPackIds));  
            
            Name = name;
            OrderBy = orderBy;
        }

        public OrderByEnum OrderBy { get; }
        public string? Name { get; }
    } 
    
    Task<IEnumerable<IReadOnlyMedal>> GetMedalsAsync(
        MedalFilter filter = default, 
        int offset  = 0, 
        int limit = int.MaxValue, 
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<IReadOnlyMedal>> GetMedalsByBeatmapPackIdAsync(int beatmapPackId, CancellationToken cancellationToken = default);
}   