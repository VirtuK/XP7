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
    [SerializeField] private GameObject textBackground;
    private float timer = 1.5f;
    public bool timerActive;

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
        Research();
        textBackground = GameObject.Find("DialogueBox");
        textBackground.SetActive(false);
      
    }

    public void Research()
    {
        GameObject mensagem = GameObject.Find("Mensagem");
        text = mensagem.GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (timerActive)
        {
            timer -= Time.deltaTime;
        }
        if(timer <= 0)
        {
            CloseText();
        }
    }

    public void ShowText(string txt)
    {
        text.gameObject.SetActive(true);
        textBackground.SetActive(true);
        text.text = txt;
        timerActive = true;
    }

    public TMP_Text getText()
    {
        return text;
    }

    public void CloseText()
    {
        timerActive = false;
        timer = 1.5f;
        text.gameObject.SetActive(false);
        textBackground.SetActive(false);
    }

}
