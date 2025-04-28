using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ItemInspection : MonoBehaviour
{
    [Header("Item Settings")]
    public GameObject itemPrefab;
    private GameObject currentItem;
    private Transform currentItemTransform;
    public float distanceFromCamera = -2.0f;
    public float rotationSpeed = 5.0f;

    [Header("Shader Settings")]
    public Shader vignetteBlurShader;
    private Material vignetteBlurMaterial;
    private Camera mainCamera;
    private ItemInspectionRenderer cameraRendererScript;

    [Header("Transition Settings")]
    public float fadeDuration = 0.5f;
    public float targetScale = 1.5f;

    private Vector3 originalScale;
    private bool isInspecting = false;

    public static ItemInspection instance;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        DontDestroyOnLoad(gameObject);
        mainCamera = Camera.main;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(InitializeAfterSceneLoad());
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (isInspecting && currentItem != null)
        {
            if (Input.GetMouseButton(0))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                currentItemTransform.Rotate(Vector3.up, -mouseX * rotationSpeed, Space.World);
                currentItemTransform.Rotate(Vector3.right, mouseY * rotationSpeed, Space.World);
            }

            if (Input.GetMouseButtonDown(1))
            {
                EndInspection();
            }
        }
    }

    public void StartInspection(GameObject item)
    {
        if (vignetteBlurShader == null)
        {
            Debug.LogError("Shader not assigned!");
            return;
        }

        itemPrefab = item;

        if (currentItem == null)
        {
            currentItem = Instantiate(itemPrefab);
            currentItemTransform = currentItem.transform;

            currentItemTransform.position = mainCamera.transform.position + currentItemTransform.forward;
            currentItemTransform.rotation = Quaternion.identity;

            if (currentItem.GetComponent<Item>())
            {
                Destroy(currentItem.GetComponent<Item>());
                currentItem.tag = "Untagged";
            }

            originalScale = currentItemTransform.localScale;
            currentItemTransform.localScale = originalScale * 0.1f;

            SetupPostProcessing();

            StartCoroutine(FadeInInspection());

            isInspecting = true;
        }
    }

    public void EndInspection()
    {
        if (currentItem != null)
        {
            StartCoroutine(FadeOutInspection());
        }
    }

    private void SetupPostProcessing()
    {
        if (cameraRendererScript == null)
        {
            cameraRendererScript = mainCamera.gameObject.AddComponent<ItemInspectionRenderer>();
        }

        vignetteBlurMaterial = new Material(vignetteBlurShader);
        vignetteBlurMaterial.SetFloat("_VignetteIntensity", 0f);
        vignetteBlurMaterial.SetFloat("_BlurStrength", 0f);
        cameraRendererScript.SetMaterial(vignetteBlurMaterial);
    }

    private IEnumerator FadeInInspection()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;

            if (vignetteBlurMaterial != null)
            {
                vignetteBlurMaterial.SetFloat("_VignetteIntensity", Mathf.Lerp(0f, 0.7f, t));
                vignetteBlurMaterial.SetFloat("_BlurStrength", Mathf.Lerp(0f, 4f, t));
            }

            currentItemTransform.localScale = Vector3.Lerp(originalScale * 0.1f, originalScale * targetScale, t);

            time += Time.deltaTime;
            yield return null;
        }

        if (vignetteBlurMaterial != null)
        {
            vignetteBlurMaterial.SetFloat("_VignetteIntensity", 0.7f);
            vignetteBlurMaterial.SetFloat("_BlurStrength", 4f);
        }

        currentItemTransform.localScale = originalScale * targetScale;
    }

    private IEnumerator FadeOutInspection()
    {
        float time = 0f;
        Vector3 startScale = currentItemTransform.localScale;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;

            if (vignetteBlurMaterial != null)
            {
                vignetteBlurMaterial.SetFloat("_VignetteIntensity", Mathf.Lerp(0.7f, 0f, t));
                vignetteBlurMaterial.SetFloat("_BlurStrength", Mathf.Lerp(4f, 0f, t));
            }

            currentItemTransform.localScale = Vector3.Lerp(startScale, originalScale * 0.1f, t);

            time += Time.deltaTime;
            yield return null;
        }

        if (vignetteBlurMaterial != null)
        {
            vignetteBlurMaterial.SetFloat("_VignetteIntensity", 0f);
            vignetteBlurMaterial.SetFloat("_BlurStrength", 0f);
        }

        Destroy(currentItem);
        Destroy(vignetteBlurMaterial);
        Destroy(cameraRendererScript);
        isInspecting = false;
    }
}