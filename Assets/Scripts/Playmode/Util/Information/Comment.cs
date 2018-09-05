using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

#endif

namespace Playmode.Util.Information
{
    public class Comment : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private string _text;
#endif
    }

#if UNITY_EDITOR

    [CanEditMultipleObjects]
    [CustomEditor(typeof(Comment))]
    public class CommentEditor : Editor
    {
        private SerializedProperty _text;

        private void OnEnable()
        {
            _text = serializedObject.FindProperty("text");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _text.stringValue = EditorGUILayout.TextArea(_text.stringValue, GUILayout.MaxHeight(75));
            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}