using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GazeErrorInjector
{
    [CustomEditor(typeof(InjectorManager)), CanEditMultipleObjects]
    public class InjectorManagerEditor : Editor
    {
        public SerializedProperty activeProp, toggleProp, sdkProp, modeProp, gazeSettings, leftEyeSettings, rightEyeSettings;

        void OnEnable () 
        {
            // Setup the SerializedProperties
            sdkProp = serializedObject.FindProperty ("EyeTrackerSDK");
            toggleProp = serializedObject.FindProperty ("toggleKey");
            activeProp = serializedObject.FindProperty ("isActive");
            modeProp = serializedObject.FindProperty ("gazeMode");
            gazeSettings = serializedObject.FindProperty ("gazeSettings");
            leftEyeSettings = serializedObject.FindProperty ("leftEyeSettings");
            rightEyeSettings = serializedObject.FindProperty ("rightEyeSettings");
        }

        public override void OnInspectorGUI() 
        {
            serializedObject.Update ();

            if(Application.isPlaying)
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(sdkProp);
                GUI.enabled = true;
            }
            else
            {
                EditorGUILayout.PropertyField(sdkProp);
                EyeTrackerList sdk = (EyeTrackerList) sdkProp.enumValueIndex;
                string compilerFlag = GazeErrorInjectorConstants.GetEyeTrackerCompilerFlag(sdk);
                BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, compilerFlag);
            }
            
            EditorGUILayout.PropertyField(toggleProp);
            EditorGUILayout.PropertyField(activeProp);
            EditorGUILayout.PropertyField(modeProp);


            

            ErrorMode mode = (ErrorMode) modeProp.enumValueIndex;

            serializedObject.ApplyModifiedProperties ();

            switch(mode) 
            {

                case ErrorMode.None:
                    break;
                case ErrorMode.Independent:

                    EditorGUILayout.LabelField("Gaze", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(gazeSettings);
                    EditorGUILayout.LabelField("Left Eye", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(leftEyeSettings);
                    EditorGUILayout.LabelField("Right Eye", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(rightEyeSettings);
                    break;
                case ErrorMode.Dependent:
                    EditorGUILayout.LabelField("Left Eye", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(leftEyeSettings);
                    EditorGUILayout.LabelField("Right Eye", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(rightEyeSettings);
                    break;
            }
        }
    }
}