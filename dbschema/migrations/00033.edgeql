CREATE MIGRATION m1i4x6b6hzewtgtbtkgnumwots7lz63uadeacn6mmraenabwaj2k3a
    ONTO m1btsns3fx2cve2zbkwqjyvvnmlyzx5rj6tkst4uxwu47gvapitgpq
{
  ALTER SCALAR TYPE default::RescheduleState EXTENDING enum<Pending, Accepted, Declined>;
};
