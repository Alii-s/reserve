CREATE MIGRATION m1al2dumkuwbfmdmawy225bdloixgp4uvygkog7oqr5zamonzgtdvq
    ONTO m1ddmnl6w2i4elxcdnemgalkpb45c3b7olmwtevr5ybrx745ea4t6a
{
  CREATE TYPE default::AppointmentCalendar {
      CREATE MULTI LINK availability_slots: default::Availability;
      CREATE REQUIRED PROPERTY description: std::str;
      CREATE REQUIRED PROPERTY email: std::str {
          CREATE CONSTRAINT std::exclusive;
      };
      CREATE REQUIRED PROPERTY name: std::str;
  };
};
