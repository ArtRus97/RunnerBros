using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    public MainMenuController menuController;
    public GameObject BuyButton;
    public GameObject SelectButton;
    public GameObject SelectedButton;
    public GameObject coinImage;
    public GameObject coinText;
    public string characterName;
    private bool characterSelected = false;
    private bool characterBought = false;
    public int price;

    void Start(){
        if(PlayerPrefs.GetInt(characterName) == 1){ //tarkistetaan onko hahmo ostettu
            characterBought = true;
            coinImage.SetActive(false);
            coinText.SetActive(false);
            //määriteen hahmo ostetuksi ja laitetaan kolikon kuva ja teksi pois käytöstä
        }
    }

    void Update(){
        if(characterName == PlayerPrefs.GetString("playerCharacter")){
            characterSelected = true;
        }
        else{
            characterSelected = false;
        }
        //tarkistetaan muistiita onko hahmo valittu

        if(characterBought & !characterSelected){
            BuyButton.SetActive(false);
            SelectButton.SetActive(true);
            SelectedButton.SetActive(false);
        }
        else if(characterSelected){
            BuyButton.SetActive(false);
            SelectButton.SetActive(false);
            SelectedButton.SetActive(true);
        }
        else{
            BuyButton.SetActive(true);
            SelectButton.SetActive(false);
            SelectedButton.SetActive(false);
        }
        //laitetaan tietty nappi aktiiviseksi riipuen onko hahmo ostettu tai valittu
    }

    public void Buy(){ //hahmon ostokomento
        int playerCoins = PlayerPrefs.GetInt("playerCoins");
        //otetaan muistista kolikoiden määrä

        if(playerCoins >= price){ //tarkistetaan onko koikoit tarpeeksi
            PlayerPrefs.SetInt(characterName, 1); //määritetään muistiin hahmo ostetuksi
            playerCoins = playerCoins - price;
            PlayerPrefs.SetInt("playerCoins", playerCoins); //vähennetään muistista kolikoita
            characterBought = true;
            coinImage.SetActive(false);
            coinText.SetActive(false);
            menuController.CharacterBought();
        }
        else{
            menuController.NotEnoughCoins();
            //jos kolikoita ei ollut tarpeeksi kolikoita lähetään siitä viesti kontrollerille
        }
    }

    public void Select(){ //hahmon valintakomento
        menuController.PlayButtonSound();
        PlayerPrefs.SetString("playerCharacter", characterName);
    }


}
