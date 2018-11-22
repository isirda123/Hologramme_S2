using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangementScene : MonoBehaviour {

    GameObject[] NbOfRoom;
    int QuelScene = 0;

    // Use this for initialization
    void Start() {
        NbOfRoom = new GameObject[transform.childCount];
        int j = 0;
        for (int i =0; i<this.transform.childCount; i++)
        {
            
            if (transform.GetChild(i).gameObject.name.Contains("Test"))
            {
                NbOfRoom[j] = transform.GetChild(i).gameObject;
                NbOfRoom[j].gameObject.SetActive(false);
                j++;
            }
            
        }
        NbOfRoom[0].gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.N))
            ChangementSceneButton();
	}

    public void ChangementSceneButton()
    {
        NbOfRoom[QuelScene].gameObject.SetActive(false);
        QuelScene += 1;
        if (QuelScene >= NbOfRoom.Length)
        {
            QuelScene = 0;
            SceneManager.LoadScene(0);
        }
        NbOfRoom[QuelScene].gameObject.SetActive(true);
    }
}
