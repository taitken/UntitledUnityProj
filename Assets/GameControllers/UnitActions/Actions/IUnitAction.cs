using System;

namespace UnitAction
{
    public interface IUnitAction
    {
        bool completed { get; set; }
        bool cancel { get; set; }
        bool CheckCompleted();
        bool PerformAction();
        void CancelAction();
    }
}