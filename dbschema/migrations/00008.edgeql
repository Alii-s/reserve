CREATE MIGRATION m1eigwduaknxc3bd3x3aakhmaq344ilmqyudqfvvoqg2e5a2tpvpsa
    ONTO m1x4rdit4vuioqq3uhoyeamu6763qjtup4otssou4wanfp6ticelqa
{
  ALTER TYPE default::CasualEvent {
      DROP PROPERTY tags;
  };
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
      ALTER PROPERTY location {
          RENAME TO description;
      };
  };
  ALTER TYPE default::QueueEvent {
      DROP PROPERTY maximum_capacity;
      DROP PROPERTY organizer_name;
      DROP PROPERTY start_date;
      DROP PROPERTY tags;
  };
  ALTER TYPE default::QueueTicket {
      DROP PROPERTY customer_email;
  };
};
