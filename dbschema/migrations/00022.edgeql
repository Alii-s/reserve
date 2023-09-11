CREATE MIGRATION m1inaittb55ltxvcin3ult4b2z6n7e3qrcpqivoq4yc66yso5giwua
    ONTO m1uwlxy5y4uphmwp7wrdxfvrwno5gjkdtsdpetbq2ccbbdj2zm6naq
{
  CREATE TYPE default::AppointerNotifications {
      CREATE REQUIRED LINK appointment_calendar: default::AppointmentCalendar {
          ON TARGET DELETE DELETE SOURCE;
      };
      CREATE REQUIRED PROPERTY notificarion_type: std::str;
      CREATE REQUIRED PROPERTY reserver_email: std::str;
      CREATE REQUIRED PROPERTY reserver_name: std::str;
      CREATE REQUIRED PROPERTY reserver_phone_number: std::str;
  };
};
