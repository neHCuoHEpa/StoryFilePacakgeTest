using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryFiles
{

    public class Permissions : MonoBehaviour
    {

        IEnumerator Start()
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);

            if (Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                Debug.Log("Usage of microphone is permitted.");
            }
            else
            {
                Debug.LogWarning("Usage of microphone is not permitted.");
            }
        }
    }
}
