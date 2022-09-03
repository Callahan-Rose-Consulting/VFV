using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private Camera miniMap;

    public static MiniMap instance;

    public GameObject miniMapImage;

    public LineRenderer line;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        miniMap = GameObject.Find("MiniMap").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("m"))
        {//Either turns both the minimap and camera OFF or ON
            miniMap.enabled = !miniMap.isActiveAndEnabled;
            miniMapImage.SetActive(miniMap.isActiveAndEnabled);
        }

        
        if (GameManager.instance.current_objective != null)
        {
            Vector3 start = GameManager.instance.current_objective.transform.position;

            Vector3 end = transform.position;

            start.z = 1;

            end.z = start.z;

            var positions = new Vector3[] { start, end };

            line.SetPositions(positions);
        }
        else 
        {
            Vector3 start = Vector3.zero;

            Vector3 end = Vector3.zero;

            start.z = 1;

            end.z = start.z;

            var positions = new Vector3[] { start, end };

            line.SetPositions(positions);
        }
    }
}
