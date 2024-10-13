using UnityEngine;

public class MouseShellFiller : FieldFiller {
    [SerializeField]FieldFiller logicInstance = null;
    private int fillFractionIdx = 0;

    public override Material[] Init (Field field) {
        this.field = field; 

        var res = logicInstance.Init(field);

        fractionsInfo = logicInstance.fractionsInfo;
        defaultFractionIdx = logicInstance.DefaultFractionIdx;

        return res;
    }

    public void NextFraction () {
        fillFractionIdx = (fillFractionIdx + 1) % fractionsInfo.Length;
    }

    private void Update () {
        if (Input.GetMouseButton(0)) {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + 100*Vector3.forward);
            var cliked = Physics.Raycast(Camera.main.transform.position, mousePos - Camera.main.transform.position, out RaycastHit hitInfo);

            if (cliked) {
                if (hitInfo.collider.gameObject.TryGetComponent<Field>(out var currField)) {
                    var tile = currField.GetTileFromEdgeIdx(hitInfo.triangleIndex);

                    // TODO chnge
                    tile.fraction = fractionsInfo[fillFractionIdx].fraction;
                    tile.Flush(false);
                }
            }
        }
    }

    public override void Fill () {
        logicInstance.Fill();
    }
}
