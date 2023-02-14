using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public PlayerRunnerController player;

    public float gameSpeed;
    public float difficulty;
    public bool gamePaused;

    public int coinCount;
    private float score;
    private float scoreMultiplier = 1.0f;

    public TextMeshProUGUI coinCounter;
    public TextMeshProUGUI TotalCoinCounter;
    public TextMeshProUGUI scoreCounter;
    public TextMeshProUGUI finalScoreCounter;
    public TextMeshProUGUI recordScoreCounter;
    public GameObject GameUI;
    public GameObject GameOverSceen;
    public GameObject PauseScreen;

    private const float PLAYER_DISTANCE_SPAWN_PLATFORM_PART = 20f;
    [SerializeField] private Transform startPlatform;
    [SerializeField] private Transform testingPlatform;
    [SerializeField] private List<Transform> platformListEasy;
    [SerializeField] private List<Transform> platformListMedium;
    [SerializeField] private List<Transform> platformListHard;
    private Vector3 previousEndPosition;
    private int platformCount;

    private enum Difficulty{
        Easy,
        Medium,
        Hard
    }

    void Start(){
        gamePaused = false;
        Time.timeScale = 1;
        
        previousEndPosition = startPlatform.Find("EndPosition").position;
        //otetaan aloitus kentän palasen loppukohta
        
        int startingSpawnPlatforms = 3;
        for (int i = 0; i < startingSpawnPlatforms; i++){
            SpawnPlatform();
            //luodaan aloksu kolme kentän palasta
        }

        if (testingPlatform != null) {
            Debug.Log("Using Testing Plarform");
        }
    }

    void Update(){
        scoreCounter.text = score.ToString("f0");
        //muutetaan käyttöliittymän pistetekstiä

        if(!gamePaused){ //jos peli ei ole pysäytetty
            score += Time.deltaTime * scoreMultiplier;
            //suurennetaan pisteitä riippuen ajasta ja moninkertojasta
            gameSpeed = gameSpeed + 0.00005f;
            //suurennetaan pelin nopeutta kokoajan

            if(Input.GetButtonDown("Cancel")) { Pause(); }
            //suoritetaan pelin pysäytys komento jas painetaan menunappia
        }
        else {
            if(Input.GetButtonDown("Cancel")) { Resume(); }
            //suoritetaans en sijaan pelin jatkamis komento jos peli on pyäystetty
        }

        transform.position += new Vector3(gameSpeed * difficulty * Time.deltaTime, 0, 0);
        //muutetaan kontrollerin sijaintia riippuen pelin vaikeudesta ja ajasta

        if (Vector3.Distance(player.GetPosition(), previousEndPosition) < PLAYER_DISTANCE_SPAWN_PLATFORM_PART){
            SpawnPlatform();
            //aloitetaan kenttien luominen riippuen kuinka kaukana pelaaja on edellisestä palasesta
        }
    }

    private Transform SpawnPlatform(Transform platform,Vector3 spawnPosition){
        Transform platformTransform = Instantiate(platform, spawnPosition, Quaternion.identity);
        return platformTransform;
        //luodaan ja palautetaan kentänpalanen
    }

    private void SpawnPlatform(){
        List<Transform> chosenPlatformList;

        switch (GetDifficulty()) { //valitaan kenttälista vaikeustason mukaan
            default:
            case Difficulty.Easy: chosenPlatformList = platformListEasy; break;
            case Difficulty.Medium: chosenPlatformList = platformListMedium; break;
            case Difficulty.Hard: chosenPlatformList = platformListHard; break;
        }

        Transform chosenPlatform = chosenPlatformList[Random.Range(0, platformListMedium.Count)];
        //valitaan sekalaisesti listalta kentän palanen

        if (testingPlatform != null) { //tarkistetaan onko testi kentänpala käytössä
            chosenPlatform = testingPlatform; //muutetaan kokolista testipalaksi
        }

        Transform previousPlatform = SpawnPlatform(chosenPlatform, previousEndPosition);
        //lähetään kentän luonti komennolle valittu palanen ja edellisen palasen loppukohta
        previousEndPosition = previousPlatform.Find("EndPosition").position;
        //etsitään kentänpalasen loppukohta, joka oli määritetty erikseen joka palaselle
        platformCount++; //suurennetaan palasten määrää
    }

    private Difficulty GetDifficulty(){
        if (platformCount >= 10 & platformCount < 20){
            scoreMultiplier = 1.5f;
            return Difficulty.Medium;
        }
        else if(platformCount >= 20){
            scoreMultiplier = 2.0f;
            return Difficulty.Hard;
        }
        else return Difficulty.Easy;
    }

    public void EndGame(){
        gamePaused = true;
        Time.timeScale = 0;
        //pysäytetään peli ja aika

        int totalCoins = PlayerPrefs.GetInt("playerCoins");
        totalCoins = totalCoins + coinCount;
        PlayerPrefs.SetInt("playerCoins", totalCoins);
        TotalCoinCounter.text = totalCoins.ToString();
        //laitetaan kerätyt kolikot talteen ja näytetän kaikki kolikot näkymiin

        finalScoreCounter.text = score.ToString("f0");
        //laitetaan lopulliset pisteet näkymiin
        if(score > PlayerPrefs.GetFloat("recordScore")){
            recordScoreCounter.text = score.ToString("f0");
            PlayerPrefs.SetFloat("recordScore", score);
            //tarkistetaan onko uusi pistemäärä suurempi kuin edellinen ennätys ja päivitetään se tarvittaessa
        }
        else{
            recordScoreCounter.text = PlayerPrefs.GetFloat("recordScore").ToString("f0");
            //muulloin laitetaan vanha ennätys näkymiin
        }

        GameUI.SetActive(false); //otetaan pelin käyttöliittymä pois käytöstä
        GameOverSceen.SetActive(true); //laitetaan pelin lopetusruutu aktiiviseksi
    }

    public void AddCoin(){
        coinCount++;
        coinCounter.text = coinCount.ToString();
    }

    public void Restart(){
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        gamePaused = false;
        Time.timeScale = 1;
    }

    public void Reset(){
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        gamePaused = false;
        Time.timeScale = 1;
    }

    public void Pause(){
        gamePaused = true;
        Time.timeScale = 0;
        PauseScreen.SetActive(true);
    }

    public void Resume(){
        gamePaused = false;
        Time.timeScale = 1;
        PauseScreen.SetActive(false);
    }

    public void Exit(){
        SceneManager.LoadScene(sceneName:"MainMenuScene");
    }
}
