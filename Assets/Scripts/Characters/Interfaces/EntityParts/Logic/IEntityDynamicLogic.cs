using Characters.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Characters.Interfaces
{
    public interface IEntityDynamicLogic : IEntityLogic, IStunnable
    {
        public IEntityPhysicsBody Body { get; }
        public IEntityData Data { get; }
        public CharacterStats Stats { get; }

        public void Initialize(IEntityData data);
    }
}
