CREATE MIGRATION m1e4t4kgwvgqejk2ejscqcatnjhwh6iiv256di6s3qrhbnmf72ygna
    ONTO m12544pj5pkx2acxz5twndrr6no6bom3gd7odxjg6fhasjjfjicsfa
{
  ALTER TYPE default::QueueEvent {
      CREATE PROPERTY last_reset: std::datetime;
  };
  ALTER TYPE default::QueueTicket {
      CREATE PROPERTY status: std::str;
  };
};
