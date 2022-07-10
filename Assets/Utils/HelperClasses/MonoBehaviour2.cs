using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;

namespace UnityEngine
{
    public abstract class MonoBehaviour2 : MonoBehaviour
    {

        private IList<Action> onDeathCallbacks = new List<Action>();
        protected IList<Subscription> subscriptions = new List<Subscription>();
        public virtual void OnClickedByUser()
        {

        }

        public void BeforeDestroy(Action callback)
        {
            this.onDeathCallbacks.Add(callback);
        }

        protected virtual void BeforeDeath()
        {

        }

        public void Destroy()
        {
            this.subscriptions.ForEach(sub =>{
                sub.unsubscribe();
            });
            foreach (Action callback in this.onDeathCallbacks)
            {
                callback();
            }
            this.BeforeDeath();
            Destroy(gameObject);
        }
    }
}