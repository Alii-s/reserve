CREATE MIGRATION m15rpnkzr5df7knvccypjkizfvsmeem2gc6jvcrbqdyaoqj6qvp4bq
    ONTO m1inaittb55ltxvcin3ult4b2z6n7e3qrcpqivoq4yc66yso5giwua
{
  ALTER TYPE default::AppointerNotifications {
      ALTER PROPERTY notificarion_type {
          RENAME TO notification_type;
      };
  };
};
