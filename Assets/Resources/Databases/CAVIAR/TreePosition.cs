
public class TreePosition 
    : UnityEngine.MonoBehaviour
{
    private int PosX;
    private int PosY;
    private float PosZ;

    private UnityEngine.Vector3 origin;
    //private float minY = 1000.0f;
    //private float maxDistance = 200.0f;
    //private float taillePrefab = 0.0f; // find an alternative to this !

    public UnityEngine.GameObject treePrefabs;

    void Awake()
    {

        //  Pour ligne n, découper en fonction des virgules pour recuperer PosX PosY & PosZ
        System.Collections.Generic.List<
            System.Collections.Generic.Dictionary<
                string, 
                object
            >
        > data = CSVReader.Read("Databases/CAVIAR/Arbres_test2");

        for (var i = 0; i < data.Count; i++)
        {

            int PosX = ((int)data[i]["y_axis"]);

            // Passage de Z-up à Y-up entre Matlab et Unity
            int PosZ = ((int)data[i]["x_axis"]);


            // Raycast pour obtenir position Y sur objet terrain
            //UnityEngine.Vector3 origin = this.transform.InverseTransformPoint(new UnityEngine.Vector3(PosX, minY, PosZ));
            UnityEngine.Vector3 origin = new UnityEngine.Vector3(PosX, 1000.0f, PosZ);

            UnityEngine.RaycastHit hit;
            UnityEngine.Physics.Raycast(origin, -UnityEngine.Vector3.up, out hit, UnityEngine.Mathf.Infinity);

            // 2.2 Ajouter taillePrefab a posY, permettant poser sur sol (position du 0 au centre objet)
            //float PosY = hit.point.y + (taillePrefab / 2);

            // Instantiate object at the defined position
            UnityEngine.GameObject.Instantiate(
                original: treePrefabs,
                //position: new UnityEngine.Vector3(PosX, hit.point.y, PosZ),
                position: hit.point,
                rotation: treePrefabs.transform.rotation,
                parent: this.gameObject.transform
            );

        } /* for() */
         
    } /* Awake() */

} /* class TreePosition */
