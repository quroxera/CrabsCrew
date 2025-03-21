﻿using Scripts.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Components.LevelManagement
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var session = FindObjectOfType<GameSession>();
            session.LoadSessionData();

            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
