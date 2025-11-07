using System;
using System.Collections.Generic;

namespace Framework
{
    public class RoguelikeInventoryConfig : IGameInventoryConfig
    {
        public List<string> GetCurrencyTypes()
        {
            return new List<string>
            {
                // TODO: 재화 타입 추가 (예: "Gold", "Wood", "Stone", "Iron", "Energy")
            };
        }

        public List<string> GetEquipmentSlots()
        {
            return new List<string>
            {
                // TODO: 장비 슬롯 추가 (예: "Tool", "Pet")
            };
        }

        public int GetDefaultCapacity()
        {
            return 60; // TODO: 필요한 용량으로 변경
        }

        public Type GetCustomInventoryType()
        {
            return null;
        }
    }
}
