using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Event;

public interface IEventRepository
{
    Task<CasualEventInput?> CreateAsync(CasualEventInput? newEvent);
    Task<CasualEventView?> GetByIdAsync(string id);
    Task<CasualEventInput?> GetByIdForEditAsync(string id);
    Task<CasualTicketInput?> AddReserverAsync(CasualTicketInput? newTicket);
    Task<List<CasualTicketView?>> GetAttendeesAsync(string id);
    Task UpdateAsync(CasualEventInput? editEvent);
    Task CloseReservationAsync(string id);
    Task<CasualEventView?> GetEventFromTicketAsync(string id);
    Task<CasualTicketView?> GetTicketByIdAsync(string id);
    Task CancelReservationAsync(Guid? deletedTicketId, Guid? eventId);
    Task<List<CasualTicketInput?>> CheckIfAlreadyReserved(CasualTicketInput? newTicket);
}
