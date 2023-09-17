using EdgeDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Reserve.Core.Features.Event;

public class EventRepository : IEventRepository
{
    private readonly EdgeDBClient _client;
    public EventRepository(EdgeDBClient client)
    {
        _client = client;
    }

    public async Task<CasualEventInput?> CreateAsync(CasualEventInput? newEvent)
    {
        ArgumentNullException.ThrowIfNull(newEvent);
        try
        {
            var query = @"With Inserted := (
                        INSERT CasualEvent {
                            title:= <str>$title,
                            organizer_name:= <str>$organizer_name,
                            organizer_email:= <str>$organizer_email,
                            maximum_capacity:= <int32>$maximum_capacity,
                            location:= <str>$location,
                            start_date:=<datetime>$start_date,
                            end_date:=<datetime>$end_date,
                            current_capacity:= <int32>$current_capacity,
                            description:= <str>$description,
                            opened:= <bool>$opened,
                            image_url:=<str>$image_url
                        }
                    )
                    Select Inserted{*};";
            var result = await _client.QuerySingleAsync<CasualEventInput?>(query, new Dictionary<string, object?>
            {
                {"title", newEvent?.Title },
                {"organizer_name", newEvent?.OrganizerName },
                {"organizer_email", newEvent?.OrganizerEmail },
                {"maximum_capacity", newEvent?.MaximumCapacity },
                {"location", newEvent?.Location },
                {"start_date", newEvent?.StartDate },
                {"end_date", newEvent?.EndDate },
                {"current_capacity", 0 },
                {"description", newEvent?.Description },
                {"opened", true },
                {"image_url", newEvent?.ImageUrl ?? "" }
            });
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        } 
    }

    public async Task<CasualEventView?> GetByIdAsync(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"SELECT CasualEvent {*} FILTER .id = <uuid>$id;";
            var result = await _client.QuerySingleAsync<CasualEventView?>(query, new Dictionary<string, object?>
        {
            {"id", guidId }
        });
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    public async Task<CasualEventInput?> GetByIdForEditAsync(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"SELECT CasualEvent {*} FILTER .id = <uuid>$id;";
            var result = await _client.QuerySingleAsync<CasualEventInput?>(query, new Dictionary<string, object?>
        {
            {"id", guidId }
        });
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task<CasualTicketInput?> AddReserverAsync(CasualTicketInput? newTicket)
    {
        ArgumentNullException.ThrowIfNull(newTicket);
        ArgumentNullException.ThrowIfNull(newTicket.CasualEvent);
        try
        {
            var result = await _client.TransactionAsync(async (tx) =>
            {
                await tx.ExecuteAsync(@"UPDATE CasualEvent
                FILTER .id = <uuid>$casual_event
                SET {
                    current_capacity := .current_capacity + 1
                };", new Dictionary<string, object?>
                {
                {"casual_event", newTicket.CasualEvent.Id }
                });

                return await tx.QuerySingleAsync<CasualTicketInput?>(@"with inserted :=(
                INSERT CasualTicket {
                    reserver_name:= <str>$reserver_name,
                    reserver_email:= <str>$reserver_email,
                    reserver_phone_number:= <str>$reserver_phone_number,
                    casual_event:= (
                        SELECT CasualEvent
                        FILTER .id = <uuid>$casual_event
                        limit 1
                    )   
                }
            )
            Select inserted{*};", new Dictionary<string, object?>
                {
                    {"reserver_name", newTicket.ReserverName },
                    {"reserver_email", newTicket.ReserverEmail },
                    {"reserver_phone_number", newTicket.ReserverPhoneNumber },
                    {"casual_event", newTicket.CasualEvent!.Id }
                });
            });
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    public async Task<List<CasualTicketView?>> GetAttendeesAsync(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"SELECT CasualTicket {
            reserver_name,
            reserver_email,
            reserver_phone_number,
            casual_event: {
                            id,
                            title,
                            organizer_name,
                            organizer_email,
                            maximum_capacity,
                            location,
                            start_date,
                            end_date,
                            current_capacity,
                            description,
                            opened,
                            image_url
                        }
            } FILTER .casual_event.id = <uuid>$id;";
            return (await _client.QueryAsync<CasualTicketView?>(query, new Dictionary<string, object?>
                {
                    {"id", guidId }
                })).ToList();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return null!;
        }
    }
    public async Task UpdateAsync(CasualEventInput? editEvent)
    {
        ArgumentNullException.ThrowIfNull(editEvent);
        try
        {
            var query = @"UPDATE CasualEvent
            FILTER .id = <uuid>$id
            SET {
                title:= <str>$title,
                organizer_name:= <str>$organizer_name,
                organizer_email:= <str>$organizer_email,
                maximum_capacity:= <int32>$maximum_capacity,
                location:= <str>$location,
                start_date:=<datetime>$start_date,
                end_date:=<datetime>$end_date,
                current_capacity:= <int32>$current_capacity,
                description:= <str>$description,
                image_url:=<str>$image_url
            };";
            await _client.ExecuteAsync(query, new Dictionary<string, object?>
            {
                {"id", editEvent.Id },
                {"title",editEvent.Title },
                {"organizer_name",editEvent.OrganizerName },
                {"organizer_email",editEvent.OrganizerEmail },
                {"maximum_capacity",editEvent.MaximumCapacity },
                {"location",editEvent.Location },
                {"start_date",editEvent.StartDate },
                {"end_date",editEvent.EndDate },
                {"current_capacity",editEvent.CurrentCapacity },
                {"description",editEvent.Description },
                {"image_url",editEvent.ImageUrl }
            });
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task CloseReservationAsync(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"UPDATE CasualEvent
            FILTER .id = <uuid>$id
            SET {
                opened:= false
            };";
            await _client.ExecuteAsync(query, new Dictionary<string, object?>
            {
                {"id", guidId }
            });
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task<CasualEventView?> GetEventFromTicketAsync(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"SELECT CasualTicket {
                        casual_event: {
                            id,
                            title,
                            organizer_name,
                            organizer_email,
                            maximum_capacity,
                            location,
                            start_date,
                            end_date,
                            current_capacity,
                            description,
                            opened,
                            image_url
                        }
                    }
                    FILTER .id = <uuid>$id;";
            var result = await _client.QuerySingleAsync<CasualEventView?>(query, new Dictionary<string, object?>
            {
                {"id", guidId }
            });
            return result;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task<CasualTicketView?> GetTicketByIdAsync(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"SELECT CasualTicket {
                        reserver_name,
                        reserver_email,
                        reserver_phone_number,
                        casual_event: {
                            id,
                            title,
                            organizer_name,
                            organizer_email,
                            maximum_capacity,
                            location,
                            start_date,
                            end_date,
                            current_capacity,
                            description,
                            opened,
                            image_url
                        }
                    } FILTER CasualTicket.id = <uuid>$id;";
            var result = await _client.QuerySingleAsync<CasualTicketView?>(query, new Dictionary<string, object?>
            {
                {"id", guidId }
            });
            return result;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task CancelReservationAsync(Guid? deletedTicketId, Guid? eventId)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(deletedTicketId);
            ArgumentNullException.ThrowIfNull(eventId);
            await _client.TransactionAsync(async (tx) =>
            {
                var query = @"DELETE CasualTicket
                        FILTER .id = <uuid>$id;";
                await tx.ExecuteAsync(query, new Dictionary<string, object?>
                {
                {"id", deletedTicketId }
                });

                await tx.ExecuteAsync(@"UPDATE CasualEvent
                FILTER .id = <uuid>$casual_event
                SET {
                    current_capacity := .current_capacity - 1
                };", new Dictionary<string, object?>
                    {
                        {"casual_event", eventId}
                    });
            });
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    public async Task<List<CasualTicketInput?>> CheckIfAlreadyReserved(CasualTicketInput? newTicket)
    {
        ArgumentNullException.ThrowIfNull(newTicket);
        ArgumentNullException.ThrowIfNull(newTicket.CasualEvent);
        var query = @"SELECT CasualTicket{
            reserver_name,
            reserver_email,
            reserver_phone_number,
            casual_event: {
                id,
                title,
                organizer_name,
                organizer_email,
                maximum_capacity,
                location,
                start_date,
                end_date,
                current_capacity,
                description,
                opened,
                image_url
            }
        } FILTER .casual_event.id = <uuid>$casual_event and .reserver_email = <str>$reserver_email;";
        try
        {
            return (await _client.QueryAsync<CasualTicketInput?>(query, new Dictionary<string, object?>
            {
                {"casual_event", newTicket.CasualEvent.Id },
                {"reserver_email", newTicket.ReserverEmail }
            })).ToList();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return null!;
        }
    }
    public async Task<List<CasualEventView>> GetAllEvents()
    {
        var query = @"SELECT CasualEvent{*};";
        try
        {
            return (await _client.QueryAsync<CasualEventView>(query)).ToList();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return null!;
        }
    }
}
