using System;
using System.Collections.Generic;

namespace Framework
{
    public class DeckStrategyInventoryConfig : IGameInventoryConfig
    {
        public List<string> GetCurrencyTypes()
        {
            return new List<string>
            {
                // TODO: 재화 타입 추가 (예: "Gold", "Mana", "Crystal")
            };
        }

        public List<string> GetEquipmentSlots()
        {
            return new List<string>
            {
                // TODO: 장비 슬롯 추가 (예: "Artifact", "Relic")
            };
        }

        public int GetDefaultCapacity()
        {
            return 40; // TODO: 필요한 용량으로 변경
        }

        public Type GetCustomInventoryType()
        {
            return null;
        }
    }
}
