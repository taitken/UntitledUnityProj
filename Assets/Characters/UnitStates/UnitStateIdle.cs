using Characters;

namespace Unit.Models
{
    public class UnitStateIdle : BaseUnitState
    {
        public UnitStateIdle() : base(eUnitState.Idle)
        {

        }

        public override void Update(WorldCharacter worldChar)
        {

        }
    }
}