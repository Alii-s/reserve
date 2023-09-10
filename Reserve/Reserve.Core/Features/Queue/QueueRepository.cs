using EdgeDB;
using System;


namespace Reserve.Core.Features.Queue;

public class QueueRepository : IQueueRepository
{
    private readonly EdgeDBClient _client;
    public QueueRepository(EdgeDBClient client)
    {
        _client = client;
    }

    public async Task<QueueEvent> Create(QueueEvent queueEvent)
    {
        var query = @"WITH Inserted := (
                    INSERT QueueEvent {
                        title := <str>$title,
                        organizer_email := <str>$organizer_email,
                        description := <str>$description,
                        current_number_served := <int32>$current_number_served,
                        ticket_counter := <int32>$ticket_counter
                    }
                  )
                  SELECT Inserted{*};";
        var result = await _client.QuerySingleAsync<QueueEvent>(query, new Dictionary<string, object?>
        {
            {"title", queueEvent.Title },
            {"organizer_email", queueEvent.OrganizerEmail },
            {"description", queueEvent.Description },
            {"current_number_served", queueEvent.CurrentNumberServed },
            {"ticket_counter", queueEvent.TicketCounter }
        });
        return result;
    }

    public async Task<QueueEvent> GetQueueEventByID(string id)
    {
        Guid guidId = Guid.Parse(id);
        var query = @"SELECT QueueEvent {*} FILTER .id = <uuid>$id;";
        var results = await _client.QueryAsync<QueueEvent>(query, new Dictionary<string, object?>
        {
            {"id", guidId }
        });
        var result = results.FirstOrDefault();
        return result;
    }

    public async Task<QueueTicket> GetQueueTicketByID(string id)
    {
        Guid guidId = Guid.Parse(id);
        var query = @"SELECT QueueTicket {*} FILTER .id = <uuid>$id;";
        var results = await _client.QueryAsync<QueueTicket>(query, new Dictionary<string, object?>
        {
            {"id", guidId }
        });
        var result = results.FirstOrDefault();
        return result;
    }


    public async Task<int> GetNextQueueNumber(string queueId)
    {
        Guid guidId = Guid.Parse(queueId);
        var query = @"SELECT QueueEvent.ticket_counter + 1
                    FILTER QueueEvent.id = <uuid>$id;";
        var results = await _client.QueryAsync<int>(query, new Dictionary<string, object?>
    {
        {"id", guidId }
    });
        var result = results.FirstOrDefault();
        return result;
    }
    public async Task IncrementTicketCounter(string queueId)
    {
        Guid guidId = Guid.Parse(queueId);
        var query = @"UPDATE QueueEvent
                  FILTER .id = <uuid>$id
                  SET { ticket_counter := .ticket_counter + 1 };";
        await _client.ExecuteAsync(query, new Dictionary<string, object?>
    {
        {"id", guidId }
    });
    }


    public async Task<QueueTicket> RegisterCustomer(QueueTicket queueTicket)
    {
        Guid guidId = queueTicket.QueueEvent.Id;
        var query = @"WITH Inserted := (
                INSERT QueueTicket {
                    customer_name := <str>$customer_name,
                    customer_phone_number := <str>$customer_phone_number,
                    queue_number := <int32>$queue_number,
                    queue_event := (SELECT QueueEvent FILTER .id = <uuid>$queueId)
                }
              )
              SELECT Inserted {
                  id,
                  customer_name,
                  customer_phone_number,
                  queue_number,
                  queue_event: {*}
              };";

        var result = await _client.QuerySingleAsync<QueueTicket>(query, new Dictionary<string, object?>
        {
            {"customer_name", queueTicket.CustomerName },
            {"customer_phone_number", queueTicket.CustomerPhoneNumber },
            {"queue_number", queueTicket.QueueNumber },
            {"queueId", guidId }
        });
        return result;
    }

    public async Task ServeCurrentCustomer(string queueEventId)
    {
        Guid guidId = Guid.Parse(queueEventId);
        var query = @"
        WITH
            QueueEvent := (SELECT QueueEvent FILTER .id = <uuid>$queueEventId),
            CurrentTicket := (SELECT QueueTicket FILTER .queue_event = QueueEvent AND .queue_number = QueueEvent.current_number_served)
        DELETE QueueTicket FILTER .id = CurrentTicket.id;
    ";
        await _client.ExecuteAsync(query, new Dictionary<string, object?>
        {
            {"queueEventId", guidId }
        });
    }

    public async Task<List<QueueTicket>> GetAttendees(string queueEventId)
    {
        Guid guidId = Guid.Parse(queueEventId);
        var query = @"
                    WITH
                        QueueEvent := (SELECT QueueEvent FILTER .id = <uuid>$queueEventId)
                    SELECT QueueTicket {  id,
                 customer_name,
                 customer_phone_number,
                 queue_number,
                 queue_event: {*}
                    } FILTER .queue_event = QueueEvent;";
        var result = await _client.QueryAsync<QueueTicket>(query, new Dictionary<string, object?>
    {
        {"queueEventId", guidId }
    });
        return result.ToList();
    }

    public async Task MarkAsReserved(QueueTicket ticket, int queueNumber)
    {
        var query = @"WITH Updated := (
                    UPDATE QueueEvent
                    FILTER .id = <uuid>$eventId
                    SET {
                        current_number_served := .current_number_served + 1
                    }
                ),
                Deleted := (
                    DELETE QueueTicket
                    FILTER .queue_number = <int32>$queueNumber AND .queue_event.id = <uuid>$eventId
                )
                SELECT Updated {
                    id,
                    title,
                    organizer_email,
                    description,
                    current_number_served,
                    ticket_counter
                };";

        var result = await _client.QuerySingleAsync<QueueEvent>(query, new Dictionary<string, object?>
    {
        {"queueNumber", queueNumber },
        {"eventId", ticket.QueueEvent.Id }
    });
    }

    public async Task ResetQueue(string queueEventId)
    {
        Guid guidId = Guid.Parse(queueEventId);
        var updatequery = @"    UPDATE QueueEvent
                        FILTER .id = <uuid>$eventId
                        SET {
                            current_number_served := 1,
                            ticket_counter := 0
                        };";

        await _client.QueryAsync<QueueEvent>(updatequery, new Dictionary<string, object?>
        {
            {"eventId", guidId}
        });
        var deletequery = @"    DELETE QueueTicket
               FILTER .queue_event.id = <uuid>$eventId;";
;

        await _client.QueryAsync<QueueEvent>(deletequery, new Dictionary<string, object?>
        {
            {"eventId", guidId}
        });
    }


}
