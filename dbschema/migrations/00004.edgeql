CREATE MIGRATION m177zwoqvffweirp2uocovqsecuarubee2a4edremmeoz2iit4mdpa
    ONTO m1parn3byabcjcq4t5degmekronpt6rly5ayj6do5wjta6z4m7ylgq
{
  ALTER TYPE default::CasualEvent {
      ALTER PROPERTY current_capacity {
          SET TYPE std::int32;
      };
      ALTER PROPERTY maximum_capacity {
          SET TYPE std::int32;
      };
  };
  ALTER TYPE default::QueueEvent {
      ALTER PROPERTY current_capacity {
          SET TYPE std::int32;
      };
      ALTER PROPERTY maximum_capacity {
          SET TYPE std::int32;
      };
  };
  ALTER TYPE default::QueueTicket {
      ALTER PROPERTY queue_number {
          SET TYPE std::int32;
      };
  };
};
