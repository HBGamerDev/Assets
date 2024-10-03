using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public List<Transform> players = new List<Transform>();

    public Vector3 offset;

    public float min;
    public float max;
    public float limit;

    public float top;
    public float bottom;
    public float left;
    public float right;

    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        if(players.Count == 0)
        {
            return;
        }

        foreach(Transform missing in players)
        {
            if(missing == null)
            {
                players.Remove(missing);
            }
        }    

        if(FindObjectOfType<GameManager>().go)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name != "ButtonTest")
            {
                if (players.Count <= 1)
                {
                    SceneManager.LoadScene("Victory");
                }
            }
        }

        Move();
        Zoom();
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = newPosition;
    }

    void Zoom()
    {
        float zoom = Mathf.Lerp(max, min, GreatestDistance() / limit);
        cam.orthographicSize = zoom;
    }

    float GreatestDistance()
    {
        Vector3 centerPoint = GetCenterPoint();

        var bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }
        transform.position = new Vector3( Mathf.Clamp(centerPoint.x, left / bounds.size.x, right / bounds.size.x), Mathf.Clamp(centerPoint.y, bottom / bounds.size.y, top / bounds.size.y), transform.position.z);

        return bounds.size.x + bounds.size.y;
    }

    Vector3 GetCenterPoint()
    {
        if(players.Count == 1)
        {
            return players[0].position;
        }

        var bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.center;
    }
}
