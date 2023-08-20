CREATE MIGRATION m17txr7lfwhbl6hczveph3hhrhoky3i3q7rks3n2fpzgznu2ptgzza
    ONTO m177zwoqvffweirp2uocovqsecuarubee2a4edremmeoz2iit4mdpa
{
  ALTER TYPE default::QueueEvent {
      ALTER PROPERTY current_capacity {
          RENAME TO current_number_served;
      };
  };
  ALTER TYPE default::QueueEvent {
      DROP PROPERTY end_date;
  };
  ALTER TYPE default::QueueEvent {
      DROP PROPERTY location;
  };
  ALTER TYPE default::QueueEvent {
      DROP PROPERTY maximum_capacity;
  };
  ALTER TYPE default::QueueEvent {
      CREATE REQUIRED PROPERTY description: std::str {
          SET REQUIRED USING (<std::str>{});
      };
      DROP PROPERTY organizer_name;
  };
  ALTER TYPE default::QueueEvent {
      DROP PROPERTY start_date;
      DROP PROPERTY tags;
  };
  ALTER TYPE default::QueueTicket {
      DROP PROPERTY customer_email;
  };
};
