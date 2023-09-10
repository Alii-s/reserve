module default {
    type User{
        required name: str;
        required phone_number: str;
        required email: str{
            constraint exclusive;
        };
        required gender: str;
        required date_of_birth: datetime;
        required password: str;
    }
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
        required day: str;
        required start_time: str;
        required end_time: str;
        required appointment_calendar: AppointmentCalendar{
            on target delete delete source;
        }
    }
    type Business{
        required name: str;
        required email: str{
            constraint exclusive;
        };
        required password: str;
        required description: str;
        multi availability_slots: Availability
    }
    type Appointment{
        required customer: User;
        required business: Business;
        required appointment_slot: datetime;
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
    }
    type AppointmentCalendar{
        required name: str;
        required email: str{
            constraint exclusive;
        };
        required description: str;
        multi availability_slots: Availability
    }
}