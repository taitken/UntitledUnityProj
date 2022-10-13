using Unit.Models;

namespace Characters
{
    public class UnitStateFactory
    {
        public static BaseUnitState CreateUnitState(eUnitState unitState)
        {
            BaseUnitState returnState;
            switch (unitState)
            {
                case eUnitState.Idle:
                    returnState = new UnitStateIdle();
                    break;
                case eUnitState.Digging:
                    returnState = new UnitStateDigging();
                    break;
                case eUnitState.Moving:
                    returnState = new UnitStateMoving();
                    break;
                case eUnitState.Planting:
                    returnState = new UnitStatePlanting();
                    break;
                case eUnitState.Storing:
                    returnState = new UnitStateStoring();
                    break;
                default:
                    returnState = new UnitStateIdle();
                    break;
            }
            return returnState;
        }
    }
}