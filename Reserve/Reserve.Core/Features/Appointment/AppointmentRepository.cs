using EdgeDB;
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
    public async Task CreateAppointmentCalendar(AppointmentCalendar appointmentCalendar, string availabilitySlots)
    {
        ArgumentNullException.ThrowIfNull(appointmentCalendar);
        EdgeDB.DataTypes.Json jsonAvailabilitySlots = new(availabilitySlots);
        var query = @"with
                        raw_data := <json>$data,
                        for item in json_array_unpack(raw_data) union (
                        insert Availability { 
                        day := <str>item['Day'],
                        start_time := <str>item['StartTime'],
                        end_time := <str>item['EndTime'],
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
            { "id", appointmentCalendar.Id }
        });

    }
    public async Task<AppointmentCalendar?> GetByIdAsync(string Id)
    {
        try
        {
            Guid guidId = Guid.Parse(Id);
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
    public async Task<List<Availability>> GetSlotsFromCalendarIdAsync(string Id)
    {
        try
        {
            Guid guidId = Guid.Parse(Id);
            var query = @"select Availability{
                            id,
                            day,
                            start_time,
                            end_time,
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
    public async Task<AppointmentCalendar> CreateAppointmentInfo(AppointmentCalendar appointmentCalendar)
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
            return new AppointmentCalendar();
        }
    }
    public async Task DeleteAppointmentSlot(string id)
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
}

