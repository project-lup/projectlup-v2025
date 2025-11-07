using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 디버그/테스트용 인벤토리 설정
    /// 모든 기능을 테스트할 수 있도록 다양한 재화와 슬롯 제공
    /// </summary>
    public class DebugInventoryConfig : IGameInventoryConfig
    {
        public List<string> GetCurrencyTypes()
        {
            return new List<string>
            {
                "Gold",           // 테스트용 기본 재화
                "Silver",         // 테스트용 보조 재화
                "TestCurrency"    // 테스트용 특수 재화
            };
        }

        public List<string> GetEquipmentSlots()
        {
            return new List<string>
            {
                "Weapon",         // 무기 슬롯
                "Armor",          // 방어구 슬롯
                "Accessory",      // 악세서리 슬롯
                "TestSlot"        // 테스트용 슬롯
            };
        }

        public int GetDefaultCapacity()
        {
            return 50; // 테스트용 넉넉한 용량
        }

        public Type GetCustomInventoryType()
        {
            return null; // 기본 Inventory 사용
        }
    }
}
