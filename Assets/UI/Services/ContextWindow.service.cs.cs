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
        public MonoObseravable<IList<ContextWindowModel>> contextObseravable { get; set; } = new MonoObseravable<IList<ContextWindowModel>>(new List<ContextWindowModel>());

        public void AddContext(ContextWindowModel context)
        {
            IList<ContextWindowModel> _contexts = this.contextObseravable.Get();
            if (_contexts.Find(existingcontext => { return context.ID == existingcontext.ID; }) == null)
            {
                _contexts.Add(context);
                this.contextObseravable.Set(_contexts);
            }
        }

        public void RemoveContext(long modelID)
        {
            this.contextObseravable.Set(this.contextObseravable.Get().Filter(context => { return context.objectID != modelID; }));
        }
    }
}
