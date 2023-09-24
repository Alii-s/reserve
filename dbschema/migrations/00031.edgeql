CREATE MIGRATION m1wltdfnvsihglqcuhfsiplilbunn636dgdfbwu4efqtqubiq2nbaq
    ONTO m1x6owoz44um64676pyvgpmrlhztx5yp4ktgqc6ii6jmimivmfhl7a
{
  ALTER TYPE default::RescheduleRequest {
      ALTER LINK original_appointment {
          CREATE CONSTRAINT std::exclusive;
      };
  };
};
