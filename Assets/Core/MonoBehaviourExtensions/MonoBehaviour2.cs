using System;
using System.Collections.Generic;
using GameControllers.Models;
using UnityEngine;
using UtilityClasses;
using Zenject;

namespace UnityEngine
{
    public abstract class MonoBehaviour2 : MonoBehaviour
    {

        private IList<Action> onDeathCallbacks = new List<Action>();
        protected IList<Subscription> subscriptions = new List<Subscription>();

 
        public virtual void OnClickedByUser()
        {

        }

        public virtual void OnMouseEnter()
        {

        }

        public virtual void OnMouseOver()
        {

        }

        public virtual void OnMouseExit()
        {

        }

        public virtual void OnDrag(DragEventModel dragEvent)
        {

        }

        public virtual void OnDragEnd(DragEventModel dragEvent)
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
            this.subscriptions.ForEach(sub =>
            {
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