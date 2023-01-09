using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GazeErrorInjector
{
    [CustomEditor(typeof(InjectorManager)), CanEditMultipleObjects]
    public class InjectorManagerEditor : Editor
    {
        public SerializedProperty 
        modeProp;

        void OnEnable () 
        {
            // Setup the SerializedProperties
            modeProp = serializedObject.FindProperty ("gazeMode");

        }

        // public override void OnInspectorGUI() 
        // {
        //     serializedObject.Update ();

        //     EditorGUILayout.PropertyField(modeProp);
        //     GazeMode mode = (GazeMode) modeProp.enumValueIndex;


        //     serializedObject.ApplyModifiedProperties ();

        //     switch(mode) 
        //     {
        //         //Independent Mode
        //         case GazeMode.Independent:
        //             //EditorGUILayout.LabelField("Gaze", EditorStyles.boldLabel);
        //             break;
        //         //Dependent Mode
        //         case GazeMode.Dependent:
        //             EditorGUILayout.LabelField("Right Eye", EditorStyles.boldLabel);
        //             EditorGUILayout.LabelField("Left Eye", EditorStyles.boldLabel);
        //             break;
        //     }
        // }
    }
}