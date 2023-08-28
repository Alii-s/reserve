using Reserve.Models.Queue;

namespace Reserve.Repositories;

public interface IQueueRepository
{
    Task<QueueEvent> Create(QueueEvent newQueue);
    Task<QueueEvent> GetByID(string id);
    Task<int> GetNextQueueNumber(string queueId);
    Task<QueueTicket> RegisterCustomer(QueueTicket queueTicket);
    Task ServeCurrentCustomer(string queueEventId);
    Task ResetQueue(string queueEventId);
}