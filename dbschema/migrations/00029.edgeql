CREATE MIGRATION m1pdnbuvzwqxvdllvxf5v2erdywiphco5bdwmrhgexzuwatkagwhbq
    ONTO m1th4dggyoka7t4l2qzncjv5yalhr26m6nxffrqb7uugsmp53oo3uq
{
  ALTER TYPE default::RescheduleRequest {
      ALTER LINK original_appointment {
          ON TARGET DELETE DELETE SOURCE;
      };
      ALTER LINK requested_time {
          ON TARGET DELETE DELETE SOURCE;
      };
  };
};
