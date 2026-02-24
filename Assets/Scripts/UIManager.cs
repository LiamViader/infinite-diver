using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI startGameText;
    public TextMeshProUGUI lastRecordText;
    public TextMeshProUGUI personalBestText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI error;
    public TextMeshProUGUI traveledDistanceText;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void UpdateGameplayDistanceText(int distance)
    {
        traveledDistanceText.text = distance.ToString() + "M";
    }

    public void ShowGameplayTexts()
    {
        traveledDistanceText.enabled = true;
    }

    public void HideStartTexts()
    {
        startGameText.enabled = false;
        lastRecordText.enabled = false;
        personalBestText.enabled = false;
        titleText.enabled = false;
    }
    public void HideGameplayTexts()
    {
        traveledDistanceText.enabled = false;
    }

    public void ShowStartTexts()
    {
        startGameText.enabled = true;
        lastRecordText.enabled = true;
        personalBestText.enabled = true;
        titleText.enabled = true;
    }

    public void SetLastRecordText(int distance)
    {
        lastRecordText.text = distance.ToString() + " M";
    }

    public void SetPBText(int distance)
    {
        personalBestText.text = "Personal Best: " + distance.ToString() + " M";
    }

    public void ShowError(string message)
    {
        error.text = message;
        error.enabled = true;
    }

}
