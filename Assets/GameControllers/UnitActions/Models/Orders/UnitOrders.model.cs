using System;
using System.Collections.Generic;
using GameControllers.Services;
using Unit.Models;
using UnityEngine;

namespace GameControllers.Models
{
    public abstract class UnitOrderModel : BaseModel
    {
        public UnitOrderModel(Vector3Int _coordinates, eOrderTypes _orderType, bool _displayIcon = true) : base()
        {
            this.coordinates = _coordinates;
            this.orderType = _orderType;
            this.displayIcon = _displayIcon;
            this.iconDeletedFromWorld = false;
        }
        public eOrderTypes orderType { get; set; }
        public Vector3Int coordinates { get; set; }
        public float prioritySetting { get; set; }
        public bool displayIcon { get; set; }
        public bool iconDeletedFromWorld { get; set; }
        public virtual bool PathAdjacentToCoors { get { return true; } }

        public virtual bool IsUniqueCheck(IList<UnitOrderModel> orderList)
        {
            return orderList.Find(existingOrder => { return this.ID == existingOrder.ID || this.coordinates == existingOrder.coordinates; }) == null;
        }

        public virtual bool CanAssignToUnit(IList<IBaseService> _services, UnitModel _unitModel)
        {
            IPathFinderService pathingService = this.GetService<IPathFinderService>(_services);
            IEnvironmentService environmentService = this.GetService<IEnvironmentService>(_services);
            if (pathingService == null || environmentService == null) return false;
            return pathingService.CanPathTo(environmentService.LocalToCell(_unitModel.position), this.coordinates, pathingService.GetPathFinderMap(), this.PathAdjacentToCoors);
        }

        protected T GetService<T>(IList<IBaseService> _services) where T : IBaseService
        {
            T returnService = default(T);
            try
            {
                returnService = (T)_services.Find(service =>
                {
                    return typeof(T).IsAssignableFrom(service.GetType());
                });
            }
            catch (System.Exception)
            {
                Debug.LogException(new System.Exception("Service type does not exist in the provided service list. Attemped type: " + typeof(T).ToString()));
            }
            return returnService;
        }
    }
}

