CREATE MIGRATION m1uwlxy5y4uphmwp7wrdxfvrwno5gjkdtsdpetbq2ccbbdj2zm6naq
    ONTO m1izh42qlwvcgf72ezeqlwtlk2tu5s5vhexl6foltxrz3kmrkeu3nq
{
  ALTER TYPE default::Appointment RENAME TO default::AppointmentDetails;
};
