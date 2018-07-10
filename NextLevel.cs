using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel : MonoBehaviour
{
    [SerializeField]
    private Button timeButton;
    [SerializeField]
    private Button bulletButton;

    public void LoadNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void rewardTime()
    {
        FindObjectOfType<AdManager>().rewardType = "time";
        FindObjectOfType<AdManager>().ShowRewardBasedAd();
        timeButton.interactable = false;
    }

    public void rewardBullet()
    {
        FindObjectOfType<AdManager>().rewardType = "bullet";
        FindObjectOfType<AdManager>().ShowRewardBasedAd();
        bulletButton.interactable = false;
    }

    public void EnableRewardButtons()
    {
        timeButton.interactable = true;
        bulletButton.interactable = true;
    }
}
