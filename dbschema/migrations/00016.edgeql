CREATE MIGRATION m14soowyaultukborhlmcjvoyaxsdznm4cbzczt5raegecblzhbhrq
    ONTO m1t4ew4tsz74p6viko3d7mi5gtsqssbdlo7zyf54vtmzp4m2kae7ja
{
  ALTER TYPE default::Availability {
      ALTER PROPERTY end_time {
          SET TYPE std::str USING (<std::str>.end_time);
      };
      ALTER PROPERTY start_time {
          SET TYPE std::str USING (<std::str>.start_time);
      };
  };
};
