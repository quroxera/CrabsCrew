using Scripts.Utils;
using UnityEngine;

namespace Scripts.Components
{
    public class ShowWindowComponent : MonoBehaviour
    {
        [SerializeField] private string _path;
        public void Show()
        {
            WindowUtils.CreateWindow(_path);
        } 
    }
}