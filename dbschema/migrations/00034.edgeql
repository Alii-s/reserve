CREATE MIGRATION m1ud6j6g2bxrnpgig2c3hgh4mpwnchcnjzdhnto4khyvesg2gdh5nq
    ONTO m1i4x6b6hzewtgtbtkgnumwots7lz63uadeacn6mmraenabwaj2k3a
{
  ALTER TYPE default::AppointmentReschedule {
      ALTER LINK original_appointment {
          DROP CONSTRAINT std::exclusive;
      };
  };
};
