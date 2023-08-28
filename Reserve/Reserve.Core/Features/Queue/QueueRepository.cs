using EdgeDB;

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
                        current_number_served := <int32>$current_number_served
                    }
                  )
                  SELECT Inserted{*};";
        var result = await _client.QuerySingleAsync<QueueEvent>(query, new Dictionary<string, object?>
        {
            {"title", queueEvent.Title },
            {"organizer_email", queueEvent.OrganizerEmail },
            {"description", queueEvent.Description },
            {"current_number_served", queueEvent.CurrentNumberServed }
        });
        return result;
    }

    public async Task<QueueEvent> GetByID(string id)
    {
        Guid guidId = Guid.Parse(id);
        var query = @"SELECT QueueEvent {*} FILTER .id = <uuid>$id;";
        var result = await _client.QuerySingleAsync<QueueEvent>(query, new Dictionary<string, object?>
        {
            {"id", guidId }
        });
        return result;
    }

    public async Task<int> GetNextQueueNumber(string queueId)
    {
        Guid guidId = Guid.Parse(queueId);
        var query = @"WITH QueueEvent := (
                    SELECT QueueEvent FILTER .id = <uuid>$id
                  )
                  SELECT std::sequence_next(introspect QueueNumber);";
        var result = await _client.QuerySingleAsync<int>(query, new Dictionary<string, object?>
        {
            {"id", guidId }
        });
        return result;
    }

    public async Task<QueueTicket> RegisterCustomer(QueueTicket queueTicket)
    {
        Guid guidId = queueTicket.QueueEventId;
        var query = @"WITH Inserted := (
                    INSERT QueueTicket {
                        customer_name := <str>$customer_name,
                        customer_phone_number := <str>$customer_phone_number,
                        queue_number := <int32>$queue_number,
                        queue_event := (SELECT QueueEvent FILTER .id = <uuid>$queueId)
                    }
                  )
                  SELECT Inserted{*};";
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

    public async Task ResetQueue(string queueEventId)
    {
        Guid guidId = Guid.Parse(queueEventId);

        var deleteQuery = @"
        DELETE QueueTicket 
        FILTER .queue_event.id = <uuid>$queueEventId;
    ";
        await _client.ExecuteAsync(deleteQuery, new Dictionary<string, object?>
    {
        {"queueEventId", guidId }
    });

        var updateQuery = @"
        UPDATE QueueEvent 
        SET { current_number_served := 1 } 
        FILTER .id = <uuid>$queueEventId;
    ";
        await _client.ExecuteAsync(updateQuery, new Dictionary<string, object?>
    {
        {"queueEventId", guidId }
    });
    }

}
