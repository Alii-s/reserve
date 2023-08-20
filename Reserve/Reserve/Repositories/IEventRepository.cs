using Reserve.Models.Event;

namespace Reserve.Repositories;

public interface IEventRepository
{
    Task Create(CasualEvent newEvent);
}
