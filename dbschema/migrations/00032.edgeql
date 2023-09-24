CREATE MIGRATION m1btsns3fx2cve2zbkwqjyvvnmlyzx5rj6tkst4uxwu47gvapitgpq
    ONTO m1wltdfnvsihglqcuhfsiplilbunn636dgdfbwu4efqtqubiq2nbaq
{
  ALTER TYPE default::RescheduleRequest RENAME TO default::AppointmentReschedule;
};
