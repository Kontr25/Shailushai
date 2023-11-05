using UnityEngine;
using UnityEngine.UI;
using YG;

namespace _Scripts.MyGames
{
    public class GoingToTheWebSite : MonoBehaviour
    {
        [SerializeField] private Button _gameButton;
        [SerializeField] private bool _isHasLocalization = false;

        [SerializeField] private string _gameID;
        private string _currentUrl;
        public bool IsHasLocalization => _isHasLocalization;

        private void Start()
        {
            _gameButton.onClick.AddListener(() => OpenSite(_currentUrl));
            YandexGame.GetDataEvent += GetData;
            
            if (YandexGame.SDKEnabled == true)
            {
                GetData();
            }
        }

        private void GetData()
        {
            print($"Domain = {YandexGame.EnvironmentData.domain}");
            _currentUrl = $"https://yandex.{YandexGame.EnvironmentData.domain}/games/app/{_gameID}?lang={YandexGame.EnvironmentData.language}";
        }

        private void OnDestroy()
        {
            _gameButton.onClick.RemoveListener(() => OpenSite(_currentUrl));
            YandexGame.GetDataEvent -= GetData;
        }

        private void OpenSite(string url)
        {
            Application.OpenURL(url);
        }
    }
}