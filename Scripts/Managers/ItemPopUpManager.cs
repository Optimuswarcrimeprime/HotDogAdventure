using System.Collections;
using UnityEngine;
using TMPro;

public class ItemPopUpManager : MonoBehaviour
{
    public static ItemPopUpManager instance; 
    [Header("UI Elements")]
    public GameObject itemPopUpUI; 
    public TextMeshProUGUI itemNameText; 
    public UnityEngine.UI.Image itemIconImage; 

    [Header("Audio")]
    public AudioSource popUpAudio;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowItemPopUp(Item item)
    {
        itemNameText.text = item.itemName;
        itemIconImage.sprite = item.icon;

        itemPopUpUI.SetActive(true);

        if (popUpAudio != null)
        {
            popUpAudio.Play();
        }

        StartCoroutine(HidePopUpAfterDelay(2f));
    }

    private IEnumerator HidePopUpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        itemPopUpUI.SetActive(false);
    }
}