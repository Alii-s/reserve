namespace Reserve.Core.Features.Queue;

public interface IQueueRepository
{
    Task<QueueEvent> Create(QueueEvent newQueue);
    Task<int> GetNextQueueNumber(string queueId);
    Task<QueueTicket> RegisterCustomer(QueueTicket queueTicket);
    Task ServeCurrentCustomer(string queueEventId);
    Task ResetQueue(string queueEventId);
    Task<List<QueueTicket>> GetAttendees(string queueEventId);
    Task IncrementTicketCounter(string queueId);
    Task<QueueEvent> GetQueueEventByID(string id);
    Task<QueueTicket> GetQueueTicketByID(string id);
}