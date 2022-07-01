using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using Environment.Models;

namespace GameControllers.Services
{
    public interface IEnvironmentService
    {
        Subscribable<IList<MineableObjectModel>> mineableObjects { get; set; }
        void addMineableObject(MineableObjectModel mineableObject);
        void removeMineableObject(long id);
    }
}
