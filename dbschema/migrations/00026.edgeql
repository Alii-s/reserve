CREATE MIGRATION m1l3imtoztyi6g4l6zew56cpbyhdy2h4pqnffprqcxyz4wts7u44da
    ONTO m1o4t2nyotp35i7kln4tgxml6m4nvnyzx5pst55vw6qf5lvera6nva
{
  ALTER TYPE default::QueueTicket {
      CREATE PROPERTY status: std::str;
  };
};
