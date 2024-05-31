using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private Button changeSceneButton;

    private void Start()
    {
        changeSceneButton.onClick.AddListener(GetChangeSceneClick);
    }

    private void GetChangeSceneClick()
    {
        SceneManager.LoadScene(1);
    }
}
