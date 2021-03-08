public class ObjectPosition 
    : UnityEngine.MonoBehaviour
{
    private int PosX;
    private int PosY;
    private float PosZ;

    private UnityEngine.Vector3 origin;
    //private float minY = 100.0f;
    //private float maxDistance = 200.0f;
    //public float taillePrefab = 5.0f; // find an alternative to this !

    public UnityEngine.GameObject cubePrefabs;


    void Awake()
    {

        // 2. Pour ligne n, découper en fonction des virgules pour recuperer PosX PosY & PosZ
        // 2.2 Ajouter delta a posY, permettant poser sur sol
        // 3. Instantiate objet à cette position

        System.Collections.Generic.List<
            System.Collections.Generic.Dictionary<
                string, 
                object
            >
        > data = CSVReader.Read("Databases/CAVIAR/Objets3D_test2");

        for (var i = 0; i < data.Count; i++)
        //for (var i = 0; i < 2; i++)
        {

            int PosX = ((int)data[i]["y_axis"]);
            //print(PosX);
            int PosZ = ((int)data[i]["x_axis"]);


            // Raycast pour obtenir position Y sur objet terrain
            UnityEngine.Vector3 origin = new UnityEngine.Vector3 (PosX, 1000.0f, PosZ);

            UnityEngine.RaycastHit hit;
            UnityEngine.Physics.Raycast(origin, -UnityEngine.Vector3.up, out hit, UnityEngine.Mathf.Infinity);

            //if (Physics.Raycast(origin, -Vector3.up, out hit, maxDistance))
            //print("Found an object - hit point x : " + hit.point.x + "  y :" + hit.point.y + "   z :" + hit.point.z);
            //float PosY = hit.point.y + (taillePrefab / 2);
            //float PosY = ((float)data[i]["z_axis"]);

            // print("position x - " + PosX + "position Y - " + PosY + "   Position z - " + PosZ);

            UnityEngine.GameObject.Instantiate(
                original: cubePrefabs,
                //position: new UnityEngine.Vector3(PosX, hit.point.y, PosZ),
                position: hit.point,
                rotation: cubePrefabs.transform.rotation,
                parent: this.gameObject.transform
            );

        } /* for() */

    } /* Awake() */

} /* class ObjectPosition */
