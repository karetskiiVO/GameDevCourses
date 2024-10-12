using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class MouseShellFiller : FieldFiller {
    [SerializeField]GameObject logicInstanceObj = null;
    FieldFiller logicInstance = null;

    public override Material[] Init (Field field) {
        this.field = field; 

        logicInstance = logicInstanceObj.GetComponent<FieldFiller>();
        var res = logicInstance.Init(field);

        fractionsInfo = logicInstance.fractionsInfo;
        defaultFractionIdx = logicInstance.DefaultFractionIdx;

        return res;
    }

    private void Update () {
        if (Input.GetMouseButton(0)) {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + 100*Vector3.forward);
            var cliked = Physics.Raycast(Camera.main.transform.position, mousePos - Camera.main.transform.position, out RaycastHit hitInfo);

            if (cliked) {
                if (hitInfo.collider.gameObject.TryGetComponent<Field>(out var currField)) {
                    var tile = currField.GetTileFromEdgeIdx(hitInfo.triangleIndex);

                    // TODO chnge
                    tile.fraction = fractionsInfo[1].fraction;
                    tile.Flush(false);
                }
            }
        }
    }

    public override void Fill () {
        logicInstance.Fill();
    }
}
