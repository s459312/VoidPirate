using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;

    [Header("AsyncManager")]
    [SerializeField] private GameObject loader;

    public GameObject teleport;
    private bool isInRange;
    public bool isActive = false;
    public Animator anim;
    public string levelToLoad;
    public string realm;
    public string willBeBoss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    isActive = true;
        //}
        if (isActive)
        {
            TargetIndicator.instance.MarkTarget(teleport.transform);
            anim.SetTrigger("Active");
            if (isInRange)
            {
                isActive = false;
                //LoadLevelButton(levelToLoad);
                LoadDefinedLevel();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isInRange = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isInRange = false;
    }
    public void LoadLevelButton(string levelToLoad)
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelASync(levelToLoad));
    }
    IEnumerator LoadLevelASync(string levelToLoad)
    {

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }
    public void SetActive()
    {
        isActive = true;
    }

    public void SetLevelToLoad(string levelToLoad, string realm, string willBeBoss)
    {
        this.levelToLoad = levelToLoad;
        this.realm = realm;
        this.willBeBoss = willBeBoss;

        loader.GetComponent<AsyncLoader>().DefineRealm(realm);
        loader.GetComponent<AsyncLoader>().WillBeBossRoom(willBeBoss);
    }

    public void LoadDefinedLevel()
    {
        loader.GetComponent<AsyncLoader>().DefineRealm(realm);
        loader.GetComponent<AsyncLoader>().WillBeBossRoom(willBeBoss);
        loader.GetComponent<AsyncLoader>().LoadLevel(levelToLoad);
    }
}
