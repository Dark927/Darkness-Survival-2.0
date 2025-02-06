using Characters.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Characters.Interfaces
{
    public interface IEntityLogic
    {
        public CharacterBodyBase Body { get; }
        public CharacterBaseData Data { get; }
        public CharacterStats Stats { get; }
    }
}
