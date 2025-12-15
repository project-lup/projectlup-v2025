using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.ES
{
    public enum MoveState
    {
        IDLE,
        MOVING,
        INTERACTING
    }


    public class PlayerBlackboard : BaseBlackboard
    {
        public float InteractionRadius = 2.0f;
        public FixedJoystick leftJoystick;
        public FixedJoystick rightJoystick;
        public int CurrentWeaponID = 2;
        [HideInInspector]
        public EventBroker eventBroker;
        [HideInInspector]
        public Weapon weapon;
        public InteractionDetector InteractionDetector;
        [HideInInspector]
        public Animator animator;
        [HideInInspector]
        public MoveState moveState = MoveState.IDLE;
        [HideInInspector]
        public IInteractable interactingObject = null;
        [HideInInspector]
        public bool isInteractionButtonPressed = false;
        [HideInInspector]
        public bool isReloadButtonPressed = false;
        [HideInInspector]
        public PlayerOverheadUI playerOverheadUI;
        [HideInInspector]
        public WeaponEquip weaponEquip;
        [HideInInspector]
        public PlayerIK playerIK;
    
        public void ResetInteractionState()
        {
            //isCastingInteraction = false;
            interactingObject = null;
        }

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            playerIK = GetComponentInChildren<PlayerIK>();
            eventBroker = FindAnyObjectByType<EventBroker>();
        }

        public void SetWeaponVisible(bool isVisible)
        {
            weapon.SetWeaponVisible(isVisible);
            playerIK.SetIsActivateIK(isVisible);
        }
    }
}


