CREATE MIGRATION m1d4msuyv3t4tzwld27b4oqkzs3lakc3a2s6chywos2fcsz3vb6cnq
    ONTO initial
{
  CREATE SCALAR TYPE default::Days EXTENDING enum<Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday>;
  CREATE TYPE default::Availability {
      CREATE REQUIRED PROPERTY day: default::Days;
      CREATE REQUIRED PROPERTY end_time: cal::local_time;
      CREATE REQUIRED PROPERTY start_time: cal::local_time;
  };
  CREATE TYPE default::Business {
      CREATE MULTI LINK availability_slots: default::Availability;
      CREATE REQUIRED PROPERTY description: std::str;
      CREATE REQUIRED PROPERTY email: std::str {
          CREATE CONSTRAINT std::exclusive;
      };
      CREATE REQUIRED PROPERTY name: std::str;
      CREATE REQUIRED PROPERTY password: std::str;
  };
  CREATE SCALAR TYPE default::Gender EXTENDING enum<Male, Female>;
  CREATE TYPE default::User {
      CREATE REQUIRED PROPERTY date_of_birth: std::datetime;
      CREATE REQUIRED PROPERTY email: std::str {
          CREATE CONSTRAINT std::exclusive;
      };
      CREATE REQUIRED PROPERTY gender: default::Gender;
      CREATE REQUIRED PROPERTY name: std::str;
      CREATE REQUIRED PROPERTY national_id: std::str;
      CREATE REQUIRED PROPERTY password: std::str;
  };
  CREATE TYPE default::Appointments {
      CREATE REQUIRED LINK appointment_slot: default::Availability;
      CREATE REQUIRED LINK business: default::Business;
      CREATE REQUIRED LINK customer: default::User;
  };
  CREATE TYPE default::CasualEvent {
      CREATE REQUIRED PROPERTY current_capacity: std::int64;
      CREATE REQUIRED PROPERTY end_date: std::datetime;
      CREATE REQUIRED PROPERTY location: std::str;
      CREATE REQUIRED PROPERTY maximum_capacity: std::int64;
      CREATE REQUIRED PROPERTY organizer_email: std::str;
      CREATE REQUIRED PROPERTY organizer_name: std::str;
      CREATE REQUIRED PROPERTY start_date: std::datetime;
      CREATE PROPERTY tags: array<std::str>;
      CREATE REQUIRED PROPERTY title: std::str;
  };
  CREATE TYPE default::CasualTicket {
      CREATE REQUIRED LINK casual_event: default::CasualEvent {
          ON TARGET DELETE DELETE SOURCE;
      };
      CREATE REQUIRED PROPERTY reserver_email: std::str;
      CREATE REQUIRED PROPERTY reserver_name: std::str;
  };
  CREATE TYPE default::QueueEvent {
      CREATE REQUIRED PROPERTY current_capacity: std::int64;
      CREATE REQUIRED PROPERTY end_date: std::datetime;
      CREATE REQUIRED PROPERTY location: std::str;
      CREATE REQUIRED PROPERTY maximum_capacity: std::int64;
      CREATE REQUIRED PROPERTY organizer_email: std::str;
      CREATE REQUIRED PROPERTY organizer_name: std::str;
      CREATE REQUIRED PROPERTY start_date: std::datetime;
      CREATE PROPERTY tags: array<std::str>;
      CREATE REQUIRED PROPERTY title: std::str;
  };
  CREATE TYPE default::QueueTicket {
      CREATE REQUIRED LINK queue_event: default::QueueEvent {
          ON TARGET DELETE DELETE SOURCE;
      };
      CREATE REQUIRED PROPERTY customer_email: std::str;
      CREATE REQUIRED PROPERTY customer_name: std::str;
      CREATE REQUIRED PROPERTY queue_number: std::int64;
  };
};
