CREATE MIGRATION m1wxvxuyxd6sdu5r5n4dbepucgka5ryhutdrv63cwfgoijkum7aaia
    ONTO m1x4rdit4vuioqq3uhoyeamu6763qjtup4otssou4wanfp6ticelqa
{
  ALTER TYPE default::CasualTicket {
      CREATE CONSTRAINT std::exclusive ON ((.reserver_email, .casual_event));
  };
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
