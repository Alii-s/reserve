CREATE MIGRATION m1th4dggyoka7t4l2qzncjv5yalhr26m6nxffrqb7uugsmp53oo3uq
    ONTO m1qgszs7ccomsk4ugiqhfvt72s2exdes3spvuwgxrtkkzhmjyue3ta
{
  ALTER TYPE default::RescheduleRequest {
      CREATE REQUIRED LINK original_appointment: default::AppointmentDetails {
          SET REQUIRED USING (<default::AppointmentDetails>{});
      };
  };
  ALTER TYPE default::RescheduleRequest {
      DROP PROPERTY requested_time;
  };
  ALTER TYPE default::RescheduleRequest {
      CREATE REQUIRED LINK requested_time: default::Availability {
          SET REQUIRED USING (<default::Availability>{});
      };
      DROP PROPERTY original_appointment_id;
  };
};
