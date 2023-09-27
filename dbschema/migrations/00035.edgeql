CREATE MIGRATION m1rm3ymnphxlqvlwc74w3ujjvdjdhtg723voqtsbtolyowtdpzbpjq
    ONTO m1ud6j6g2bxrnpgig2c3hgh4mpwnchcnjzdhnto4khyvesg2gdh5nq
{
  CREATE SCALAR TYPE default::AppointmentState EXTENDING enum<Pending, Done>;
  ALTER TYPE default::AppointmentDetails {
      CREATE REQUIRED PROPERTY status: default::AppointmentState {
          SET REQUIRED USING (<default::AppointmentState>{});
      };
  };
};
