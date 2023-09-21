CREATE MIGRATION m1qgszs7ccomsk4ugiqhfvt72s2exdes3spvuwgxrtkkzhmjyue3ta
    ONTO m1l3imtoztyi6g4l6zew56cpbyhdy2h4pqnffprqcxyz4wts7u44da
{
  CREATE TYPE default::RescheduleRequest {
      CREATE REQUIRED PROPERTY is_accepted: std::bool;
      CREATE REQUIRED PROPERTY original_appointment_id: std::str;
      CREATE REQUIRED PROPERTY requested_time: std::datetime;
  };
};
