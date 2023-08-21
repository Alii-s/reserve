CREATE MIGRATION m1ceukozeeuwmiiidi6ky2jqef6zfswhgcetr36qv6ftcbjrzrqnaa
    ONTO m177zwoqvffweirp2uocovqsecuarubee2a4edremmeoz2iit4mdpa
{
  ALTER TYPE default::CasualEvent {
      CREATE PROPERTY image_url: std::str;
  };
};
