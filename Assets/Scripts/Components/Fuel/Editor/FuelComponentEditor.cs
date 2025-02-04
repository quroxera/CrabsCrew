using UnityEditor;

namespace Scripts.Components.Fuel
{
    [CustomEditor(typeof(FuelComponent))]
    public class FuelComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var fuelComponent = (FuelComponent)target;

            EditorGUILayout.LabelField("Max Fuel Check", fuelComponent.MaxFuel.ToString());
            base.OnInspectorGUI();
        }
    }
}