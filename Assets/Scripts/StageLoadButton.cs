using Manager;
using UnityEngine;

public class StageLoadButton : MonoBehaviour
{
    public void LoadStage(int stage)
    {
        Define.StageKind StageKind = (Define.StageKind)stage;
        Manager.StageManager.Instance.GetCurrentStage().LoadStage(StageKind);
    }
}
