using UnityEngine;
using DSG;
public interface IAttackTargetSelector
{
    public LineupSlot SelectTarget(Character Attacker);
}
