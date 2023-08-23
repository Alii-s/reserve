module default {
    scalar type Gender extending enum<Male, Female>;
    scalar type Days extending enum<Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday>;
    type User{
        required name: str;
        required phone_number: str;
        required email: str{
            constraint exclusive;
        };
        required gender: Gender;
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
        tags: array<str>;
        required current_capacity: int32;
    }
    type CasualTicket{
        required reserver_name: str;
        required reserver_email: str;
        required reserver_phone_number: str;
        required casual_event: CasualEvent{
            on target delete delete source;
        }
        constraint expression on (
            .casual_event.current_capacity < .casual_event.maximum_capacity and .casual_event.opened = true
        );
    }
    type Availability {
        required day: Days;
        required start_time: cal::local_time;
        required end_time: cal::local_time;
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
        required organizer_name: str;
        required organizer_email: str;
        required maximum_capacity: int32;
        required location: str;
        required start_date: datetime;
        required end_date: datetime;
        tags: array<str>;
        required current_capacity: int32;
    }
    type QueueTicket{
        required customer_name: str;
        required customer_email: str;
        required customer_phone_number: str;
        required queue_event: QueueEvent{
            on target delete delete source;
        }
        required queue_number: int32;
    }
}
