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

    public async Task<CasualEvent> Create(CasualEvent newEvent)
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
                            image_url:=<str>$image_url
                        }
                    )
                    Select Inserted{*};";
        var result = await _client.QuerySingleAsync<CasualEvent>(query, new Dictionary<string, object?>
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
            {"image_url", newEvent.ImageUrl }
        });
        return result;
    }

    public async Task<CasualEvent?> GetById(string id)
    {
        Guid guidId = Guid.Parse(id);
        var query = @"SELECT CasualEvent {*} FILTER .id = <uuid>$id;";
        var result = await _client.QuerySingleAsync<CasualEvent>(query, new Dictionary<string, object?>
        {
            {"id", guidId }
        });
        return result;
    }
}
