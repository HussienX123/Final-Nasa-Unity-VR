using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using TMPro;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class NarrationBlock
{
    [Header("Narrtion Section")]
    [TextArea]
    public string NarrationText;
    public AudioClip NarrationAudio;

    [Header("Narrtion Events Timers")]
    public float MessageStartEventTimer;
    public float AfterAuidoEndEventTimer;

    [Header("Narrtion Events")]
    public UnityEvent OnStartNarrationEvent;
    public UnityEvent OnEndAudioEvent;

    [Header("Narrtion Flow")]
    public bool MoveToNextBlock = false;
}

public class Narrator : MonoBehaviour
{

    [ShowNonSerializedField] private int CurrentNarrtionBlockIndex = -1;

    private AudioSource source;
    public TextMeshPro CaptionText;

    [Foldout("Narrtion Settings")] public bool startNarrtionOnStart = false;
    [Foldout("Narrtion Settings")] public float startNarrtionDelay = 0;

    public UnityEvent OnMoveToNextBlockEvent;

    public List<NarrationBlock> Narrations;

    [HorizontalLine(color: EColor.Red)]
    public bool isDebug = false;

    private void Start()
    {
        if (GetComponent<AudioSource>() != null)
            source = GetComponent<AudioSource>();
        else
            source = gameObject.AddComponent<AudioSource>();


        if(startNarrtionOnStart)
        {
            Invoke(nameof(NextBlock) , startNarrtionDelay);
        }
    }

    [Button("Next Block")]
    public void NextBlock()
    {
        StartCoroutine(NextBlockCoroutine());
    }

    IEnumerator NextBlockCoroutine() 
    {
        CurrentNarrtionBlockIndex++;

        if (CurrentNarrtionBlockIndex == Narrations.Count)
            yield break;

        NarrationBlock CurrentBlock = Narrations[CurrentNarrtionBlockIndex];

        CaptionText.text = $"<mark=#000000aa>{CurrentBlock.NarrationText}</mark>";


        source.clip = CurrentBlock.NarrationAudio;
        source.Play();

        if (isDebug)
        {
            CurrentBlock.AfterAuidoEndEventTimer = 0;
            CurrentBlock.MessageStartEventTimer = 0;
            source.Stop();
        }

        yield return new WaitForSeconds(CurrentBlock.MessageStartEventTimer);
        CurrentBlock.OnStartNarrationEvent.Invoke();

        while(source.isPlaying)
        {
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(CurrentBlock.AfterAuidoEndEventTimer - CurrentBlock.MessageStartEventTimer);
        CurrentBlock.OnEndAudioEvent.Invoke();


        if (CurrentBlock.MoveToNextBlock)
            NextBlock();
    }

    //this function will move the narration to a spesific block index
    public void MoveToBlock(int index)
    {
        if (index < 0 || index >= Narrations.Count)
        {
            Debug.LogError("Invalid narration block index.");
            return;
        }
        CurrentNarrtionBlockIndex = index - 1; // -1 because we will increment it in the next line
        NextBlock();
    }

    private void Update()
    {
        CaptionText.transform.LookAt(Camera.main.transform);
    }

#if UNITY_EDITOR
        [ContextMenu("Save Text To File")]
        public void SaveTextsToFile()
        {

            // Combine path to save inside persistentDataPath
            string path = Path.Combine(Application.persistentDataPath, SceneManager.GetActiveScene().name + ".txt");


            // Use StringBuilder for efficiency
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (var item in Narrations)
            {
                if (!string.IsNullOrEmpty(item.NarrationText))
                {
                    sb.AppendLine(item.NarrationText);
                }
            }

            // Write all text to file
            File.WriteAllText(path, sb.ToString());

            Debug.Log($"Texts saved to: {path}");
        }

    [ContextMenu("Load Text From File")]
    public void LoadTextsFromFile()
    {
        string path = Path.Combine(Application.persistentDataPath, SceneManager.GetActiveScene().name + ".txt");

        if (!File.Exists(path))
        {
            Debug.LogError($"File not found: {path}");
            return;
        }

        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < Narrations.Count; i++)
        {
            if (i < lines.Length)
            {
                Narrations[i].NarrationText = lines[i];
            }
            else
            {
                Debug.LogWarning($"File has fewer lines than narration blocks. Block {i} not updated.");
                break;
            }
        }

        Debug.Log($"Texts loaded from: {path}");
    }

    [Button("Load Audio Clips from Folder")]
    public void LoadAudioClipsFromFolder()
    {
        // Ask for folder path
        string folderPath = EditorUtility.OpenFolderPanel("Select Audio Folder", Application.dataPath, "");

        if (string.IsNullOrEmpty(folderPath))
            return;

        // Convert absolute path to relative path (Unity's asset path)
        string relativePath = "Assets" + folderPath.Replace(Application.dataPath, "");

        // Get all audio files from folder
        string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] { relativePath });

        if (guids.Length == 0)
        {
            Debug.LogWarning("No audio clips found in the selected folder.");
            return;
        }

        // Load and assign clips to messageClip list
        for (int i = 0; i < Narrations.Count && i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
            Narrations[i].NarrationAudio = clip;
        }

        Debug.Log($"Loaded {Mathf.Min(Narrations.Count, guids.Length)} audio clips into NarrationBlocks.");
    }
#endif

}
