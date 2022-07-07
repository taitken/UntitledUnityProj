using System;
using System.Collections.Generic;

namespace UtilityClasses
{
    public class Subscribable<t1>
    {
        private t1 subscribableObject;
        private IList<Action<t1>> subscribers;
        public Subscribable(t1 initialObject)
        {
            this.subscribers = new List<Action<t1>>();
            this.subscribableObject = initialObject;
        }
        public t1 Get()
        {
            return this.subscribableObject;
        }
        public void Set(t1 updatedObject)
        {
            this.subscribableObject = updatedObject;
            this.NotifyAllSubscribers();
        }

        public Subscription Subscribe(Action<t1> subscription)
        {
            this.subscribers.Add(subscription);
            Subscription subscriptionRefernce = new Subscription(delegate ()
            {
                this.subscribers.Remove(subscription);
            });
            subscription(this.subscribableObject);

            return subscriptionRefernce;
        }

        public void NotifyAllSubscribers()
        {
            foreach (Action<t1> subscriber in this.subscribers)
            {
                subscriber(this.subscribableObject);
            }
        }
    }
}