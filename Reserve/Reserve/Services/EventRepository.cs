using Reserve.Repositories;
using EdgeDB;
using Reserve.Models.Event;

namespace Reserve.Services;

public class EventRepository : IEventRepository
{
    private readonly EdgeDBClient _client;
    public EventRepository(EdgeDBClient client)
    {
        _client = client;     
    }

    public async Task<CasualEvent?> CreateAsync(CasualEvent? newEvent)
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
                            tags:= <array<str>>$tags,
                            current_capacity:= <int32>$current_capacity,
                            description:= <str>$description,
                            opened:= <bool>$opened,
                            image_url:=<str>$image_url
                        }
                    )
                    Select Inserted{*};";
        var result = await _client.QuerySingleAsync<CasualEvent?>(query, new Dictionary<string, object?>
        {
            {"title", newEvent.Title },
            {"organizer_name", newEvent.OrganizerName },
            {"organizer_email", newEvent.OrganizerEmail },
            {"maximum_capacity", newEvent.MaximumCapacity },
            {"location", newEvent.Location },
            {"start_date", newEvent.StartDate },
            {"end_date", newEvent.EndDate },
            {"tags", newEvent.Tags },
            {"current_capacity", newEvent.CurrentCapacity },
            {"description", newEvent.Description },
            {"opened", true },
            {"image_url", newEvent.ImageUrl }
        });
        return result;
    }

    public async Task<CasualEvent?> GetByIdAsync(string id)
    {
        Guid guidId = Guid.Parse(id);
        var query = @"SELECT CasualEvent {*} FILTER .id = <uuid>$id;";
        var result = await _client.QuerySingleAsync<CasualEvent?>(query, new Dictionary<string, object?>
        {
            {"id", guidId }
        });
        return result;
    }

    public async Task<CasualTicket?> AddReserverAsync(CasualTicket? newTicket)
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

            return await tx.QuerySingleAsync<CasualTicket?>(@"with inserted :=(
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
    public async Task<List<CasualTicket?>> GetAttendeesAsync(string id)
    {
        Guid guidId = Guid.Parse(id);
        var query = @"SELECT CasualTicket {
            reserver_name,
            reserver_email,
            reserver_phone_number
        } FILTER .casual_event.id = <uuid>$id;";
        return (await _client.QueryAsync<CasualTicket?>(query, new Dictionary<string, object?>
        {
            {"id", guidId }
        })).ToList();
    }
    public async Task UpdateAsync(CasualEvent? editEvent)
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
                tags:= <array<str>>$tags,
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
            {"tags",editEvent.Tags },
            {"current_capacity",editEvent.CurrentCapacity },
            {"description",editEvent.Description },
            {"image_url",editEvent.ImageUrl }
        });
    }

    public async Task CloseReservationAsync(string id)
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

    public async Task<CasualEvent?> GetEventFromTicketAsync(string id)
    {
        Guid guidId = Guid.Parse(id);
        var query = @"SELECT CasualTicket {
                        casual_event: {*}
                    }
                    FILTER .id = <uuid>$id;";
        var result = await _client.QuerySingleAsync<CasualEvent?>(query, new Dictionary<string, object?>
        {
            {"id", guidId }
        });
        return result;
    }

    public async Task<CasualTicket?> GetTicketByIdAsync(string id)
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
                            tags,
                            current_capacity,
                            description,
                            opened,
                            image_url
                        }
                    } FILTER CasualTicket.id = <uuid>$id;";
        var result = await _client.QuerySingleAsync<CasualTicket?>(query, new Dictionary<string, object?>
        {
            {"id", guidId }
        });
        return result;
    }

    public async Task CancelReservationAsync(Guid? deletedTicketId, Guid? eventId)
    {
        await _client.TransactionAsync(async (tx) =>
        {
            var query = @"DELETE CasualTicket
                        FILTER .id = <uuid>$id;";
            await _client.ExecuteAsync(query, new Dictionary<string, object?>
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
}
