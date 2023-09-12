CREATE MIGRATION m1nvx3j4pqt4l4c2bib7fq24zc4ge3saehwdzy2gkye436jssqj2ba
    ONTO m14soowyaultukborhlmcjvoyaxsdznm4cbzczt5raegecblzhbhrq
{
  ALTER TYPE default::QueueEvent {
      CREATE REQUIRED PROPERTY ticket_counter: std::int32 {
          SET REQUIRED USING (<std::int32>{0});
      };
  };
};
