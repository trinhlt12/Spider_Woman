using SFRemastered.BehaviorTree.ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace SFRemastered.BehaviorTree.Core
{
    public abstract class DecoratorNode : Node
    {
        [SerializeField] protected Node child;
        
        public void SetChild(Node child)
        {
            this.child = child;
        }

        public override void Initialize(BlackBoardSO blackBoard)
        {
            base.Initialize(blackBoard);
            child.Initialize(blackBoard);
        }
    }
}