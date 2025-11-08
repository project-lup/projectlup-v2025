using UnityEngine;
using UnityEngine.SceneManagement;
namespace LUP.ST
{

    public class SceneChanger : MonoBehaviour
    {
        public void LoadGameScene()
        {
            StageManager.Instance.GetCurrentStage().LoadStage(Define.StageKind.ST, 1);
            //SceneManager.LoadScene("GameScene");
        }
        public void LoadLobbyScene()
        {
            StageManager.Instance.GetCurrentStage().LoadStage(Define.StageKind.ST, 2);
            //SceneManager.LoadScene("LobbyScene");
        }
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        public void QuitGame()
        {
            Debug.Log("게임 종료!");
            Application.Quit();
        }
    }
}