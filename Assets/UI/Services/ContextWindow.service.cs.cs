using System;
using System.Collections.Generic;
using UtilityClasses;
using UI.Models;
using UI.Services;

namespace UI.Services
{
    public class ContextWindowService : IContextWindowService
    {
        public ContextWindowService()
        {

        }
        public Subscribable<IList<ContextWindowModel>> contextSubscribable { get; set; } = new Subscribable<IList<ContextWindowModel>>(new List<ContextWindowModel>());

        public void AddContext(ContextWindowModel context)
        {
            IList<ContextWindowModel> _contexts = this.contextSubscribable.Get();
            if (_contexts.Find(existingcontext => { return context.ID == existingcontext.ID; }) == null)
            {
                _contexts.Add(context);
                this.contextSubscribable.Set(_contexts);
            }
        }

        public void RemoveContext(long modelID)
        {
            this.contextSubscribable.Set(this.contextSubscribable.Get().Filter(context => { return context.objectID != modelID; }));
        }
    }
}
