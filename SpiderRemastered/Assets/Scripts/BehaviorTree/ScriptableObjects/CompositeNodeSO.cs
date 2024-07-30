using System.Collections.Generic;
using SFRemastered.BehaviorTree.Core;
using UnityEngine;

namespace SFRemastered.BehaviorTree.ScriptableObjects
{
    [CreateAssetMenu(menuName = "BehaviorTree/CompositeNode")]
    public class CompositeNodeSO : NodeSO
    {
        public List<NodeSO> children;
        public override Node CreateNode()
        {
            CompositeNode compositeNode = ScriptableObject.CreateInstance<CompositeNode>();
            foreach (var child in children)
            {
                compositeNode.AddChild(child.CreateNode());
            }
            return compositeNode;
        }
    }
}