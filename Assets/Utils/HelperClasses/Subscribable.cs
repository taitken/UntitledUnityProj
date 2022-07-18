using System;
using System.Collections.Generic;

namespace UtilityClasses
{
    public class Obseravable<t1>
    {
        private t1 ObseravableObject;
        private IList<Action<t1>> subscribers;
        public Obseravable(t1 initialObject)
        {
            this.subscribers = new List<Action<t1>>();
            this.ObseravableObject = initialObject;
        }
        public t1 Get()
        {
            return this.ObseravableObject;
        }
        public void Set(t1 updatedObject)
        {
            this.ObseravableObject = updatedObject;
            this.NotifyAllSubscribers();
        }

        public Subscription Subscribe(Action<t1> subscription)
        {
            this.subscribers.Add(subscription);
            Subscription subscriptionRefernce = new Subscription(delegate ()
            {
                this.subscribers.Remove(subscription);
            });
            subscription(this.ObseravableObject);

            return subscriptionRefernce;
        }

        public void NotifyAllSubscribers()
        {
            foreach (Action<t1> subscriber in this.subscribers)
            {
                subscriber(this.ObseravableObject);
            }
        }
    }
}