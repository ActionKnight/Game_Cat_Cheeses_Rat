using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject CheesePrefab;
    private int TotalCheesesInScene;

    public int LivesLeft=3;

    int SpawnIndex;

    private GameObject[] Spawners;
    public GameObject GlobalCanvas;
    public GameObject WinPanel;

    bool TimerOn;
    float TimeLeft;

    public TextMeshProUGUI TimerText,CheeseCounterText,HeartText;
    private void Awake()
    {
        if (instance == null) { instance = this; }
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(1);
        LivesLeft = 3;
        SceneManager.sceneLoaded += OnSceneLoaded;
        WinPanel.SetActive(false);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0 && scene.buildIndex!=1)
        {
            GlobalCanvas.SetActive(true);
            SpawnIndex = 0;
            LivesLeft = 3;
            Spawners = GameObject.FindGameObjectsWithTag("Spawner");
            TotalCheesesInScene = Spawners.Length;
            SpawnNew();
            ResetTimer();
        }
        else
        {
            GlobalCanvas.SetActive(false);
            ResetTimer();
            TimerOn = false;
        }
    }

    public void SpawnNew()
    {
        if (SpawnIndex < Spawners.Length)
        {
            CheeseCounterText.text = SpawnIndex.ToString() + "/" + TotalCheesesInScene.ToString();
            var cheese = Instantiate(CheesePrefab, Spawners[SpawnIndex].transform.position, Quaternion.identity);
            SpawnIndex++;
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == 2) LoadNextLevel();
            else if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                WinPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
    private void Update()
    {
        if (LivesLeft == 0)
        {
            StartCoroutine("OnDeathReturn");
            
        }
        if (TimerOn)
        {
            TimeLeft -= Time.deltaTime;
            TimerText.text = ((int)TimeLeft).ToString();
        }
        if (TimeLeft <= 0)
        {
            EnemyAi.instance.Death(false);
        }

        HeartText.text = LivesLeft.ToString() + "/3";
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ResetTimer()
    {
        TimeLeft = 60;
        TimerOn = true;
    }

    IEnumerator OnDeathReturn()
    {
        yield return new WaitForSeconds(2.5f);
        LivesLeft = 3;
        SceneManager.LoadScene(1);
    }

    public void ResetGame()
    {
        Time.timeScale = 1;
        ResetTimer();
        TimerOn = false;
        SceneManager.LoadScene(1);
    }

}
