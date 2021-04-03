using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(STween), true)]
public class STweenEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
		
        STween tween = (STween)target;
        if (GUILayout.Button("Run"))
        {
            tween.Begin();
        }
        if (GUILayout.Button("Restore"))
        {
            tween.Restore();
        }

		this.serializedObject.ApplyModifiedProperties();
    }
}
