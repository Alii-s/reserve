using Reserve.Models.Event;

namespace Reserve.Repositories;

public interface IEventRepository
{
    Task<CasualEvent?> CreateAsync(CasualEvent? newEvent);
    Task<CasualEvent?> GetByIdAsync(string id);
    Task<CasualTicket?> AddReserverAsync(CasualTicket? newTicket);
    Task<List<CasualTicket?>> GetAttendeesAsync(string id);
    Task UpdateAsync(CasualEvent? editEvent);
    Task CloseReservationAsync(string id);
    Task<CasualEvent?> GetEventFromTicketAsync(string id);
    Task<CasualTicket?> GetTicketByIdAsync(string id);
    Task CancelReservationAsync(Guid? deletedTicketId, Guid? eventId);
}
