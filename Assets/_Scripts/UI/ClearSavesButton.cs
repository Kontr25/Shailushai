using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace _Scripts.UI
{
    public class ClearSavesButton : MonoBehaviour
    {
        public void ClearSave()
        {
            YandexGame.ResetSaveProgress();
            YandexGame.SaveProgress();
            SceneManager.LoadScene(0);
        }
    }
}