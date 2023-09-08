CREATE MIGRATION m1r7vuox6s7hh3xqykjtzo7lelv62hjb4yxhs4u6ecxzijtfangquq
    ONTO m1sxtr3dc4647qzim63glzfz4hgwyryysup3x2n2iyzz7eygpb2dma
{
  ALTER TYPE default::Availability {
      CREATE REQUIRED LINK appointment_calendar: default::AppointmentCalendar {
          ON TARGET DELETE DELETE SOURCE;
          SET REQUIRED USING (<default::AppointmentCalendar>{});
      };
  };
};
