using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Environment.Models;

namespace GameControllers.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        public Subscribable<IList<MineableObjectModel>> mineableObjects { get; set; } = new Subscribable<IList<MineableObjectModel>>(new List<MineableObjectModel>());

        public void AddMineableObject(MineableObjectModel mineableObject)
        {
            IList<MineableObjectModel> _mineableObjects = this.mineableObjects.Get();
            if (_mineableObjects.Find(existingMineableObject => { return mineableObject.localPosition == existingMineableObject.localPosition; }) == null)
            {
                _mineableObjects.Add(mineableObject);
                this.mineableObjects.Set(_mineableObjects);
            }
        }
        public void RemoveMineableObject(long id)
        {
            this.mineableObjects.Set(this.mineableObjects.Get().Filter(mineableObject => { return mineableObject.ID != id; }));
        }
    }
}