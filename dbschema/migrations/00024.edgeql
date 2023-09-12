CREATE MIGRATION m12544pj5pkx2acxz5twndrr6no6bom3gd7odxjg6fhasjjfjicsfa
    ONTO m15rpnkzr5df7knvccypjkizfvsmeem2gc6jvcrbqdyaoqj6qvp4bq
{
  ALTER TYPE default::AppointerNotifications {
      CREATE REQUIRED LINK slot: default::Availability {
          ON TARGET DELETE DELETE SOURCE;
          SET REQUIRED USING (<default::Availability>{});
      };
  };
};
