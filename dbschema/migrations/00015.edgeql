CREATE MIGRATION m1t4ew4tsz74p6viko3d7mi5gtsqssbdlo7zyf54vtmzp4m2kae7ja
    ONTO m1r7vuox6s7hh3xqykjtzo7lelv62hjb4yxhs4u6ecxzijtfangquq
{
  ALTER TYPE default::Availability {
      ALTER PROPERTY end_time {
          SET TYPE cal::local_time USING (<cal::local_time>.end_time);
      };
      ALTER PROPERTY start_time {
          SET TYPE cal::local_time USING (<cal::local_time>.start_time);
      };
  };
};
