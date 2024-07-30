using System.Collections.Generic;
using SFRemastered.BehaviorTree.Core;
using UnityEngine;

namespace SFRemastered.BehaviorTree.ScriptableObjects
{
    public class ActionNodeSO : NodeSO
    {
        public override Node CreateNode()
        {
            return ScriptableObject.CreateInstance<ActionNode>();
        }
    }
}
