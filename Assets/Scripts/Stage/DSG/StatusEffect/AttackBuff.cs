using DSG.Utils.Enums;
using UnityEngine;

namespace DSG
{
    public class AttackBuff : IStatusEffect
    {
        public EOperationType operationType;
        public AttackBuff(EOperationType Type, float Amount = 1f, int Turns = 1)
            : base(EStatusEffectType.AttackBuff, Amount, Turns)
        {
            operationType = Type;
        }
        public override void Apply(Character C)
        {
            switch(operationType)
            {
                case EOperationType.Calm:
                    {
                        C.BattleComp.attack = amount; break;
                    }
                case EOperationType.Plus:
                    {
                        C.BattleComp.attack += amount; break;
                    }
                case EOperationType.Minus:
                    {
                        C.BattleComp.attack -= amount; break;
                    }
                case EOperationType.Multiply:
                    {
                        C.BattleComp.attack *= amount; break;
                    }
                case EOperationType.Division:
                    {
                        C.BattleComp.attack /= amount; break;
                    }
            }
        }
        public override void Turn(Character C) { }
        public override void Remove(Character C)
        {
            C.BattleComp.attack -= amount;
        }
    }
}