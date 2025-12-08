using UnityEngine;

[CreateAssetMenu(fileName = "ItemLoader", menuName = "StaticData/LUP Item Loader")]
public class LUPItemStaticDataLoader : BaseStaticDataLoader<LUP.LUPItemStaticData>
{
    [Header("시트 정보")]
    [Tooltip("이 로더가 로드하는 시트의 이름 (예: RPG_Items, ST_Items)")]
    [SerializeField] private string sheetName = "";

    [Header("구글 시트 CSV URL")]
    [Tooltip("구글 시트에서 파일 → 공유 → 웹에 게시 → CSV로 내보낸 URL")]
    [SerializeField] private string csvUrl = "";

    protected override string CSV_URL => csvUrl;

    public string SheetName => sheetName;

}
