module default {
    scalar type RescheduleState extending enum<Pending, Accepted, Declined>;
    scalar type AppointmentState extending enum<Pending, Done>;
    type CasualEvent{
        required title: str;
        required organizer_name: str;
        required organizer_email: str;
        required maximum_capacity: int32;
        required location: str;
        required opened: bool;
        required description: str;
        image_url: str;
        required start_date: datetime;
        required end_date: datetime;
        required current_capacity: int32;
    }
    type CasualTicket{
        required reserver_name: str;
        required reserver_email: str;
        required reserver_phone_number: str;
        required casual_event: CasualEvent{
            on target delete delete source;
        }
    constraint exclusive on ((.reserver_email, .casual_event));
    }
    type Availability {
        required start_time: datetime;
        required end_time: datetime;
        required available: bool;
        required appointment_calendar: AppointmentCalendar{
            on target delete delete source;
        }
    }
    type AppointmentDetails{
        required reserver_name: str;
        required reserver_phone_number: str;
        required reserver_email: str;
        required slot: Availability{
            on target delete delete source;
        }
        required appointment_status: AppointmentState;
    }
    type QueueEvent{
        required title: str;
        required organizer_email: str;
        required description: str;
        required current_number_served: int32;
      	required ticket_counter: int32;
        last_reset: datetime;
    }
    type QueueTicket{
        required customer_name: str;
        required customer_phone_number: str;
        required queue_event: QueueEvent{
            on target delete delete source;
        }
        required queue_number: int32;
	    status: str;
    }
    type AppointmentCalendar{
        required name: str;
        required email: str{
            constraint exclusive;
        };
        required description: str;
        multi availability_slots: Availability
    }
    type AppointerNotifications{
        required reserver_name: str;
        required reserver_phone_number: str;
        required reserver_email: str;
        required notification_type: str;
        new_slot: datetime;
        required appointment_calendar: AppointmentCalendar{
            on target delete delete source;
        }
        required slot: Availability{
            on target delete delete source;
        }
    }
    type AppointmentReschedule {
        required original_appointment: AppointmentDetails{
            on target delete delete source;
        }
        required requested_time: Availability{
            on target delete delete source;
        };
        required reschedule_status: RescheduleState;
    }
}