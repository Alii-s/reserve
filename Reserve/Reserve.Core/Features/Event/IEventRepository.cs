using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Event;

public interface IEventRepository
{
    Task<CasualEvent?> CreateAsync(CasualEvent? newEvent);
    Task<CasualEvent?> GetByIdAsync(string id);
    Task<CasualEvent?> GetByIdForEditAsync(string id);
    Task<CasualTicket?> AddReserverAsync(CasualTicket? newTicket);
    Task<List<CasualTicket?>> GetAttendeesAsync(string id);
    Task UpdateAsync(CasualEvent? editEvent);
    Task CloseReservationAsync(string id);
    Task<CasualEvent?> GetEventFromTicketAsync(string id);
    Task<CasualTicket?> GetTicketByIdAsync(string id);
    Task CancelReservationAsync(Guid? deletedTicketId, Guid? eventId);
    Task<List<CasualTicket?>> CheckIfAlreadyReserved(CasualTicket? newTicket);
    Task<List<CasualEvent>> GetAllEvents();
}
