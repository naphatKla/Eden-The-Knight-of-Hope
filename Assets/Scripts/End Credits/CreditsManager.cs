using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private KeyCode keyExit;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyExit))
        {
            HadleEscInput();
        }
    }
    void HadleEscInput()
    {
        SceneManager.LoadScene("FirstPlayable");
    }
}
