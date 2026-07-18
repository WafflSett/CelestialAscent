using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(DeckManager))]
public class DeckManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DeckManager deckManager = (DeckManager)target;
        if (GUILayout.Button("Draw next card"))
        {
            HandManager handManager = FindFirstObjectByType<HandManager>();
            if (handManager != null) { 
                deckManager.DrawCard(handManager);
            }
        }
    }

}
#endif
