[System.Serializable]
public class ShootingStaticData
{

    [Column("Id")] public int Id;
    [Column("Type")] public string Type;
    [Column("Hp")] public float Hp;
    [Column("Atk")] public float Atk;
    [Column("Def")] public float Def;
    [Column("Spd")] public float Spd;
    [Column("AtkRange")] public float AtkRange;
    [Column("AtkSpeed")] public float AtkSpeed;
    [Column("AtkCooldown")] public float AtkCooldown;
    [Column("BulletSpeed")] public float BulletSpeed;
    [Column("SpecialAbility")] public string SpecialAbility;
    [Column("SkillType")] public string SkillType;
    [Column("SkillTarget")] public string SkillTarget;
    [Column("SkillEffect")] public string SkillEffect;
    [Column("SkillFeature")] public float SkillFeature;
    [Column("Name")] public string Name;
    [Column("Description")] public string Description;

}
