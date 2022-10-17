using Unit.Models;

namespace Characters
{
    public abstract class BaseUnitState
    {
        public eUnitState stateEnum;
        public BaseUnitState(eUnitState _unitState)
        {
            this.stateEnum = _unitState;
        }
        public abstract void Initialise(WorldCharacter worldChar);
        public abstract void Update(WorldCharacter worldChar);
        public abstract void FixedUpdate(WorldCharacter worldChar);
    }
}