using System;
using Scripts.Model;
using Scripts.UI.LevelsLoader;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.UI.Windows.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        private Action _closeAction;

        protected override void Start()
        {
            var timeScale = Time.timeScale;
            base.Start();
            
            Time.timeScale = timeScale;
            GameSession.IsPaused = false;
        }
        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }
        
        public void OnLanguages()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
        }

        public void OnStartGame()
        {
            _closeAction = () =>
            {
                var loader = FindObjectOfType<LevelLoader>();
                loader.LoadLevel("Level 1");
            };

            Close();
        }
        
        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };

            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            base.OnCloseAnimationComplete();
            _closeAction?.Invoke();
        }
    }
}