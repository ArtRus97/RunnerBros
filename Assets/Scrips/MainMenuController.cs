using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject shopMenu;
    public TextMeshProUGUI coinCount;
    public TextMeshProUGUI alertText;
    
    AudioSource audioSource;
    public AudioClip buyClip;
    public AudioClip failClip;
    public AudioClip buttonClip;

    void Start(){
        audioSource = GetComponent<AudioSource>();

        if(PlayerPrefs.GetInt("firstTime") != 1){
            PlayerPrefs.SetInt("virtualGuy", 1);
            PlayerPrefs.SetString("playerCharacter", "virtualGuy");
            PlayerPrefs.SetInt("firstTime", 1);
            //tarkistetaan pelataanko peliä ekaa kertaa
            //ja laitetaan yksi hahmoista valituksi
        }

        coinCount.text = PlayerPrefs.GetInt("playerCoins").ToString();
        //otetaan muististi kolikoiden määrä näkymiin
    }

    public void LoadRunner1(){
        PlayButtonSound();
        SceneManager.LoadScene(sceneName:"Runner1Scene");
        //vaihdetaan juoksupelinäkymään
    }

    public void OpenShop(){
        PlayButtonSound();
        mainMenu.SetActive(false);
        shopMenu.SetActive(true);
    }

    public void CloseShop(){
        PlayButtonSound();
        shopMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void CharacterBought(){
        alertText.text = "Character Bought!";
        audioSource.PlayOneShot(buyClip);
        coinCount.text = PlayerPrefs.GetInt("playerCoins").ToString();
        //vähennetään hahmon ostamiseen käytetyt kolikot muistista
    }

    public void NotEnoughCoins(){
        audioSource.PlayOneShot(failClip);
        alertText.text = "Not enough coins!";
    }

    public void PlayButtonSound(){
        audioSource.PlayOneShot(buttonClip);
    }
}
