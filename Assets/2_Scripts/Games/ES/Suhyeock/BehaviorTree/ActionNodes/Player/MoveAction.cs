using UnityEngine;
using UnityEngine.InputSystem;

namespace LUP.ES
{
    public class MoveAction : BTNode
    {
        private PlayerBlackboard blackboard;
        private CharacterController characterController;

        private float verticalVelocity;
        private float gravity = -9.81f;

        public MoveAction( PlayerBlackboard blackboard, CharacterController characterController)
        {
            this.blackboard = blackboard;
            this.characterController = characterController;
        }

        public override NodeState Evaluate()
        {
            float horizontal = blackboard.leftJoystick.Horizontal;
            float Vertical = blackboard.leftJoystick.Vertical;

            Vector3 moveDir = new Vector3(horizontal, 0f, Vertical);
            if (moveDir.magnitude > 1f) moveDir.Normalize();

            if(characterController.isGrounded)
            {
                if(verticalVelocity < 0)
                {
                    verticalVelocity = -2f;
                }
            }

            verticalVelocity += gravity * Time.deltaTime;

            Vector3 finalMove = moveDir * blackboard.speed;

            finalMove.y = verticalVelocity;
            characterController.Move(finalMove * Time.deltaTime);

            if (horizontal != 0 || Vertical != 0)
            {
                if (blackboard.weapon.state != WeaponState.ATTACKING && blackboard.weapon.state != WeaponState.RELOADING)
                {
                    characterController.transform.forward = moveDir;
                }

                blackboard.moveState = MoveState.MOVING;
                return NodeState.Running;
            }
            blackboard.moveState = MoveState.IDLE;
            return NodeState.Success;
        }

        public override void Reset()
        {

        }
    }
}
