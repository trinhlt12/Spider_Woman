using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered.BehaviorTree.Core
{
    [CreateAssetMenu(menuName = "BehaviorTree/BlackBoard")]
    public class BTBlackBoard : ScriptableObject
    {
        private Dictionary<string, object> data = new Dictionary<string, object>();
        
        public void SetValue<T>(string key, T value)
        {
            if (data.ContainsKey(key))
            {
                data[key] = value;
            }
            else
            {
                data.Add(key, value);
            }
        }
        
        public T GetValue<T>(string key)
        {
            if (data.TryGetValue(key, out var value))
            {
                return (T)value;
            }

            return default;
        }
    }
}