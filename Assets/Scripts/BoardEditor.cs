using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Board))]
public class BoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Board board = (Board)target;
        DrawDefaultInspector();
        if(GUILayout.Button("Generate Board"))
        {
            board.CreateBoardUI();
        }
        if (GUILayout.Button("Delete Board"))
        {
            board.DeleteBoard();
        }
    }
}
