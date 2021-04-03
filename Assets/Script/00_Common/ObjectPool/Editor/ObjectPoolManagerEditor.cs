using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using ObjectPool;

[CustomEditor(typeof(ObjectPoolManager))]
public class ObjectPoolManagerEditor : Editor
{

    //////////////////////////////////////////////////////////////////////////////
    //public

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Manual Pool List");
        this.serializedObject.Update();
        this.poolList.DoLayoutList();
        this.uiPoolList.DoLayoutList();
        this.serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }

    //////////////////////////////////////////////////////////////////////////////
    //protected

    protected void OnEnable()
    {
        this.UpdateList();
    }

    //////////////////////////////////////////////////////////////////////////////
    //private

    private ReorderableList poolList;
    private ReorderableList uiPoolList;

    private void UpdateList()
    {
        ObjectPoolManager manager = (ObjectPoolManager)this.target;

        this.poolList = new ReorderableList(serializedObject, serializedObject.FindProperty("poolInfo"), true, true, true, true);

        poolList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight), "Pool Type");
            EditorGUI.LabelField(new Rect(rect.x + 160, rect.y, rect.size.x - 160 - 60, EditorGUIUtility.singleLineHeight), "Prefab");
            EditorGUI.LabelField(new Rect(rect.x + 160 + rect.size.x - 160 - 60, rect.y, 60, EditorGUIUtility.singleLineHeight), "Pool Size");
        };

        poolList.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = poolList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("poolType"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 160, rect.y, rect.size.x - 160 - 60, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("prefab"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 160 + rect.size.x - 160 - 60, rect.y, 60, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("poolSize"), GUIContent.none);
        };

        this.uiPoolList = new ReorderableList(serializedObject, serializedObject.FindProperty("uiPoolInfo"), true, true, true, true);

        uiPoolList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight), "Pool Type");
            EditorGUI.LabelField(new Rect(rect.x + 160, rect.y, rect.size.x - 160 - 60, EditorGUIUtility.singleLineHeight), "Prefab");
            EditorGUI.LabelField(new Rect(rect.x + 160 + rect.size.x - 160 - 60, rect.y, 60, EditorGUIUtility.singleLineHeight), "Pool Size");
        };

        uiPoolList.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = uiPoolList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("poolType"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 160, rect.y, rect.size.x - 160 - 60, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("prefab"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 160 + rect.size.x - 160 - 60, rect.y, 60, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("poolSize"), GUIContent.none);
        };
    }
}