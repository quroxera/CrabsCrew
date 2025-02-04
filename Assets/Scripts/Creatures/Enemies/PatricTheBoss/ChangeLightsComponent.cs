using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Scripts.Creatures.Enemies.PatricTheBoss
{
    public class ChangeLightsComponent : MonoBehaviour
    {
        [SerializeField] private Light2D[] _lights;
        [ColorUsage(true, true)] [SerializeField] private Color[] _colors;
        
        [ContextMenu("Set Color")]
        public void SetColor(int stageIndex)
        {
            if (stageIndex < 0 || stageIndex >= _colors.Length)
                return;
            
            foreach (var light2D in _lights)
            {
                light2D.color = _colors[stageIndex];
            }
        }
    }
}