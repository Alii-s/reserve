CREATE MIGRATION m1o4t2nyotp35i7kln4tgxml6m4nvnyzx5pst55vw6qf5lvera6nva
    ONTO m12544pj5pkx2acxz5twndrr6no6bom3gd7odxjg6fhasjjfjicsfa
{
  ALTER TYPE default::AppointerNotifications {
      CREATE PROPERTY new_slot: std::datetime;
  };
  ALTER TYPE default::QueueEvent {
      CREATE PROPERTY last_reset: std::datetime;
  };
};
