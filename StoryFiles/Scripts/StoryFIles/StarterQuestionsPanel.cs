using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StoryFiles
{
    public class StarterQuestionsPanel : MonoBehaviour
    {
        public StarterQuestionButton questionButtonPrefab;
        public Transform questionContainer;
        public GameObject closeButton;
        public GameObject showButton;

        [HideInInspector]
        public SFBillboard sfBillboard;

        public void SetupStarterQuestions(Question[] questions)
        {
            while (questionContainer.childCount > 0)
            {
                var child = questionContainer.GetChild(0);
                child.parent = null;
                Destroy(child.gameObject);
            }

            //starterQuestionPanel.SetActive(user.conversationalStarters.question.Length > 0);

            foreach (var question in questions)
            {
                StarterQuestionButton qb = Instantiate(questionButtonPrefab, questionContainer);
                qb.label.text = question.text;
                qb.GetComponent<Button>().onClick.AddListener(() => { sfBillboard.Answer(question); });
            }
        }
    }
}