using System;
using System.Collections.Generic;

namespace UtilityClasses {
    public class Observable<t1>{
        private t1 observableObject;
        private IList<Action<t1>> subscribers;
        public Observable(t1 initialObject){
            this.subscribers = new List<Action<t1>>();
            this.observableObject = initialObject;
        }
        public void next(t1 updatedObject)
        {
            this.observableObject = updatedObject;
            foreach (Action<t1> subscriber in this.subscribers)
            {
                subscriber(this.observableObject);
            }
        }

        public Subscription subscribe(Action<t1> subscription)
        {
            this.subscribers.Add(subscription);
            Subscription subscriptionRefernce = new Subscription(delegate(){
                this.subscribers.Remove(subscription);
            });
            subscription(this.observableObject);

            return subscriptionRefernce;
        }
    }
}