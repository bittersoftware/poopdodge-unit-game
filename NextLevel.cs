using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel : MonoBehaviour
{
    [SerializeField]
    private GameObject newRecord;
    [SerializeField]
    private Button timeButton;
    [SerializeField]
    private Button bulletButton;

    public void OnEnable()
    {
        Debug.Log("Next Level Screen OnEnable is OK");
        if (FindObjectOfType<GameManager>().isNewRecord)
        {
            newRecord.SetActive(true);
        }
    }

    public void LoadNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void rewardTime()
    {

#if UNITY_EDITOR
        FindObjectOfType<GameManager>().setRewardTime();
#elif UNITY_ANDROID
        if (FindObjectOfType<AdManager>().IsAdLoaded())
        {
            FindObjectOfType<AdManager>().rewardType = "time";
            FindObjectOfType<AdManager>().ShowRewardBasedAd();
            timeButton.interactable = false;
        }
#endif
    }

    public void rewardBullet()
    {
#if UNITY_EDITOR
        FindObjectOfType<GameManager>().setRewardBullet();
#elif UNITY_ANDROID
        if (FindObjectOfType<AdManager>().IsAdLoaded())
        {
            FindObjectOfType<AdManager>().rewardType = "bullet";
            FindObjectOfType<AdManager>().ShowRewardBasedAd();
            bulletButton.interactable = false;
        }
#endif
    }


    public void EnableRewardButtons()
    {
        timeButton.interactable = true;
        bulletButton.interactable = true;
    }

}
