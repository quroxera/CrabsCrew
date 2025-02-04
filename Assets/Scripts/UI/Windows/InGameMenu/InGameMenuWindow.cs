using Scripts.Model;
using Scripts.Utils;
using UnityEngine.SceneManagement;

namespace Scripts.UI.Windows.InGameMenu
{
    public class InGameMenuWindow : AnimatedWindow
    {
        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnLanguages()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
        }
        
        public void OnExit()
        {
            SceneManager.LoadScene("MainMenu");

            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);
        }
    }
}