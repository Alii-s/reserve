﻿using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json.Schema;
using Org.BouncyCastle.Asn1.Ocsp;
using Reserve.Core.Features.Appointment;
using Reserve.Core.Features.Event;
using System.Text.Json;
using static Reserve.Helpers.DateTimeHelper;

namespace Reserve.Endpoints;

public static class AppointmentEndpoints
{
    public static RouteGroupBuilder MapAppointmentsApi(this RouteGroupBuilder group)
    {
        group.MapPost("create-calendar", async ([FromBody] AppointmentCalendar newCalendar, HttpContext context, IAntiforgery antiforgery, IAppointmentRepository _appointmentRepository) =>
        {
            try
            {
                await antiforgery.ValidateRequestAsync(context);
                string slots = JsonSerializer.Serialize(newCalendar.AvailabilitySlots);
                await _appointmentRepository.CreateAppointmentCalendar(newCalendar, slots);
                if (newCalendar is not null)
                {
                    context.Response.Headers["X-Success-Redirect"] = $"/Appointment/CalendarNotification/{newCalendar.Id}";
                    return Results.Ok();
                }
                else
                {
                    return Results.BadRequest();
                }
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        group.MapDelete("delete-slot/{id}", async ([FromRoute] string id, HttpContext context, IAppointmentRepository _appointmentRepository, IAntiforgery _antiforgery) =>
        {
            try
            {
                await _antiforgery.ValidateRequestAsync(context);
                await _appointmentRepository.DeleteAppointmentSlot(id);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        group.MapPost("add-slot/{id}", async ([FromForm] Availability availabilitySlot, string id, HttpContext context, IAppointmentRepository _appointmentRepository, IAntiforgery _antiforgery) =>
        {
            try
            {
                await _antiforgery.ValidateRequestAsync(context);
                availabilitySlot.StartTime = ConvertFrom24To12(availabilitySlot.StartTime);
                availabilitySlot.EndTime = ConvertFrom24To12(availabilitySlot.EndTime);
                availabilitySlot = await _appointmentRepository.AddAppointmentSlot(id, availabilitySlot);
                if (availabilitySlot is null)
                {
                    return Results.BadRequest();
                }
                return Results.Content($"<tr>\r\n                <td>{availabilitySlot.Day}</td>\r\n                <td>{availabilitySlot.StartTime}</td>\r\n                <td>{availabilitySlot.EndTime}</td>\r\n                <td><button hx-delete=\"/delete-slot/{availabilitySlot.Id}\" hx-headers='js:{{\"X-CSRF-TOKEN\": document.getElementsByName(\"__RequestVerificationToken\")[0].value}}' hx-target=\"closest tr\" hx-swap=\"outerHTML swap:1s\" class=\"btn reserve-red-button\">Delete Slot</button></td>\r\n            </tr>", "text/html");
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        return group;
    }
}
