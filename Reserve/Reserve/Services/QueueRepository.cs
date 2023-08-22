using Reserve.Repositories;
using EdgeDB;
using Reserve.Models.Queue;
using Reserve.Models.Event;

namespace Reserve.Services;

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
}
