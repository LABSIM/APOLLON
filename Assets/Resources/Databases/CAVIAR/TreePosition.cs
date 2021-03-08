
public class TreePosition 
    : UnityEngine.MonoBehaviour
{

    public UnityEngine.GameObject treePrefabs;

    private void OnEnable()
    {

        // Pour ligne n, découper en fonction des virgules pour recuperer PosX PosY & PosZ
        System.Collections.Generic.List<
            System.Collections.Generic.Dictionary<
                string, 
                object
            >
        > data = CSVReader.Read(
            "Databases/CAVIAR/" 
                + Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.settings.GetString(
                    "database_tree_seed_file"
                )
        );

        for(int i = 0; i < data.Count; i++)
        {
            
            // Passage de Z-up à Y-up entre Matlab et Unity
            // & raycast pour obtenir position Y sur objet terrain
            UnityEngine.RaycastHit hit;
            UnityEngine.Physics.Raycast(
                new UnityEngine.Vector3(
                    ((int)data[i]["y_axis"]),
                    1000.0f,
                    ((int)data[i]["x_axis"])
                ),
                UnityEngine.Vector3.down, 
                out hit, 
                UnityEngine.Mathf.Infinity
            );

            // instantiate
            UnityEngine.GameObject.Instantiate(
                original:
                    treePrefabs,
                position:
                    hit.point,
                rotation:
                    treePrefabs.transform.rotation,
                parent:
                    this.gameObject.transform
            );

        } /* for() */

    } /* OnEnable() */

    private void OnDisable()
    {

        // check if there is previous children 
        foreach (UnityEngine.Transform child in this.gameObject.transform)
        {
            // destroy every clones
            UnityEngine.GameObject.Destroy(child.gameObject);
        }

    } /* OnDisable() */

} /* class TreePosition */
