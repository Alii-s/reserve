CREATE MIGRATION m1ymmz64p3fzec4gsyefo4f7apkfrnpwkrf3yl76onko67wlykjvpq
    ONTO m1al2dumkuwbfmdmawy225bdloixgp4uvygkog7oqr5zamonzgtdvq
{
  ALTER TYPE default::Availability {
      ALTER PROPERTY day {
          SET TYPE std::str USING ('');
      };
  };
  ALTER TYPE default::User {
      ALTER PROPERTY gender {
          SET TYPE std::str USING ('');
      };
  };
  DROP SCALAR TYPE default::Days;
  DROP SCALAR TYPE default::Gender;
};
