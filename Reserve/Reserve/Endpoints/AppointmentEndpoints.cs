using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json.Schema;
using Org.BouncyCastle.Asn1.Ocsp;
using Reserve.Core.Features.Appointment;
using Reserve.Core.Features.Event;
using System.Text.Json;
using System.Text.RegularExpressions;
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
                await _appointmentRepository.CreateAppointmentCalendarAsync(newCalendar, slots);
                if (newCalendar is not null)
                {
                    context.Response.Headers["X-Success-Redirect"] = $"/calendar-creation-notification/{newCalendar.Id}";
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
                Availability deletedSlot = await _appointmentRepository.GetSlotByIdAsync(id);
                if(deletedSlot.Available == false)
                {
                    context.Response.Headers["HX-Redirect"] = $"/upcoming-appointments/{deletedSlot.AppointmentCalendar.Id}";
                    context.Response.Cookies.Append("error", "Can't delete a reserved slot");
                    return Results.BadRequest("Can't delete a reserved slot");
                }
                await _appointmentRepository.DeleteAppointmentSlotAsync(id);
                context.Response.Headers["HX-Redirect"] = $"/upcoming-appointments/{deletedSlot.AppointmentCalendar.Id}";
                context.Response.Cookies.Append("success", "Slot deleted successfully");
                return Results.Ok();
            }
            catch (Exception e)
            {
                context.Response.Cookies.Append("error", "error in deleting slot");
                return Results.BadRequest(e.Message);
            }
        });
        group.MapPost("add-slot/{id}", async ([FromBody] Availability availabilitySlot, string id, HttpContext context, IAppointmentRepository _appointmentRepository, IAntiforgery _antiforgery) =>
        {
            try
            {
                await _antiforgery.ValidateRequestAsync(context);
                availabilitySlot = await _appointmentRepository.AddAppointmentSlotAsync(id, availabilitySlot);
                if (availabilitySlot is null)
                {
                    return Results.BadRequest("Enter values in start date and end date");
                }
                if(availabilitySlot.EndTime < availabilitySlot.StartTime)
                {
                    return Results.BadRequest("End time must be after start time");
                }
                return Results.Ok(availabilitySlot);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        group.MapDelete("cancel-appointment/{id}", async (string id, HttpContext context, IAntiforgery _antiforgery, IAppointmentRepository _appointmentRepository) =>
        {
            try
            {
                await _antiforgery.ValidateRequestAsync(context);
                AppointmentReschedule reschedule = await _appointmentRepository.GetRescheduleByIdAsync(id);
                if(reschedule is not null)
                {
                    context.Response.Headers["HX-Redirect"] = $"/user-dashboard/{id}";
                    context.Response.Cookies.Append("check", "Check notifications before taking this action");
                    return Results.BadRequest("Check notifications before taking this action");
                }
                AppointmentDetails cancelledAppointment = await _appointmentRepository.GetAppointmentDetailsByIdAsync(id);
                await _appointmentRepository.CancelAppointmentAsync(cancelledAppointment);
                context.Response.Headers["HX-Redirect"] = "/event-cancellation";
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        group.MapDelete("delete-notification/{id}", async (string id, HttpContext context, IAntiforgery _antiforgery, IAppointmentRepository _appointmentRepository) =>
        {
            try
            {
                await _antiforgery.ValidateRequestAsync(context);
                await _appointmentRepository.DeleteNotification(id);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        group.MapDelete("finish-appointment/{id}", async (string id, HttpContext context, IAntiforgery _antiforgery, IAppointmentRepository _appointmentRepository) =>
        {
            try
            {
                await _antiforgery.ValidateRequestAsync(context);
                AppointmentDetails finishedAppointment = await _appointmentRepository.FinishAppointment(id);
                context.Response.Headers["HX-Redirect"] = $"/upcoming-appointments/{finishedAppointment.Slot.AppointmentCalendar.Id}";
                context.Response.Cookies.Append("success", "appointment finished successfully");
                return Results.Ok();
            }
            catch (Exception e)
            {
                context.Response.Cookies.Append("check", "error in finishing appointment, try again");
                AppointmentDetails finishedAppointment = await _appointmentRepository.FinishAppointment(id);
                context.Response.Headers["HX-Redirect"] = $"/upcoming-appointments/{finishedAppointment.Slot.AppointmentCalendar.Id}";
                return Results.BadRequest(e.Message);
            }
        });
        group.MapGet("get-appointments/{id}", async (string id, IAppointmentRepository _appointmentRepository) =>
        {
            try
            {
                List<AppointmentDetails> appointments = await _appointmentRepository.GetAppointmentDetailsForCalendarAsync(id);
                return Results.Ok(appointments);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        group.MapGet("free-slots/{id}", async (string id, IAppointmentRepository _appointmentRepository) =>
        {
            try
            {
                List<Availability> availableSlots = await _appointmentRepository.GetFreeSlotsForCalendarView(id);
                return Results.Ok(availableSlots);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
            
        });
        group.MapDelete("delete-request/{id}", async (string id, HttpContext context, IAntiforgery _antiforgery, IAppointmentRepository _appointmentRepository) =>
        {
            try
            {
                await _antiforgery.ValidateRequestAsync(context);
                AppointmentReschedule reschedule = await _appointmentRepository.GetRequestByIdAsync(id);
                if(reschedule is null)
                {
                    return Results.NotFound();
                }
                if(reschedule.RescheduleStatus == RescheduleState.Pending)
                {
                    await _appointmentRepository.DeclineRescheduling(id);
                    await _appointmentRepository.DeleteRequest(id);
                    return Results.Ok();
                }
                await _appointmentRepository.DeleteRequest(id);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        group.MapGet("get-pending-slots", async (IAppointmentRepository _appointmentRepository) =>
        {
            try
            {
                List<Availability> pendingSlots = await _appointmentRepository.GetPendingSlots();
                return Results.Ok(pendingSlots);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        return group;
    }
}
