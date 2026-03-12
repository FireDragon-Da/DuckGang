using UnityEditor;
using UnityEngine;

public static class EditorTest
{
    [MenuItem("Tools/Test Runtime Reference")]
    public static void TestReference()
    {
        var testObject = ScriptableObject.CreateInstance<QuacxiconSO>();
        Debug.Log("Successfully referenced QuacxiconSO: " + (testObject != null));
        Object.DestroyImmediate(testObject);
    }
}
