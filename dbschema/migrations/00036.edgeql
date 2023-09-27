CREATE MIGRATION m1q66tgdg627inmpdbc2wijv7ynth4eq3ib3ysd3qfnsomft2ya7lq
    ONTO m1rm3ymnphxlqvlwc74w3ujjvdjdhtg723voqtsbtolyowtdpzbpjq
{
  ALTER TYPE default::AppointmentDetails {
      ALTER PROPERTY status {
          RENAME TO appointment_status;
      };
  };
};
