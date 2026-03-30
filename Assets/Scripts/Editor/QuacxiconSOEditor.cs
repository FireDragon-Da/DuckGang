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
        GUILayout.Label("Data Validation", EditorStyles.boldLabel);
        
        int totalLines = 0;
        int emptyCategories = 0;
        int categoriesWithSource = 0;
        
        foreach (var category in so.categories)
        {
            totalLines += category.contentList.Count;
            if (category.contentList.Count == 0)
                emptyCategories++;
            if (category.sourceTxtFile != null)
                categoriesWithSource++;
        }
        
        // Show validation info
        EditorGUILayout.HelpBox(
            $"Total Categories: {so.categories.Count}\n" +
            $"Total Lines Stored: {totalLines}\n" +
            $"Empty Categories: {emptyCategories}\n" +
            $"Categories with Source Files: {categoriesWithSource}",
            totalLines > 0 ? MessageType.Info : MessageType.Warning
        );
        
        // Warning if data is missing
        if (emptyCategories > 0 && categoriesWithSource > 0)
        {
            EditorGUILayout.HelpBox(
                "?? Some categories have source files but empty content lists!\n" +
                "Click 'Import all Txt' to load data into the ScriptableObject.",
                MessageType.Warning
            );
        }
        
        if (totalLines == 0 && categoriesWithSource > 0)
        {
            EditorGUILayout.HelpBox(
                "? NO DATA SAVED! This asset will be empty in builds!\n" +
                "You MUST click 'Import all Txt' and save the asset.",
                MessageType.Error
            );
        }
        
        GUILayout.Space(10);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Import Actions", EditorStyles.boldLabel);

        GUI.backgroundColor = Color.cyan; 
        if (GUILayout.Button("Import all Txt (Import All)", GUILayout.Height(35)))
        {
            so.ImportAll();
            EditorUtility.SetDirty(so);
            AssetDatabase.SaveAssets();
        }
        GUI.backgroundColor = Color.white; 
        
        GUILayout.Space(5);
        
        // Verify data button
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("? Verify Data Saved", GUILayout.Height(30)))
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            int verifyTotal = 0;
            foreach (var category in so.categories)
            {
                verifyTotal += category.contentList.Count;
            }
            
            if (verifyTotal > 0)
            {
                EditorUtility.DisplayDialog(
                    "Data Verification",
                    $"? SUCCESS!\n\n" +
                    $"Data is properly saved in the ScriptableObject.\n" +
                    $"Total lines: {verifyTotal}\n" +
                    $"Categories: {so.categories.Count}\n\n" +
                    $"This data will be available in builds.",
                    "OK"
                );
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "Data Verification",
                    "?? WARNING!\n\n" +
                    "No data found in contentLists.\n" +
                    "Please import data using 'Import all Txt' button.",
                    "OK"
                );
            }
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(10);

        foreach (var category in so.categories)
        {
            if (category.sourceTxtFile != null)
            {
                string buttonLabel = $"SingleImport -> {category.categoryName} ({category.contentList.Count} lines)";
                
                if (category.contentList.Count == 0)
                {
                    GUI.backgroundColor = Color.yellow;
                }
                
                if (GUILayout.Button(buttonLabel))
                {
                    category.ImportFromSource();
                    Debug.Log($"Imported: {category.categoryName}");
                    EditorUtility.SetDirty(so);
                    AssetDatabase.SaveAssets();
                }
                
                GUI.backgroundColor = Color.white;
            }
        }
    }
}
