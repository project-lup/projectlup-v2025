using LUP.DSG.Utils.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public static class StageInitializeInvoker
    {
        // base.OnStageEnter() 이후 초기화 지점
        public static event Action<DeckStrategyStage> OnDSGStageInitialize;
        public static event Action<DeckStrategyStage> OnDSGStagePostInitialize;

        public static void Invoke(DeckStrategyStage stage)
        {
            OnDSGStageInitialize?.Invoke(stage);
            OnDSGStagePostInitialize?.Invoke(stage);
        }
    }

    public class DeckStrategyStage : BaseStage
    {
        public BaseRuntimeData RuntimeData;
        public BaseRuntimeData enemyStageRuntimeData;

        public List<DeckStaticData> DeckDataList;
        public List<DeckCharacterStaticData> CharacterDataList;
        public TeamMVPData mvpData;
        public List<CharacterPrefabData> characterPrefabList = new List<CharacterPrefabData>();

        public DeckStrategyRuntimeData DSGRuntimeData { get; private set; }
        public DSGEnemyStageRuntimeData DSGEnemyRuntimeData { get; private set; }

        protected override void Awake() 
        {
            base.Awake();
            StageKind = Define.StageKind.DSG;

        }

        public override IEnumerator OnStageEnter()
        {
            yield return base.OnStageEnter();
            
            DSGRuntimeData ??= (DeckStrategyRuntimeData)RuntimeData;
            DSGEnemyRuntimeData ??= (DSGEnemyStageRuntimeData)enemyStageRuntimeData;

            if (DSGRuntimeData?.OwnedCharacterList is { Count: <= 0 })
            {
                OwnedCharacterTable characterTable = Resources.Load<OwnedCharacterTable>(
                    "Data/Games/DSG/ScriptableObjects/OwnedCharacter/OwnedCharacterListTable");
                if (characterTable != null)
                    DSGRuntimeData.OwnedCharacterList = characterTable.ownedCharacterList;
            }

            if (DSGEnemyRuntimeData?.SelectedEnemyStage != null)
            {
                EnemyStageData enemyStageData = Resources.Load<EnemyStageData>(
                    "Data/Games/DSG/ScriptableObjects/EnemyStageData1");
                if (enemyStageData != null)
                    DSGEnemyRuntimeData.SelectedEnemyStage = enemyStageData;
            }

            SaveDatas();

            // PCR 인벤토리 접근 가능 여부 확인
            if (InventoryManager.Instance.HasInventory("PCR"))
                Debug.Log("[DSGStage] PCR 인벤토리 접근 가능");
            else
                Debug.LogWarning("[DSGStage] PCR 인벤토리가 아직 로드되지 않았습니다!");

            StageInitializeInvoker.Invoke(this);

            yield return null;
        }

        // PCR 팀의 공유 인벤토리 가져오기
        public Inventory GetSharedInventory() => InventoryManager.Instance.GetInventory("PCR");

        public override IEnumerator OnStageStay()
        {
            yield return base.OnStageStay();
            //일단 납두기
            yield return null;
        }
        public override IEnumerator OnStageExit()
        {
            yield return base.OnStageExit();
            //구현부


            yield return null;
        }
        protected override void LoadResources()
        {
            //resource = ResourceManager.Instance.Load...
        }

        protected override void GetDatas()
        {
            LoadStaticDatas();
            LoadRuntimeDatas();

            if (RuntimeData is not DeckStrategyRuntimeData deckStrategyRuntimeData) return;
            if (deckStrategyRuntimeData.Teams.Count > 0) return;

            deckStrategyRuntimeData.Teams.AddRange(new Team[6]);
        }

        protected override void SaveDatas()
        {
            List<BaseRuntimeData> runtimeDataList = new List<BaseRuntimeData>();

            if (RuntimeData != null) runtimeDataList.Add(RuntimeData);
            if (enemyStageRuntimeData != null) runtimeDataList.Add(enemyStageRuntimeData);

            base.SaveRuntimeDataList(runtimeDataList);
        }

        private void LoadStaticDatas()
        {
            List<BaseStaticDataLoader> loaders = base.GetStaticData(this, 1);
            if (loaders == null) return;

            foreach (BaseStaticDataLoader loader in loaders)
            {
                switch (loader)
                {
                    case DeckStaticDataLoader deckLoader:
                        DeckDataList = deckLoader.GetDataList();
                        break;
                    case DeckCharacterStaticDataLoader charLoader:
                        CharacterDataList = charLoader.GetDataList();
                        break;
                    case DeckCharacterModelStaticDataLoader modelLoader:
                        LoadCharacterPrefabs(modelLoader.GetDataList());
                        break;
                }
            }
        }

        private void LoadRuntimeDatas()
        {
            List<BaseRuntimeData> runtimeDatas = base.GetRuntimeData(this, 1);
            if (runtimeDatas != null)
            {
                foreach (BaseRuntimeData data in runtimeDatas)
                {
                    if (data is DeckStrategyRuntimeData deckData)
                        RuntimeData = deckData;
                }
            }

            List<BaseRuntimeData> enemyDatas = base.GetRuntimeData(this, 2);
            if (enemyDatas != null)
            {
                foreach (BaseRuntimeData data in enemyDatas)
                {
                    if (data is DSGEnemyStageRuntimeData enemyData)
                        enemyStageRuntimeData = enemyData;
                }
            }
        }

        private void LoadCharacterPrefabs(List<DeckCharacterModelStaticData> modelDataList)
        {
            if (modelDataList == null) return;

            foreach (DeckCharacterModelStaticData modelData in modelDataList)
            {
                GameObject prefab = Resources.Load<GameObject>(modelData.ModelPath);
                if (prefab == null) continue;

                characterPrefabList.Add(new CharacterPrefabData
                {
                    prefab = prefab,
                    ID = modelData.ModelId
                });
            }
        }

        public GameObject GetCharacterPrefab(int modelId) => FindCharacterPrefabData(modelId)?.prefab;

        public CharacterPrefabData FindCharacterPrefabData(int prefabId)
        {
            if (characterPrefabList == null) return null;

            foreach (CharacterPrefabData data in characterPrefabList)
                if (data.ID == prefabId) return data;

            return null;
        }

        public CharacterData FindCharacterData(int id, int level)
        {
            if (CharacterDataList == null || DeckDataList == null) return null;

            DeckCharacterStaticData charData = null;

            foreach (DeckCharacterStaticData data in CharacterDataList)
            {
                if (data.CharacterId == id) { charData = data; break; }
            }
            if (charData == null) return null;
            if (DSGRuntimeData?.OwnedCharacterList is not { Count: > 0 }) return null;

            int statusId = id * 100 + level;
            foreach (DeckStaticData statusData in DeckDataList)
            {
                if (statusData.tableId != statusId) continue;

                CharacterData characterData = new CharacterData {
                    ID = id,
                    characterName = charData.CharacterName,
                    type = (EAttributeType)charData.AttributeType,
                    rangeType = (ERangeType)charData.RangeType,
                    maxHp = statusData.hp,
                    attack = statusData.attack,
                    defense = statusData.defense,
                    speed = statusData.speed
                };

                return characterData;
            }

            return null;
        }

        public EnemyStageData GetEnemyStage() => DSGEnemyRuntimeData?.SelectedEnemyStage;

        public void SetEnemyStage(EnemyStageData enemyStageData)
        {
            if (DSGEnemyRuntimeData == null) return;
            DSGEnemyRuntimeData.SelectedEnemyStage = enemyStageData;
            SaveDatas();
        }

        public void ChangeScene(int sceneIndex)
        {
            //0: edit
            //1: battle
            //2: result

            LoadStage(StageKind, sceneIndex);
        }

        public void BattleEnd()
        {
            if (GetComponent<BattleSystem>() != null)
                ChangeScene(2);
        }

        public BattleSystem GetBattleSystem() => GetComponent<BattleSystem>();

        public Team GetSelectedTeam()
        {
            if (DSGRuntimeData == null) return null;

            int index = DSGRuntimeData.SelectedTeamIndex;
            return DSGRuntimeData.Teams[index] ??= new Team();
        }

        public void SetSelectedTeam(int index)
        {
            if (DSGRuntimeData == null) return;
            DSGRuntimeData.SelectedTeamIndex = index;
            SaveDatas();
        }

        public Team ChangeSelectedTeam(int index)
        {
            if (DSGRuntimeData == null) return null;

            DSGRuntimeData.SelectedTeamIndex = index;
            return DSGRuntimeData.Teams[index] ??= new Team();
        }

        public CharacterInfo GetOwnedCharacterById(int id)
        {
            if (DSGRuntimeData?.OwnedCharacterList == null) return null;

            List<CharacterInfo> ownedList = DSGRuntimeData.OwnedCharacterList;
            for(int i = 0; i < ownedList.Count; ++i)
            {
                if (ownedList[i].characterID == id)
                    return ownedList[i];
            }

            return null;
        }

    }
}

