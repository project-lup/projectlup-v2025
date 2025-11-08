using LUP.ST;
using UnityEngine;

public class MonsterInfo : MonoBehaviour
{

    public int Id;
    public string Type;
    public float Hp;
    public float Atk;
    public float Def;
    public float Spd;
    public float AtkRange;
    public float AtkSpeed;
    public float AtkCooldown;
    public float BulletSpeed;
    public string SpecialAbility;
    public string SkillType;
    public string SkillTarget;
    public string SkillEffect;
    public float SkillFeature;
    public string Name;
    public string Description;

    private StatComponent stats;
    void Awake()
    {
        stats = GetComponent<StatComponent>();
        if (stats == null)
        {
            Debug.LogError($"{gameObject.name}: StatComponent가 없습니다!");
        }
    }

    public bool IsHpZero() => stats != null && stats.IsDead;
    public StatComponent Stats => stats;
}
