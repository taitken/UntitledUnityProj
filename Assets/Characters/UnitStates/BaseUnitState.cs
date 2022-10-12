using Characters;

namespace Unit.Models
{
    public abstract class BaseUnitState
    {
        public eUnitState unitState;
        public BaseUnitState(eUnitState _unitState)
        {
            this.unitState = _unitState;
        }

        public abstract void Update(WorldCharacter worldChar);
    }
}