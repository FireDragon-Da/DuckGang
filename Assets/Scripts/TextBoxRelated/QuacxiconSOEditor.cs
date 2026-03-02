using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuacxiconSO))]
public class QuacxiconSOEditor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        QuacxiconSO so = (QuacxiconSO)target;

        GUILayout.Space(20);

   
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Import Actions", EditorStyles.boldLabel);

 
        GUI.backgroundColor = Color.cyan; 
        if (GUILayout.Button("Import all Txt (Import All)", GUILayout.Height(35)))
        {
            so.ImportAll();
        }
        GUI.backgroundColor = Color.white; 
        GUILayout.Space(10);

    
        foreach (var category in so.categories)
        {
            if (category.sourceTxtFile != null)
            {
                if (GUILayout.Button($"SingleImport -> {category.categoryName}"))
                {
                    category.ImportFromSource();
                    Debug.Log($"Imported: {category.categoryName}");
                    EditorUtility.SetDirty(so);
                }
            }
        }
    }
}