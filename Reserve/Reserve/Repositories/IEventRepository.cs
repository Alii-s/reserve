using Reserve.Models.Event;

namespace Reserve.Repositories;

public interface IEventRepository
{
    Task<CasualEvent> Create(CasualEvent newEvent);
    Task<CasualEvent?> GetById(string id);
    Task AddReserver(CasualTicket newTicket);
}
