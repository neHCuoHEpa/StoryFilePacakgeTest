using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static StoryFiles.StoryFilesInfo;

namespace StoryFiles
{

    [CustomEditor(typeof(SFBillboard))]
    public class SFBillboardEditor : Editor
    {

        private SFBillboard billboard;

        void OnEnable()
        {
            billboard = (SFBillboard)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("storyFileID"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("quality"), new GUIContent("Video Quality"));

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("environment"), new GUIContent("Environment"));
            if (EditorGUI.EndChangeCheck())
            {
                DisplayPushToTalkButton();
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("showPushToTalkButton"), new GUIContent("'Push To Talk' Button"));
            if (EditorGUI.EndChangeCheck())
            {
                DisplayPushToTalkButton();
            }


            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("showStarterQuestion"), new GUIContent("Starter Questions"));
            if (EditorGUI.EndChangeCheck())
            {
                DisplayStarterQuestions();
            }

            if (billboard.showStarterQuestion != StarterQuestionsPresenting.Never)
            {
                EditorGUI.BeginChangeCheck();

                //EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.LabelField("Starter Questions Position");
                //billboard.starterQuestionPosition = (StarterQuestionsPosition)EditorGUILayout.EnumPopup(billboard.starterQuestionPosition);
                //EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("starterQuestionPosition"), new GUIContent("Starter Questions Position"));
                if (EditorGUI.EndChangeCheck())
                {
                    ChangeStarterQuestionPosition();
                    ChangeStartersShowButton();
                    DisplayStarterQuestions();
                }

                if (billboard.showStarterQuestion == StarterQuestionsPresenting.OnClick)
                {
                    EditorGUI.BeginChangeCheck();

                    //EditorGUILayout.BeginHorizontal();
                    //EditorGUILayout.LabelField("Show Buttpn Position");
                    //billboard.starterQuestionShowButtonPosition = (StarterQuestionsShowButtonPosition)EditorGUILayout.EnumPopup(billboard.starterQuestionShowButtonPosition);
                    //EditorGUILayout.EndHorizontal();

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("starterQuestionShowButtonPosition"), new GUIContent("Show Button Position"));

                    if (EditorGUI.EndChangeCheck())
                    {
                        ChangeStartersShowButton();
                        DisplayStarterQuestions();
                    }
                }
            }

            //billboard.debugLevelFlag = StoryFilesInfo.debugLevelFlag;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("debugLevelFlag"), new GUIContent("Debug Level"));
            EditorGUI.EndChangeCheck();
            StoryFilesInfo.debugLevelFlag = billboard.debugLevelFlag;

            //billboard.debugComponentsFlag = StoryFilesInfo.debugComponentsFlag;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("debugComponentsFlag"), new GUIContent("Debug Components"));
            EditorGUI.EndChangeCheck();
            StoryFilesInfo.debugComponentsFlag = billboard.debugComponentsFlag;

            if (GUILayout.Button("Change"))
            {
                DisplayPushToTalkButton();
                DisplayStarterQuestions();
                ChangeStarterQuestionPosition();
                ChangeStartersShowButton();
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void DisplayStarterQuestions()
        {
            var starterQuestionsPanel = billboard.GetComponentInChildren<StarterQuestionsPanel>(true);
            if (starterQuestionsPanel == null)
                return;
            if (starterQuestionsPanel.closeButton != null)
                starterQuestionsPanel.closeButton.SetActive(billboard.showStarterQuestion == StarterQuestionsPresenting.OnClick);
            if (starterQuestionsPanel.showButton != null)
                starterQuestionsPanel.showButton.SetActive(billboard.showStarterQuestion == StarterQuestionsPresenting.OnClick);
            starterQuestionsPanel.gameObject.SetActive(billboard.showStarterQuestion != StarterQuestionsPresenting.Never);
        }

        private void DisplayPushToTalkButton()
        {
            var button = billboard.GetComponentInChildren<SFPushToTalk>(true);
            if (button == null && billboard.transform.parent != null)
                button = billboard.transform.parent.GetComponentInChildren<SFPushToTalk>(true);
            if (button != null)
                button.gameObject.SetActive(billboard.showPushToTalkButton == PushToTalkVisibility.Enable);
        }

        public void ChangeStarterQuestionPosition()
        {
            var starterQuestionsPanel = billboard.GetComponentInChildren<StarterQuestionsPanel>();

            Transform parent = null;
            if (billboard.starterQuestionPosition == StarterQuestionsPosition.RightOutside && GameObject.Find("Outside-Right") != null)
            {
                parent = GameObject.Find("Outside-Right").transform;
            }
            if (billboard.starterQuestionPosition == StarterQuestionsPosition.RightInside && GameObject.Find("Inside-Right") != null)
            {
                parent = GameObject.Find("Inside-Right").transform;
            }
            if (billboard.starterQuestionPosition == StarterQuestionsPosition.LeftInside && GameObject.Find("Inside-Left") != null)
            {
                parent = GameObject.Find("Inside-Left").transform;
            }
            if (billboard.starterQuestionPosition == StarterQuestionsPosition.LeftOutside && GameObject.Find("Outside-Left") != null)
            {
                parent = GameObject.Find("Outside-Left").transform;
            }

            if (parent != null && starterQuestionsPanel != null)
            {
                var sqrt = starterQuestionsPanel.GetComponent<RectTransform>();
                var prt = parent.GetComponent<RectTransform>();
                sqrt.pivot = prt.pivot;
                sqrt.anchorMin = prt.anchorMin;
                sqrt.anchorMax = prt.anchorMax;
                sqrt.anchoredPosition = prt.anchoredPosition;
                sqrt.sizeDelta = prt.sizeDelta;
                sqrt.localPosition = prt.localPosition;

                var rotaiton = prt.localRotation.eulerAngles;
                var y = rotaiton.y * starterQuestionsPanel.GetComponentInParent<Canvas>().transform.localScale.y;
                sqrt.localRotation = Quaternion.Euler(0.0f, y, 0.0f); ;
                
            }
        }

        public void ChangeStartersShowButton()
        {
            var starterQuestionsPanel = billboard.GetComponentInChildren<StarterQuestionsPanel>();
            
            Transform parent = null;
            if (billboard.starterQuestionPosition == StarterQuestionsPosition.LeftOutside || billboard.starterQuestionPosition == StarterQuestionsPosition.LeftInside)
            {
                if (billboard.starterQuestionShowButtonPosition == StarterQuestionsShowButtonPosition.Top)
                {
                    parent = GameObject.Find("SQLeftTopPosition").transform;
                }
                else
                {
                    parent = GameObject.Find("SQLeftBottomPosition").transform;
                }

            }
            if (billboard.starterQuestionPosition == StarterQuestionsPosition.RightOutside || billboard.starterQuestionPosition == StarterQuestionsPosition.RightInside)
            {
                if (billboard.starterQuestionShowButtonPosition == StarterQuestionsShowButtonPosition.Top)
                {
                    parent = GameObject.Find("SQRightTopPosition").transform;
                }
                else
                {
                    parent = GameObject.Find("SQRightBottomPosition").transform;
                }

            }
            if (parent != null && starterQuestionsPanel != null && starterQuestionsPanel.showButton != null)
            {
                var brt = starterQuestionsPanel.showButton.GetComponent<RectTransform>();
                var prt = parent.GetComponent<RectTransform>();
                brt.pivot = prt.pivot;
                brt.anchorMin = prt.anchorMin;
                brt.anchorMax = prt.anchorMax;
                brt.anchoredPosition = prt.anchoredPosition;
                brt.sizeDelta = prt.sizeDelta;
            }
        }
    }
}