using System;
using Unit.Models;

namespace UnitAction
{
    public interface IUnitAction
    {
        UnitModel unit {get;set;}
        bool completed { get; set; }
        bool cancel { get; set; }
        bool CheckCompleted();
        bool PerformAction();
        void CancelAction();
    }
}