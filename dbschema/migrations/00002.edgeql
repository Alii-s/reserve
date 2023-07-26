CREATE MIGRATION m1qu2qks4aspksummm6g73dtj55lhtor37vnc2ggtxfi7toac6vmka
    ONTO m1d4msuyv3t4tzwld27b4oqkzs3lakc3a2s6chywos2fcsz3vb6cnq
{
  ALTER TYPE default::Appointments {
      DROP LINK appointment_slot;
  };
  ALTER TYPE default::Appointments RENAME TO default::Appointment;
  ALTER TYPE default::Appointment {
      CREATE REQUIRED PROPERTY appointment_slot: std::datetime {
          SET REQUIRED USING (<std::datetime>{});
      };
  };
  ALTER TYPE default::QueueTicket {
      CREATE REQUIRED PROPERTY customer_phone_number: std::str {
          SET REQUIRED USING (<std::str>{});
      };
  };
  ALTER TYPE default::User {
      ALTER PROPERTY national_id {
          RENAME TO phone_number;
      };
  };
};
