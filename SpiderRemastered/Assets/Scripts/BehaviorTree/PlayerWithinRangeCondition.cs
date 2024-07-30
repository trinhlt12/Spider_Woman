using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace SFRemastered.BehaviorTree
{
    public class PlayerWithinRangeCondition : ConditionTask
    {
        public BBParameter<GameObject> player;
        public BBParameter<float> range = 5f;

        protected override bool OnCheck()
        {
            return Vector3.Distance(agent.transform.position, player.value.transform.position) <= range.value;
        }
    }
}