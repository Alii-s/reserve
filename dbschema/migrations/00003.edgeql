CREATE MIGRATION m1parn3byabcjcq4t5degmekronpt6rly5ayj6do5wjta6z4m7ylgq
    ONTO m1qu2qks4aspksummm6g73dtj55lhtor37vnc2ggtxfi7toac6vmka
{
  ALTER TYPE default::CasualEvent {
      CREATE REQUIRED PROPERTY description: std::str {
          SET REQUIRED USING (<std::str>{});
      };
  };
};
