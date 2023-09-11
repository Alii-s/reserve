CREATE MIGRATION m1roiym3z5fqmtvvprgmbl2623b3blfjq3bojthwafca5e73aibj3q
    ONTO m13i62ycj7gd6r4gkq6lhzaeehn5mrllxzulsxslspkomuotd6qa7a
{
  ALTER TYPE default::Availability {
      DROP PROPERTY day;
      ALTER PROPERTY end_time {
          SET TYPE std::datetime USING (<std::datetime>{});
      };
      ALTER PROPERTY start_time {
          SET TYPE std::datetime USING (<std::datetime>{});
      };
  };
};
