using UnityEngine;

namespace SFRemastered.BehaviorTree.ScriptableObjects
{
    [CreateAssetMenu(menuName = "BehaviorTree/BehaviorTree")]
    public class BehaviorTreeSO : ScriptableObject
    {
        public NodeSO rootNode;
    }
}