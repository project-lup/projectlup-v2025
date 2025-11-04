using UnityEngine;

namespace ES
{
    public class BaseBlackboard : MonoBehaviour
    {
        public float HP = 0.0f;
        public float MaxHP = 100.0f;
        public float speed = 5.0f;

        [HideInInspector]
        public bool isDead = false;
    }
}
