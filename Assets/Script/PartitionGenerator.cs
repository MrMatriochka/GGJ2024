using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PartitionGenerator : MonoBehaviour
{
    public Transform[] tracks;
    public Transform[] finalTracks;
    public KeyCode[] keyToPress;
    public KeyCode[] keyToLongPress;
    public GameObject notePrefab;
    public GameObject longNotePrefab;
    public GameObject barreNotePrefab;
    public GameObject noteHolder;
    public AudioSource audioSource;

    public float beatTempo;
    bool hasStarted;

    public string prefabPath;
    public string prefabName;
    void Start()
    {
        beatTempo = beatTempo / 60f;
    }
    int id = 0;
    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            if (Input.anyKeyDown)
            {
                hasStarted = true;
                audioSource.Play();
            }
        }
        else
        {
            noteHolder.transform.localPosition += new Vector3(0, beatTempo * Time.deltaTime, 0);
        }

        for (int i = 0; i < tracks.Length; i++)
        {
            if (Input.GetKeyDown(keyToPress[i]))
            {
                GameObject note = Instantiate(notePrefab, tracks[i].position, Quaternion.Euler(90,0,90));
                note.transform.parent = finalTracks[i].transform;
                note.GetComponent<NoteObject>().keyToPress = keyToPress[i];
                note.name = id.ToString();
                id++;
            }
            if (Input.GetKeyDown(keyToLongPress[i]))
            {
                GameObject note = Instantiate(longNotePrefab, tracks[i].position, Quaternion.identity);
                note.GetComponent<NoteObject>().keyToPress = keyToPress[i];
                note.transform.parent = finalTracks[i].transform;
            }
            if (Input.GetKeyUp(keyToLongPress[i]))
            {
                GameObject note = Instantiate(longNotePrefab, tracks[i].position, Quaternion.identity);
                note.GetComponent<NoteObject>().keyToPress = keyToPress[i];
                note.transform.parent = finalTracks[i].transform;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject note = Instantiate(barreNotePrefab, tracks[0].parent.position, barreNotePrefab.transform.rotation);
            note.transform.parent = finalTracks[4].transform;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            EndMusic();
        }
    }

    void EndMusic()
    {
        noteHolder.transform.localScale = new Vector3(1,1,1);
        noteHolder.GetComponent<BeatScroller>().beatTempo = beatTempo*60;
        string path = prefabPath +"/"+ prefabName + ".prefab";
        path = AssetDatabase.GenerateUniqueAssetPath(path);
        PrefabUtility.SaveAsPrefabAsset(noteHolder, path);
    }
}
