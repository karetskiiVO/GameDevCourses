using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterLogic : MonoBehaviour {
    [SerializeField]GameObject instanse;
    TileLogic[][] field;
    System.Random rDevise = new System.Random();

    private TileLogic spawn (Vector3 position) {
        return GameObject.Instantiate(instanse, position, Quaternion.identity, transform).GetComponent<TileLogic>();
    }

    // Start is called before the first frame update
    void Start () {
        var zero    = Vector3.zero;
        var forward = Vector3.forward;
        var back    = Vector3.back;
        var left    = Vector3.left;
        var right   = Vector3.right;

        field = new TileLogic[][]{
            new TileLogic[]{spawn(left + forward), spawn(forward), spawn(right + forward)},
            new TileLogic[]{          spawn(left),    spawn(zero),           spawn(right)},
            new TileLogic[]{   spawn(left + back),    spawn(back),    spawn(right + back)},
        };
    }

    private void close () {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (field[i][j].fraction == 0) {
                    field[i][j].fraction = 4;
                } 
            }
        }
    }
    private void lockMaster () {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                field[i][j].masterTalks = true;
            }
        }
    }
    private void unlockMaster () {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                field[i][j].masterTalks = false;
            }
        }
    }

    private bool checkWinner () {
        for (int i = 0; i < 3; i++) {
            if (field[i][0].fraction == field[i][1].fraction && 
                field[i][1].fraction == field[i][2].fraction &&
                field[i][0].fraction != 0) {
                    field[i][0].fraction = 3;
                    field[i][1].fraction = 3;
                    field[i][2].fraction = 3;

                    close();
                    return true;
            }

            if (field[0][i].fraction == field[1][i].fraction && 
                field[1][i].fraction == field[2][i].fraction &&
                field[2][i].fraction != 0) {
                    field[0][i].fraction = 3;
                    field[1][i].fraction = 3;
                    field[2][i].fraction = 3;

                    close();
                    return true;
            }
        }

        if (field[0][0].fraction == field[1][1].fraction && 
            field[1][1].fraction == field[2][2].fraction &&
            field[2][2].fraction != 0) {
                field[0][0].fraction = 3;
                field[1][1].fraction = 3;
                field[2][2].fraction = 3;

                close();
                return true;
        }

        if (field[0][2].fraction == field[1][1].fraction && 
            field[1][1].fraction == field[2][0].fraction &&
            field[2][0].fraction != 0) {
                field[0][2].fraction = 3;
                field[1][1].fraction = 3;
                field[2][0].fraction = 3;

                close();
                return true;
        }

        return false;
    }

    private void randomMove () {
        var empty = new List<TileLogic>();

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (field[i][j].fraction == 0) {
                    empty.Add(field[i][j]);
                } 
            }
        }
        var idx = rDevise.Next(0, empty.Count);
        empty[idx].fraction = 2;
    }

    public void Refresh () {
        lockMaster();

        if (checkWinner()) {
            Debug.Log("Player wins!");
            return;
        }

        randomMove();

        if (checkWinner()) {
            Debug.Log("Random wins!");
            return;
        }

        unlockMaster();      
    }
}
