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
    public GameObject noteHolder;

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
            }
        }
        else
        {
            noteHolder.transform.position += new Vector3(0, beatTempo * Time.deltaTime, 0);
        }

        for (int i = 0; i < tracks.Length; i++)
        {
            if (Input.GetKeyDown(keyToPress[i]))
            {
                GameObject note = Instantiate(notePrefab, tracks[i].position, Quaternion.identity);
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
