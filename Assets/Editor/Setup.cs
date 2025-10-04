using System.IO;
using UnityEngine;
using UnityEditor;

public static class Setup
{
    [MenuItem("Tools/Setup/CreateFolderStructure")]
    public static void CreateDefaultFolders()
    {
        Folders.CreateDefault("_Project", "Animations" , "Art" , "Audio" , "Data", "Prefabs" , "Scenes" , "Scripts");

        Folders.CreateDefault("_Project/Art", "UI" , "Texture" , "Materials" , "Models");

        Folders.CreateDefault("_Project/Scenes", "GameScenes" , "TestLabs");

        Folders.CreateDefault("_Project/Audio", "BackgroundMusic", "SFX");

        Folders.CreateDefault("_Project/Scripts", "Shaders" , "Code");

        Folders.CreateDefault("_Project/Prefabs", "XR Prefabs", "System Prefabs" , "Partical Prefabs");

        AssetDatabase.Refresh();
    }


    [MenuItem("Tools/Setup/ImportAssets")]
    public static void ImportAssets()
    {
        Assets.ImportAsset("DOTween HOTween v2.unitypackage" , "Demigiant/Editor ExtensionsAnimation");
        Assets.ImportAsset("Colored Hierarchy Headers.unitypackage", "Baedrick/Editor ExtensionsUtilities");
        Assets.ImportAsset("CGHierarchyIcons.unitypackage", "Franco Rosatto/Editor ExtensionsUtilities");
        Assets.ImportAsset("Folder.Icons.v0.1.2.unitypackage", "WooshiiDev");
        Assets.ImportAsset("NaughtyAttributes.unitypackage", "Denis Rizov/Editor ExtensionsUtilities");



    }

    static class Folders
    {
        public static void CreateDefault(string root, params string[] folders) 
        {
            var fullpath = Path.Combine(Application.dataPath, root);
            foreach (var folder in folders) 
            {
                var path = Path.Combine(fullpath , folder);
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }
    }

    public static class Assets
    {
        public static void ImportAsset(string asset , string subfolder, string folder = "C:/Users/hussi/AppData/Roaming/Unity/Asset Store-5.x")
        {
            AssetDatabase.ImportPackage(Path.Combine(folder, subfolder, asset), false);
        }
    }
}
