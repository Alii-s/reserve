CREATE MIGRATION m1lzt4eyyva2pujg7v6cbeuqqzqiv5odwmh27yidqxldyvvexdgesq
    ONTO m1ceukozeeuwmiiidi6ky2jqef6zfswhgcetr36qv6ftcbjrzrqnaa
{
  ALTER TYPE default::CasualTicket {
      CREATE REQUIRED PROPERTY reserver_phone_number: std::str {
          SET REQUIRED USING (<std::str>{});
      };
  };
};
