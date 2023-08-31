CREATE MIGRATION m1bjolpdddmbgyhr4qdvanqpjbuqq5cjjkabxjl2tfucpyxykw2rza
    ONTO m1eigwduaknxc3bd3x3aakhmaq344ilmqyudqfvvoqg2e5a2tpvpsa
{
  ALTER TYPE default::CasualEvent {
      ALTER PROPERTY start_date {
          RESET OPTIONALITY;
      };
  };
};
