using UnityEngine;
using UnityEngine.UI;

public class WinningManager : MonoBehaviour {
    private bool isEnded = false;
    [SerializeField]GameObject output = null;

    private void Start () {
        output.SetActive(false);
    }

    public void SendWinningMessage (string msg) {
        if (isEnded) return;
        isEnded = true;
        output.SetActive(true);

        output.GetComponent<Text>().text = msg;
    } 

    public void ResetCompetition () {
        isEnded = false;
        output.SetActive(false);
    }

    public void StopCompetition () {
        isEnded = false;
        output.SetActive(false);
    }

    public void StartCompetition () {
        isEnded = false;
        output.SetActive(false);
    }
}
