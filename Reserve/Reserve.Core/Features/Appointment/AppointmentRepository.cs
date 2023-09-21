using EdgeDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Appointment;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly EdgeDBClient _client;
    public AppointmentRepository(EdgeDBClient client)
    {
        _client = client;
    }
    public async Task CreateAppointmentCalendarAsync(AppointmentCalendar appointmentCalendar, string availabilitySlots)
    {
        ArgumentNullException.ThrowIfNull(appointmentCalendar);
        EdgeDB.DataTypes.Json jsonAvailabilitySlots = new(availabilitySlots);
        var query = @"with
                        raw_data := <json>$data,
                        for item in json_array_unpack(raw_data) union (
                        insert Availability { 
                        start_time := <datetime>item['StartTime'],
                        end_time := <datetime>item['EndTime'],
                        available := <bool>$available,
                        appointment_calendar := (
                            select AppointmentCalendar
                            filter .id = <uuid>$id
                            limit 1
                        )
                        }
                    );";
        await _client.ExecuteAsync(query, new Dictionary<string, object?>
        {
            { "data", jsonAvailabilitySlots },
            { "id", appointmentCalendar.Id },
            {"available", true }
        });

    }
    public async Task<AppointmentCalendar?> GetByIdAsync(string id)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"select AppointmentCalendar{*}
                        filter .id = <uuid>$id;";
            return await _client.QuerySingleAsync<AppointmentCalendar?>(query, new Dictionary<string, object?>
            {
                { "id", guidId }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task<List<AppointmentReschedule>> GetReschedulesByIdAsync(string id)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"SELECT RescheduleRequest { id, is_accepted, requested_time: {id,
                            start_time,
                            end_time,
                            available,
                            appointment_calendar: {
                                id,
                                name,
                                email,
                                description
                            }   
                        }, original_appointment: {*}
                         }
                        FILTER .original_appointment.id = <uuid>$id;
;";
            return (await _client.QueryAsync<AppointmentReschedule>(query, new Dictionary<string, object?>
        {
            { "id", guidId }
        })).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task DeleteAppointmentReschedule(string id)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"DELETE RescheduleRequest
