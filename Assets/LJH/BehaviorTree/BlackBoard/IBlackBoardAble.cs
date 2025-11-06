using UnityEngine;

namespace RL
{
    public interface IBlackBoardAble
    {
        bool Alive { get; set; }

        bool CanAtk { get; set; }
        bool OnAtk { get; set; }

        bool OnHitted { get; set; }
        bool OnRampage { get; set; }

        bool HasTarget { get; set; }

        bool isLocallyControlled { get; set; }

        float Speed { get; set; }
        float AtkRange { get; set; }
        float AtkCollTime { get; set; }
        float HittedAccumTime { get; set; }
        float RampageTime { get; set; }

        float TargetDistance { get; set; }
    }
}

