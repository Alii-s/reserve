CREATE MIGRATION m1sxtr3dc4647qzim63glzfz4hgwyryysup3x2n2iyzz7eygpb2dma
    ONTO m1ymmz64p3fzec4gsyefo4f7apkfrnpwkrf3yl76onko67wlykjvpq
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
