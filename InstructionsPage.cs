using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsPage : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(2);
    }
}
