
public class TreePosition 
    : UnityEngine.MonoBehaviour
{

    public UnityEngine.GameObject treePrefabs;

    private string m_seed_filename = "";

    private void Load(string seed_filename)
    {

        // log
        UnityEngine.Debug.Log(
            "<color=Blue>Info: </color> Resources.DB.CAVIAR.TreePosition.Load(" + seed_filename + ") : begin"
        );

        // check
        if (this.m_seed_filename != seed_filename)
        {

            if(this.m_seed_filename != "")
            {

                // unload previous if any
                this.Unload();

            } /* if() */

            // assign new resources
            this.m_seed_filename = seed_filename;

        }
        else
        {
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> Resources.DB.CAVIAR.TreePosition.Load(" + seed_filename + ") : resources already loaded, skip"
            );

            return;

        } /* if() */

        // log
        UnityEngine.Debug.Log(
            "<color=Blue>Info: </color> Resources.DB.CAVIAR.TreePosition.Load(" + seed_filename + ") : loading resources"
        );

        // Pour ligne n, découper en fonction des virgules pour recuperer PosX PosY & PosZ
        System.Collections.Generic.List<
            System.Collections.Generic.Dictionary<
                string,
                object
            >
        > data = CSVReader.Read(this.m_seed_filename);

        for (int i = 0; i < data.Count; i++)
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

    } /* Load() */

    private void Unload()
    {

        // log
        UnityEngine.Debug.Log(
            "<color=Blue>Info: </color> Resources.DB.CAVIAR.TreePosition.Unload() : unloading resources [" + this.m_seed_filename + "]"
        );

        foreach (UnityEngine.Transform child in this.gameObject.transform)
        {
            // destroy every clones
            UnityEngine.GameObject.DestroyImmediate(child.gameObject);
        }

        this.m_seed_filename = "";

    } /* Unload() */

    private void Awake()
    {

        // log
        UnityEngine.Debug.Log(
            "<color=Blue>Info: </color> Resources.DB.CAVIAR.TreePosition.Awake() : begin"
        );

        this.Load(
        "Databases/CAVIAR/"
            + Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.settings.GetString(
                "database_tree_seed_file"
            )
        );

        // log
        UnityEngine.Debug.Log(
            "<color=Blue>Info: </color> Resources.DB.CAVIAR.TreePosition.Awake() : end"
        );

    } /* Awake() */

    private void OnEnable()
    {

        // log
        UnityEngine.Debug.Log(
            "<color=Blue>Info: </color> Resources.DB.CAVIAR.TreePosition.OnEnable() : begin"
        );

        // load resources
        this.Load(
            "Databases/CAVIAR/"
            + Labsim.apollon.experiment.ApollonExperimentManager.Instance.Trial.settings.GetString(
                "database_tree_seed_file"
            )
        );

        // log
        UnityEngine.Debug.Log(
            "<color=Blue>Info: </color> Resources.DB.CAVIAR.TreePosition.OnEnable() : end"
        );

    } /* OnEnable() */

    private void OnDisable()
    {

        // log
        UnityEngine.Debug.Log(
            "<color=Blue>Info: </color> Resources.DB.CAVIAR.TreePosition.OnDisable() : begin"
        );

        // unload resources
        this.Unload();

        // log
        UnityEngine.Debug.Log(
            "<color=Blue>Info: </color> Resources.DB.CAVIAR.TreePosition.OnDisable() : end"
        );

    } /* OnDisable() */

} /* class TreePosition */
