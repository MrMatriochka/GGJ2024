using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PartitionGenerator : MonoBehaviour
{
    public Transform[] tracks;
    public Transform[] finalTracks;
    public KeyCode[] keyToPress;
    public GameObject notePrefab;
    public GameObject longNotePrefab;
    public GameObject barreNotePrefab;
    public GameObject noteHolder;
    public AudioSource audioSource;

    public float beatTempo;
    bool hasStarted;

    public string prefabPath;
    public string prefabName;
    
    int id = 0;

    float[] buffer = new float[5];
    public float timeForLongNote;
    Transform[] startTransformBuffer = new Transform[5];
    GameObject[] startNoteBuffer = new GameObject[5];
    void Start()
    {
        beatTempo = beatTempo / 60f;
    }
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
                startTransformBuffer[i] = note.transform;
                startNoteBuffer[i] = note;
            }
            if (Input.GetKey(keyToPress[i]))
            {
                buffer[i] += Time.deltaTime;
            }
            if (Input.GetKeyUp(keyToPress[i]))
            {
                if(buffer[i] >= timeForLongNote)
                {
                    Vector3 spawnPoint = new Vector3(
                        startTransformBuffer[i].position.x + (tracks[i].position.x - startTransformBuffer[i].position.x)/2,
                        startTransformBuffer[i].position.y + (tracks[i].position.y - startTransformBuffer[i].position.y) / 2,
                        startTransformBuffer[i].position.z + (tracks[i].position.z - startTransformBuffer[i].position.z) / 2
                        );
                    float noteLength = Vector3.Distance(tracks[i].position, startTransformBuffer[i].position);

                    GameObject note = Instantiate(longNotePrefab, finalTracks[i].transform);
                    note.transform.position = spawnPoint;
                    note.GetComponent<LongNoteObject>().keyToPress = keyToPress[i];
                    note.transform.localScale = new Vector3(noteLength, note.transform.localScale.y, note.transform.localScale.z);
                    
                    Destroy(startNoteBuffer[i]);
                }
                buffer[i] = 0;
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
