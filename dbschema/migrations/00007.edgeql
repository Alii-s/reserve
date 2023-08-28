CREATE MIGRATION m1x4rdit4vuioqq3uhoyeamu6763qjtup4otssou4wanfp6ticelqa
    ONTO m1lzt4eyyva2pujg7v6cbeuqqzqiv5odwmh27yidqxldyvvexdgesq
{
  ALTER TYPE default::CasualEvent {
      CREATE REQUIRED PROPERTY opened: std::bool {
          SET REQUIRED USING (<std::bool>{true});
      };
  };
};
