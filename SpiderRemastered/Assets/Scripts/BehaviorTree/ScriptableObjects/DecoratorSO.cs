using SFRemastered.BehaviorTree.Core;
using UnityEngine;

namespace SFRemastered.BehaviorTree.ScriptableObjects
{
    public class DecoratorSO : NodeSO
    {
        public NodeSO child;
        public override Node CreateNode()
        {
            DecoratorNode decoratorNode = ScriptableObject.CreateInstance<DecoratorNode>();
            decoratorNode.SetChild(child.CreateNode());
            return decoratorNode;
        }
    }
}