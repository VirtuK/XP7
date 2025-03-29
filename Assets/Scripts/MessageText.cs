using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
[System.Serializable]
public class MessageText : MonoBehaviour
{
    public static MessageText instance;

    [SerializeField] private TMP_Text text;
    private float timer = 3f;
    private bool timerActive;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject mensagem = GameObject.Find("Mensagem");
        text = mensagem.GetComponent<TMP_Text>();
        mensagem.SetActive(false);
      
    }

    public void Research()
    {
        GameObject mensagem = GameObject.Find("Mensagem");
        text = mensagem.GetComponent<TMP_Text>();
        mensagem.SetActive(false);
    }

    private void Update()
    {
        if (timerActive)
        {
            timer -= Time.deltaTime;
        }
        if(timer <= 0)
        {
            timerActive = false;
            timer = 3f;
            text.gameObject.SetActive(false);
        }
    }

    public void ShowText(string txt)
    {
        text.gameObject.SetActive(true);
        text.text = txt;
        timerActive = true;
    }

    public TMP_Text getText()
    {
        return text;
    }

}
