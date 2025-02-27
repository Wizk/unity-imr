#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public static class GeneratedSceneMenuItems
{
    [MenuItem("Scenes/MAIN")]
    private static void OpenMAIN()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/MAIN.unity");
        }
    }

}
#endif
