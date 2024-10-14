using UnityEngine;
using UnityEngine.UI;

public class ProgressWriter : MonoBehaviour {
    private Slider slider = null;
    private Field field = null;
    private float score = 0;
    private bool enabledCompetitive = false;
    private WinningManager winingManager = null;
    [SerializeField]string winningMessage;

    private void Start () {
        slider = gameObject.GetComponent<Slider>();
        slider.gameObject.SetActive(false);

        field = GameObject.Find("Field").GetComponent<Field>();
        winingManager = GameObject.Find("Judge").GetComponent<WinningManager>();
    }

    public void ResetCompetition () {
        score = 0;
        slider.value = score;
    }

    public void StartCompetition () {
        slider.gameObject.SetActive(true);
        enabledCompetitive = true;
        ResetCompetition();
    }

    public void StopCompetition () {
        enabledCompetitive = false;
        slider.gameObject.SetActive(false);
    }

    public void AddScore (int delta) {
        if (field.Paused) return;
        if (!enabledCompetitive) return;

        score += delta;
        score = Mathf.Min(score, slider.maxValue);
        slider.value = score;

        if (score == slider.maxValue) {
            winingManager.SendWinningMessage(winningMessage);
        }
    }
}
