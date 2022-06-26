using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine {
    public abstract class MonoBehaviour2: MonoBehaviour{

        private IList<Action> onDeathCallbacks = new List<Action>();
        public virtual void OnClickedByUser()
        {

        }

        public void BeforeDestroy(Action callback)
        {
            this.onDeathCallbacks.Add(callback);
        }

        public void DestroySelf(){
            foreach (Action callback in this.onDeathCallbacks)
            {
                callback();
            }
            Destroy(gameObject);
        }
    }
}