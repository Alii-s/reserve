CREATE MIGRATION m1izh42qlwvcgf72ezeqlwtlk2tu5s5vhexl6foltxrz3kmrkeu3nq
    ONTO m1w3g5nsrhmibbgn6ynmcotepehjpoi7vcwv4w4hld63momgj6k2xq
{
  ALTER TYPE default::Appointment {
      DROP LINK business;
  };
  ALTER TYPE default::Appointment {
      DROP LINK customer;
  };
  ALTER TYPE default::Appointment {
      CREATE REQUIRED LINK slot: default::Availability {
          ON TARGET DELETE DELETE SOURCE;
          SET REQUIRED USING (<default::Availability>{});
      };
  };
  ALTER TYPE default::Appointment {
      DROP PROPERTY appointment_slot;
  };
  ALTER TYPE default::Appointment {
      CREATE REQUIRED PROPERTY reserver_email: std::str {
          SET REQUIRED USING (<std::str>{''});
      };
  };
  ALTER TYPE default::Appointment {
      CREATE REQUIRED PROPERTY reserver_name: std::str {
          SET REQUIRED USING (<std::str>{''});
      };
      CREATE REQUIRED PROPERTY reserver_phone_number: std::str {
          SET REQUIRED USING (<std::str>{''});
      };
  };
};
