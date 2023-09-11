﻿using EdgeDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Appointment;

public class AppointmentRepository: IAppointmentRepository
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
}

