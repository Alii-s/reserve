using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Reserve.Core.Features.Event;
using Reserve.Repositories;

namespace Reserve.Endpoints;

public static class EventEndpoints
{
    public static RouteGroupBuilder MapEventsApi(this RouteGroupBuilder group)
    {
        group.MapPost("close-reservation/{id}", async ([FromRoute] string id, HttpContext context, IEventRepository _eventRepository, IAntiforgery antiforgery) =>
        {
            try
            {
                await antiforgery.ValidateRequestAsync(context);
                await _eventRepository.CloseReservationAsync(id);
                var html = @"<p class=""notification-text m-3"">Reservation closed!</p>";
                return Results.Content(html, "text/html");
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        group.MapDelete("cancel-reservation", async(HttpContext context, IEventRepository _eventRepository, IAntiforgery antiforgery) =>
        {
            try
            {
                await antiforgery.ValidateRequestAsync(context);
                return Results.RedirectToRoute("/Event/CancelNotification");
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        return group;
    }
}