FILTER .id = <uuid>$id;";
            await _client.ExecuteAsync(query, new Dictionary<string, object?>
            {
                {"id", guidId }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task<AppointmentDetails> GetAppointmentDetailsByAvailabilityId(string id)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"select AppointmentDetails{*}
                        filter .slot.id =<uuid>$id limit 1;";
            return await _client.QuerySingleAsync<AppointmentDetails?>(query, new Dictionary<string, object?>
            {
                { "id", guidId }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }


    public async Task<List<Availability>> GetSlotsFromCalendarIdAsync(string id)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"select Availability{
                            id,
                            start_time,
                            end_time,
                            available,
                            appointment_calendar: {
                                id,
                                name,
                                email,
                                description
                            }   
                          }
                        filter .appointment_calendar.id = <uuid>$id;";
            return (await _client.QueryAsync<Availability>(query, new Dictionary<string, object?>
            {
                { "id", guidId }
            })).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    public async Task<AppointmentCalendar> CreateAppointmentInfoAsync(AppointmentCalendar appointmentCalendar)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(appointmentCalendar);
            var query = @"With Inserted := (
                        INSERT AppointmentCalendar {
                            name:= <str>$name,
                            email:= <str>$email,
                            description:= <str>$description
                        }
                    )
                    Select Inserted{*};";
            appointmentCalendar = await _client.QuerySingleAsync<AppointmentCalendar>(query, new Dictionary<string, object?>
            {
                {"name", appointmentCalendar.Name },
                { "email", appointmentCalendar.Email },
                { "description", appointmentCalendar.Description }
            });
            return appointmentCalendar;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    public async Task DeleteAppointmentSlotAsync(string id)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"DELETE Availability
                        FILTER .id = <uuid>$id;";
            await _client.ExecuteAsync(query, new Dictionary<string, object?>
            {
                {"id", guidId }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task CreateAppointmentReschedule(AppointmentReschedule rescheduleRequest)
    {
        try
        {
            var query = @"
            INSERT RescheduleRequest {
                original_appointment := (SELECT AppointmentDetails FILTER .id = <uuid>$originalAppointmentId),
                requested_time := (SELECT Availability FILTER .id = <uuid>$requestedTimeId),
                is_accepted := <bool>$isAccepted
            }
        ";

            await _client.ExecuteAsync(query, new Dictionary<string, object?>
        {
            { "originalAppointmentId", rescheduleRequest.OriginalAppointment.Id },
            { "requestedTimeId", rescheduleRequest.RequestedTime.Id },
            { "isAccepted", rescheduleRequest.IsAccepted }
        });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }


    public async Task<Availability> AddAppointmentSlotAsync(string id, Availability newSlot)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(newSlot);
            Guid guidId = Guid.Parse(id);
            var query = @"with Inserted := (
                            INSERT Availability {
                                start_time := <datetime>$start_time,
                                end_time := <datetime>$end_time,
                                available := <bool>$available,
                                appointment_calendar := (
                                    select AppointmentCalendar
                                    filter .id = <uuid>$id
                                    limit 1
                                )
                            }
                        )
                        Select Inserted{*};";
            return await _client.QuerySingleAsync<Availability>(query, new Dictionary<string, object?>
            {
                {"start_time", newSlot.StartTime },
                {"end_time", newSlot.EndTime },
                {"id", guidId },
                {"available", true }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task<List<Availability>> GetOpenSlotsAsync(string id)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"SELECT Availability {
                              start_time,
                              end_time,
                              available,
                              appointment_calendar
                            }
                            FILTER
                              .available = true AND
                              .appointment_calendar.id = <uuid>$id;";
            return (await _client.QueryAsync<Availability>(query, new Dictionary<string, object?>
            {
                {"id", guidId }
            })).ToList();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    public async Task<Availability> GetSlotByIdAsync(string id)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"SELECT Availability {
                              id, 
                              start_time,
                              end_time,
                              available,
                              appointment_calendar : {
                                id,
                                name,
                                email,
                                description
                              }
                            }
                            FILTER
                              Availability.id = <uuid>$id;";
            return await _client.QuerySingleAsync<Availability>(query, new Dictionary<string, object?>
            {
                {"id", guidId }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task<AppointmentDetails> CreateAppointmentMeetingAsync(AppointmentDetails appointmentDetails)
    {
        ArgumentNullException.ThrowIfNull(appointmentDetails);
        try
        {
            await _client.TransactionAsync(async (tx) =>
            {
                var query = @"UPDATE Availability
                            FILTER .id = <uuid>$id
                            SET {
                                available := false
                            };";
                await tx.ExecuteAsync(query, new Dictionary<string, object?>
                {
                    {"id", appointmentDetails.Slot.Id }
                });
                var query2 = @"With Inserted := (
                        INSERT AppointmentDetails {
                            reserver_name:= <str>$name,
                            reserver_email:= <str>$email,
                            reserver_phone_number:= <str>$reserver_phone_number,
                            slot := (
                                select Availability
                                filter .id = <uuid>$id
                                limit 1
                            )
                        }
                    )
                    Select Inserted{*};";
                appointmentDetails = await tx.QuerySingleAsync<AppointmentDetails>(query2, new Dictionary<string, object?>
                {
                    {"name", appointmentDetails.ReserverName },
                    { "email", appointmentDetails.ReserverEmail },
                    { "reserver_phone_number", appointmentDetails.ReserverPhoneNumber },
                    { "id", appointmentDetails.Slot.Id }
                });
                
            });
            return appointmentDetails;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    public async Task<List<AppointmentDetails>> GetAppointmentDetailsForCalendarAsync(string id)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"select AppointmentDetails {
                            id,
                            reserver_name,
                            reserver_phone_number,
                            reserver_email,
                            slot: {
                                id,
                                start_time,
                                end_time,
                                available,
                                appointment_calendar: {
                                    id,
                                    name,
                                    email,
                                    description
                                }
                            }
                        } filter .slot.appointment_calendar.id = <uuid>$id and .slot.available = false;";
            return (await _client.QueryAsync<AppointmentDetails>(query, new Dictionary<string, object?>
            {
                { "id", guidId },
            })).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    public async Task<AppointmentDetails> GetAppointmentDetailsByIdAsync(string id)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"select AppointmentDetails {
                        id,
                        reserver_name,
                        reserver_phone_number,
                        reserver_email,
                        slot: {
                            id,
                            start_time,
                            end_time,
                            available,
                            appointment_calendar: {
                                id,
                                name,
                                email,
                                description
                            }
                        }
                    } filter .id = <uuid>$id;";
            return await _client.QuerySingleAsync<AppointmentDetails>(query, new Dictionary<string, object?>
            {
                {"id", guidId },
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

    }
    public async Task CancelAppointmentAsync(AppointmentDetails cancelledAppointment)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(cancelledAppointment);
            await _client.TransactionAsync(async (tx) =>
            {
                var query = @"delete AppointmentDetails
                            filter.id = <uuid>$id;";
                await tx.ExecuteAsync(query, new Dictionary<string, object?>
                {
                    {"id", cancelledAppointment.Id }
                });
                var query2 = @"UPDATE Availability
                            FILTER .id = <uuid>$id
                            SET {
                                available := true
                            };";
                await tx.ExecuteAsync(query2, new Dictionary<string, object?>
                {
                    {"id", cancelledAppointment.Slot.Id }
                });
                var query3 = @"insert AppointerNotifications {
                                    reserver_name := <str>$reserver_name,
                                    reserver_phone_number := <str>$reserver_phone_number,
                                    reserver_email := <str>$reserver_email,
                                    notification_type := 'Cancellation',
                                    slot := (select Availability filter .id = <uuid>$slotId),               
                                    appointment_calendar := (select AppointmentCalendar filter .id = <uuid>$id)
                                };";
                await tx.ExecuteAsync(query3, new Dictionary<string, object?>
                {
                    {"reserver_name", cancelledAppointment.ReserverName },
                    {"reserver_phone_number", cancelledAppointment.ReserverPhoneNumber },
                    {"reserver_email", cancelledAppointment.ReserverEmail },
                    {"id", cancelledAppointment.Slot.AppointmentCalendar.Id },
                    {"slotId", cancelledAppointment.Slot.Id }
                });
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    public async Task<List<AppointerNotifications>> GetAppointmentNotificationsForCalendarAsync(string id)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"select AppointerNotifications {
                            id,
                            reserver_name,
                            reserver_phone_number,
                            reserver_email,
                            notification_type,
                            slot: {
                                id,
                                start_time,
                                end_time,
                                available
                            },
                            appointment_calendar: {
                                id,
                                name,
                                email,
                                description
                            }
                        } filter .appointment_calendar.id = <uuid>$id;";
            return (await _client.QueryAsync<AppointerNotifications>(query, new Dictionary<string, object?>
            {
                { "id", guidId }
            })).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    public async Task DeleteNotification(string id)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"Delete AppointerNotifications
                          filter .id = <uuid>$id";
            await _client.ExecuteAsync(query, new Dictionary<string, object?>
            {
                {"id", guidId }
            });
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    public async Task<List<Availability>> GetFreeSlotsOfCalendarByIdAsync(AppointmentDetails appointment)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(appointment);
            var query = @"select Availability {
                            start_time
                        } filter .appointment_calendar.id = <uuid>$id and .available = true;";
            return (await _client.QueryAsync<Availability>(query, new Dictionary<string, object?>
            {
                {"id", appointment.Slot.AppointmentCalendar.Id }
            })).ToList();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    public async Task Reschedule(AppointmentDetails appointmentDetails, DateTime newSlot)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(appointmentDetails);
            await _client.TransactionAsync(async (tx) =>
            {
                var query = @"insert AppointerNotifications {
                                    reserver_name := <str>$reserver_name,
                                    reserver_phone_number := <str>$reserver_phone_number,
                                    reserver_email := <str>$reserver_email,
                                    notification_type := 'Rescheduling',
                                    new_slot := <datetime>$new_slot,
                                    slot := (select Availability filter .id = <uuid>$slotId),               
                                    appointment_calendar := (select AppointmentCalendar filter .id = <uuid>$id)
                                };";
                await tx.ExecuteAsync(query, new Dictionary<string, object?>
                {
                    {"reserver_name", appointmentDetails.ReserverName },
                    {"reserver_phone_number", appointmentDetails.ReserverPhoneNumber },
                    {"reserver_email", appointmentDetails.ReserverEmail },
                    {"id", appointmentDetails.Slot.AppointmentCalendar.Id },
                    {"slotId", appointmentDetails.Slot.Id },
                    {"new_slot", newSlot }
                });
                var query2 = @"UPDATE Availability
                            FILTER .id = <uuid>$id
                            SET {
                                available := true
                            };";
                await tx.ExecuteAsync(query2, new Dictionary<string, object?>
                {
                    {"id", appointmentDetails.Slot.Id }
                });
                var query3 = @"WITH first_to_update := (
                                SELECT Availability
                                FILTER .start_time = <datetime>$new_slot AND .appointment_calendar.id = <uuid>$id
                                LIMIT 1
                            ),
                            updated := (
                                UPDATE Availability
                                FILTER .id = (SELECT first_to_update.id)
                                SET {
                                    available := false
                                }
                            )
                            SELECT updated { * };";
                Availability newDate = await tx.QuerySingleAsync<Availability>(query3, new Dictionary<string, object?>
                {
                    {"new_slot", newSlot },
                    {"id", appointmentDetails.Slot.AppointmentCalendar.Id }
                });
                var query4 = @"UPDATE AppointmentDetails
                                FILTER .id = <uuid>$id
                                SET {
                                    slot := (
                                        SELECT Availability
                                        FILTER .id = <uuid>$new_slot_id
                                    )
                                }";
                await tx.ExecuteAsync(query4, new Dictionary<string, object?>
                {
                    {"id", appointmentDetails.Id },
                    {"new_slot_id", newDate.Id }
                });
            });         
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    public async Task FinishAppointment(string id)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            var query = @"WITH
                            deleted_appointment := (
                                DELETE AppointmentDetails
                                FILTER .id = <uuid>$id
                            ),
                            deleted_availability := (
                                DELETE Availability
                                FILTER .id = (SELECT deleted_appointment.slot.id)
                            )
                            SELECT deleted_appointment { * }";
            await _client.ExecuteAsync(query, new Dictionary<string, object?>
            {
                {"id", guidId }
            });
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    public async Task<AppointmentCalendar> GetCalendarFromEmail(string email)
    {
        var query = @"select AppointmentCalendar {
                        id,
                        name,
                        email,
                        description
                    } filter .email = <str>$email;";
        return await _client.QuerySingleAsync<AppointmentCalendar>(query, new Dictionary<string, object?>
        {
            {"email", email }
        });
    }
}

