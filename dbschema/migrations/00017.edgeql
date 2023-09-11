CREATE MIGRATION m13i62ycj7gd6r4gkq6lhzaeehn5mrllxzulsxslspkomuotd6qa7a
    ONTO m14soowyaultukborhlmcjvoyaxsdznm4cbzczt5raegecblzhbhrq
{
  ALTER TYPE default::QueueEvent {
      CREATE REQUIRED PROPERTY ticket_counter: std::int32 {
          SET REQUIRED USING (<std::int32>{0});
      };
  };
};
