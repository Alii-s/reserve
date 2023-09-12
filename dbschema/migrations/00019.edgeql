CREATE MIGRATION m1w3g5nsrhmibbgn6ynmcotepehjpoi7vcwv4w4hld63momgj6k2xq
    ONTO m1roiym3z5fqmtvvprgmbl2623b3blfjq3bojthwafca5e73aibj3q
{
  ALTER TYPE default::Availability {
      CREATE REQUIRED PROPERTY available: std::bool {
          SET REQUIRED USING (<std::bool>{true});
      };
  };
};
