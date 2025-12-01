using System.Collections;
using UnityEngine;

namespace LUP.ES
{
    public class DeathAction : BTNode
    {
        PlayerBlackboard blackboard;
        bool isDeathHandled = false;
        float delaySeconds = 3.0f;

        public DeathAction(PlayerBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            if (isDeathHandled == false)
            {
                isDeathHandled = true;
                blackboard.playerOverheadUI.UpdateHPUI();
                MonoBehaviour agent = blackboard.gameObject.GetComponent<MonoBehaviour>();
                agent.StartCoroutine(ShowResultRoutine());
                if (blackboard.animator != null)
                {
                    blackboard.animator.SetBool("IsDead", true);
                }
                return NodeState.Success;
            }
            return NodeState.Success;
        }

        private IEnumerator ShowResultRoutine()
        {
            yield return new WaitForSeconds(delaySeconds);
            blackboard.eventBroker.ReportGameFinish(false);
        }
        public override void Reset()
        {
            
        }
    }
}
