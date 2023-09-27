CREATE MIGRATION m1x6owoz44um64676pyvgpmrlhztx5yp4ktgqc6ii6jmimivmfhl7a
    ONTO m1pdnbuvzwqxvdllvxf5v2erdywiphco5bdwmrhgexzuwatkagwhbq
{
  DROP TYPE default::Business;
  ALTER TYPE default::RescheduleRequest {
      DROP PROPERTY is_accepted;
  };
  CREATE SCALAR TYPE default::RescheduleState EXTENDING enum<Pending, Accepted, Rejected>;
  ALTER TYPE default::RescheduleRequest {
      CREATE REQUIRED PROPERTY reschedule_status: default::RescheduleState {
          SET REQUIRED USING (<default::RescheduleState>{});
      };
  };
  DROP TYPE default::User;
};
