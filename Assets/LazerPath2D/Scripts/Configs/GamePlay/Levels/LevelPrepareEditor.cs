using UnityEditor;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.Configs.GamePlay.Levels
{
    [CustomEditor(typeof(Level))]
    public class LevelPrepareEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawDefaultInspector();

            Level level = (Level)target;

            if(GUILayout.Button(" ToDistributeNodes"))
            {
                level.ToDistributeNodesInLists();

                EditorUtility.SetDirty(level);
            }
        }
    }
}
