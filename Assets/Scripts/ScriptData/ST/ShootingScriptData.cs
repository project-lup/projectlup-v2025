[System.Serializable]
public class ShootingScriptData
{
    [Column("Id")] public int Id;
    [Column("Type")] public string Type;
    [Column("Hp")] public int Hp;
    [Column("Atk")] public int Atk;
    [Column("Def")] public int Def;
    [Column("Spd")] public float Spd;
    [Column("AtkRange")] public float AtkRange;
    [Column("AtkSpeed")] public float AtkSpeed;
    [Column("AtkCooldown")] public float AtkCooldown;
    [Column("BulletSpeed")] public float BulletSpeed;
    [Column("SpecialAbility")] public string SpecialAbility;
    [Column("SkillType")] public string SkillType;
    [Column("SkillTarget")] public string SkillTarget;
    [Column("SkillEffect")] public string SkillEffect;
    [Column("SkillFeature")] public float SkillFeature; //퍼센테이지로 증감 되는거 까먹지말기
    [Column("Name")] public string Name;
    [Column("Description")] public string Description;
}
