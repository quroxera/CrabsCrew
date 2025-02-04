using Scripts.Model;
using Scripts.UI.LevelsLoader;
using UnityEngine;

namespace Scripts.Components.LevelManagement
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void Exit()
        {
            var session = FindObjectOfType<GameSession>();
            session.SaveSessionData();
            
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel(_sceneName);
        }
    }
}