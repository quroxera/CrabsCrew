using Scripts.Model;
using Scripts.UI.LevelsLoader;
using UnityEngine;

namespace Scripts.UI.VictoryScreen
{
    public class VictoryScreenController : MonoBehaviour
    {
        private LevelLoader _loader;
        private GameSession _session;
        private void Start()
        {
            _loader = FindObjectOfType<LevelLoader>();
            _session = FindObjectOfType<GameSession>();
            _session.OnGameComplete();
        }

        public void OnRestart()
        {
            _loader.LoadLevel("Level 1");
        }

        public void OnMainMenu()
        {
            _session.DestroyProgress();
            _loader.LoadLevel("MainMenu");
        }
    }
}