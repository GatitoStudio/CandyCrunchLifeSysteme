using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class FuelSystem : MonoBehaviour
{
    [SerializeField]
    private int nbrLifeMax = 20;
    [SerializeField]
    private int nbrSecondePearLife = 6;
    [SerializeField]
    private int quantiteFuel = 0;
    [SerializeField]
    DateTime ancieneDate;
    [SerializeField]
    DateTime currentDate;
    [SerializeField]
    double tempsRestant;
    [SerializeField]
    double tempsEcouleAvantProchainFuel;
    [SerializeField]
    TextMeshProUGUI TempsRestant;
    [SerializeField]
    TextMeshProUGUI QuantObtenu;
    [SerializeField]
    Image fillImage;
    // Start is called before the first frame update
    void Start()
    {
      quantiteFuel =PlayerPrefs.HasKey("Fuel") ? Mathf.Clamp(PlayerPrefs.GetInt("Fuel"),0,nbrLifeMax) : 0;
      ancieneDate= PlayerPrefs.HasKey("Date") ? DateTime.Parse(PlayerPrefs.GetString("Date")) : DateTime.Now;
      currentDate = DateTime.Now;//ou coroutine pour get sur le net
      tempsRestant =(int) (nbrLifeMax * nbrSecondePearLife) - (quantiteFuel * nbrSecondePearLife) -(currentDate - ancieneDate).TotalSeconds;
      quantiteFuel = Mathf.Clamp(quantiteFuel + ((int)(currentDate - ancieneDate).TotalSeconds)/ nbrSecondePearLife,0,nbrLifeMax) ;
     tempsEcouleAvantProchainFuel = PlayerPrefs.HasKey("tempsEcouleAvantProchainFuel") ? double.Parse(PlayerPrefs.GetString("tempsEcouleAvantProchainFuel")) + (((int)(currentDate - ancieneDate).TotalSeconds) % nbrSecondePearLife) : 0;
     if (tempsRestant < 0) tempsRestant = 0;
      UpdateUi();
      StartCoroutine(Timer());
    }
  void UpdateUi()
    {
        QuantObtenu.text = quantiteFuel + "/" + nbrLifeMax;
        TempsRestant.text = new TimeSpan(0, 0, nbrSecondePearLife - (int)tempsEcouleAvantProchainFuel).ToString((@"hh\:mm\:ss"));
        fillImage.fillAmount =((float) (tempsEcouleAvantProchainFuel) / nbrSecondePearLife);
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("tempsEcouleAvantProchainFuel", tempsEcouleAvantProchainFuel.ToString());
        PlayerPrefs.SetInt("Fuel", quantiteFuel);
        PlayerPrefs.SetString("Date", DateTime.Now.ToString());//ou coroutine pour recupe sur le net ;
    }
    IEnumerator Timer()
    {
        while (true)
        {
            while(tempsRestant > 0) 
            {
                while(tempsEcouleAvantProchainFuel <= nbrSecondePearLife)
                {
                    tempsEcouleAvantProchainFuel += Time.deltaTime;
                    tempsRestant -= Time.deltaTime;
                    UpdateUi();
                    yield return null;
                }
                tempsEcouleAvantProchainFuel = 0;
                quantiteFuel++;
                UpdateUi();
            }
            yield return null;
        }
    }
}
