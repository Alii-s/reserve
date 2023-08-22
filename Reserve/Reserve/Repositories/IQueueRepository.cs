using Reserve.Models.Event;
using Reserve.Models.Queue;

namespace Reserve.Repositories;

public interface IQueueRepository
{
    Task<QueueEvent> Create(QueueEvent newQueue);
    Task<QueueEvent> GetByID(string id);
}