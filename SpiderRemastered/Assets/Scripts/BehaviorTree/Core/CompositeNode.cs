using System;
using System.Collections.Generic;
using SFRemastered.BehaviorTree.ScriptableObjects;
using UnityEngine;

namespace SFRemastered.BehaviorTree.Core
{
    public abstract class CompositeNode : Node
    {
        [SerializeField] protected List<Node> children = new List<Node>();
        
        public void AddChild(Node child)
        {
            children.Add(child);
        }

        public override void Initialize(BlackBoardSO blackBoard)
        {
            base.Initialize(blackBoard);
            foreach (var child in children)
            {
                child.Initialize(blackBoard);
            }
        }
    }
}