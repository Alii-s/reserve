CREATE MIGRATION m1ddmnl6w2i4elxcdnemgalkpb45c3b7olmwtevr5ybrx745ea4t6a
    ONTO m1bjolpdddmbgyhr4qdvanqpjbuqq5cjjkabxjl2tfucpyxykw2rza
{
  ALTER TYPE default::CasualEvent {
      ALTER PROPERTY start_date {
          SET REQUIRED USING (<std::datetime>{});
      };
  };
};
