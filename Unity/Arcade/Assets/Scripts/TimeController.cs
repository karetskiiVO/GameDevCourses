using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {
    private Field field = null;
    private Slider slider = null;

    void Start () {
        field = GameObject.Find("Field").GetComponent<Field>();
        slider = gameObject.GetComponent<Slider>();
    }

    public void Handle () {
        field.UpdateTimeSpan((int)slider.value);
    }
}
