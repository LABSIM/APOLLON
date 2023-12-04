
// avoid namespace pollution
namespace Labsim.apollon.gameplay.element
{
    public class ApollonWorldElementBehaviour 
        : ApolloConcreteGameplayBehaviour<ApollonWorldElementBridge>
        , UnityEngine.ISerializationCallbackReceiver
    {

        private System.Collections.Generic.Dictionary<string, UnityEngine.GameObject> m_references
            = new System.Collections.Generic.Dictionary<string, UnityEngine.GameObject>();

        public System.Collections.Generic.Dictionary<string, UnityEngine.GameObject> References { 
            get 
            {
                return this.m_references;
            }
        }

        // Hide this, because it will not work correctly without the custom editor.
        [UnityEngine.SerializeField, UnityEngine.HideInInspector]
        private System.Collections.Generic.List<ResourceEntry> m_entries = new System.Collections.Generic.List<ResourceEntry>();

        // Dictionary -> List
        public void OnBeforeSerialize()
        {
            this.m_entries.Clear();
            // You can iterate a dictionary like this if you want.
            foreach (System.Collections.Generic.KeyValuePair<string, UnityEngine.GameObject> kvp in this.m_references)
            {
                this.m_entries.Add(new ResourceEntry(kvp.Key, kvp.Value));
            }
        }

        // List -> Dictionary
        public void OnAfterDeserialize()
        {
            this.m_references.Clear();
            foreach (ResourceEntry entry in this.m_entries)
            {
                this.m_references.Add(entry.key, entry.value);
            }
        }

        [System.Serializable]
        public class ResourceEntry
        {

            public string key;
            public UnityEngine.GameObject value;

            public ResourceEntry() { }

            public ResourceEntry(string key,  UnityEngine.GameObject value)
            {
                this.key = key;
                this.value = value;
            }

        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void Awake()
        {

            // // inactive by default
            // this.enabled = false;
            // this.gameObject.SetActive(false);

        } /* Awake() */

    } /* public class ApollonWorldElementBehaviour */

// protect
#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(ApollonWorldElementBehaviour))]
    public class ApollonWorldElementBehaviourEditor 
        : UnityEditor.Editor
    {

        private UnityEditorInternal.ReorderableList list;

        public void OnEnable()
        {

            list = new UnityEditorInternal.ReorderableList(
                serializedObject, 
                serializedObject.FindProperty("m_entries"),
                true, 
                true, 
                true, 
                true
            );

            list.drawElementCallback = (UnityEngine.Rect rect, int index, bool isActive, bool isFocused) =>
            {
                UnityEditor.SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                UnityEditor.EditorGUI.PropertyField(
                    new UnityEngine.Rect(
                        rect.x, 
                        rect.y, 
                        160, 
                        UnityEditor.EditorGUIUtility.singleLineHeight
                    ), 
                    element.FindPropertyRelative("key"), 
                    UnityEngine.GUIContent.none
                );
                UnityEditor.EditorGUI.PropertyField(
                    new UnityEngine.Rect(
                        rect.x + 170, 
                        rect.y, 160, 
                        UnityEditor.EditorGUIUtility.singleLineHeight
                    ), 
                    element.FindPropertyRelative("value"), 
                    UnityEngine.GUIContent.none
                );

            };

            list.onAddCallback = (UnityEditorInternal.ReorderableList list) =>
            {
                int index = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                list.index = index;

                // Important! When adding a new element to the list (and dictionary),
                // the newly created key must be unique.
                UnityEditor.SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                element.FindPropertyRelative("key").stringValue = System.Guid.NewGuid().ToString(); // Create some kind of unique key, like a GUID or adding a number to the last one.
            };

        } /* OnEnable() */

        public override void OnInspectorGUI()
        {

            serializedObject.Update();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

        } /* OnInspectorGUI() */

    } /* class ApollonWorldElementBehaviourEditor */

#endif // UNITY_EDITOR

} /* } Labsim.apollon.ui.element.behaviour */