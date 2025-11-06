using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using DSG.Utils.Enums;
using static DSG.Character;

namespace DSG
{
    public class StatusEffectComponent : MonoBehaviour
    {
        Character owner;

        public Action<IStatusEffect> OnEffectAdded;
        public Action<IStatusEffect> OnEffectEndTurn;
        public Action<IStatusEffect> OnEffectRemoved;

        private readonly Dictionary<EStatusEffectType, IStatusEffect> _effects = new();

        private void Start()
        {
            owner = GetComponent<Character>();
        }
        public void AddEffect(IStatusEffect effect)
        {
            if (!owner.BattleComp.isAlive)
                return;

            if (_effects.TryGetValue(effect.type, out IStatusEffect getEffect))
            {
                getEffect.amount += effect.amount;  // 내부 값 수정
                _effects[effect.type] = getEffect;  // 다시 저장
            }
            else
            {
                _effects.Add(effect.type, effect);
            }

            effect.Apply(owner);
            OnEffectAdded?.Invoke(_effects[effect.type]);
        }
        public void TurnAll()
        {
            foreach (IStatusEffect effect in _effects.Values)
            {
                effect.Turn(owner);
                effect.remainingTurns--;
                OnEffectEndTurn?.Invoke(effect);
                if (effect.remainingTurns <= 0)
                {
                    RemoveEffect(effect);
                }
            }
        }
        public void RemoveEffect(IStatusEffect effect)
        {
            _effects.Remove(effect.type);
            effect.Remove(owner);
            OnEffectRemoved?.Invoke(effect);
        }

        void OnDisable()
        {
            OnEffectAdded = null; // 모든 구독자 제거
            OnEffectRemoved = null;
        }
    }
}