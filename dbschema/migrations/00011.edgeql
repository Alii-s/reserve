CREATE MIGRATION m1xvivp7hyiukzwrew4tgxz7rs66shi3dh6ntfhu4ci4yqw2zafnvq
    ONTO m1ddmnl6w2i4elxcdnemgalkpb45c3b7olmwtevr5ybrx745ea4t6a
{
  ALTER TYPE default::QueueEvent {
      CREATE REQUIRED PROPERTY ticket_counter: std::int32 {
          SET REQUIRED USING (<std::int32>{});
      };
  };
};
