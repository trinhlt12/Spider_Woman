using SFRemastered.BehaviorTree.Core;
using UnityEngine;

namespace SFRemastered.BehaviorTree.ScriptableObjects
{
    public abstract class NodeSO : ScriptableObject
    {
        public abstract Node CreateNode();
    }
}