
using System.Collections.Generic;

namespace Characters.Player.Upgrades
{
    public interface ISingleUpgrade
    {
        public List<StatUIInfo> GetUpgradeInfo();
    }
}
