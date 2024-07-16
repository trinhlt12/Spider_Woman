using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered
{
    public class WebAttachPoint : MonoBehaviour
    {
        public bool canUpdatePos = true;
        [SerializeField] private Transform target;
        
        
        private void Update()
        {
            if(!canUpdatePos) return;
            this.transform.position = target.position;
        }
    }
}
