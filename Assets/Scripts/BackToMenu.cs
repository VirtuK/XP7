using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public float delay = 5f;
   
    void Start()
    {
        StartCoroutine(BackToMenuAfterDelay());
    }

    IEnumerator BackToMenuAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Menu");
    }
}
