using Reserve.Models.Event;
using Reserve.Models.Queue;

namespace Reserve.Repositories;

public interface IQueueRepository
{
    Task Create(QueueEvent newQueue);
}
