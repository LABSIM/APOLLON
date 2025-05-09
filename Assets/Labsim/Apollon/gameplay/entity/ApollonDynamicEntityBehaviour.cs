//
// APOLLON : immersive & interactive experimental protocol engine
// Copyright (C) 2021-2023  Nawfel KINANI 
// nawfel (dot) kinani at onera (dot) fr
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program; see the files COPYING and COPYING.LESSER.
// If not, see <http://www.gnu.org/licenses/>.
//

// avoid namespace pollution
namespace Labsim.apollon.gameplay.entity
{
    public class ApollonDynamicEntityBehaviour 
        : ApollonConcreteGameplayBehaviour<ApollonDynamicEntityBridge>
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

    } /* public class ApollonDynamicEntityBehaviour */

// protect
#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(ApollonDynamicEntityBehaviour))]
    public class ApollonDynamicEntityBehaviourEditor 
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

    } /* class ApollonDynamicEntityBehaviourEditor */

#endif // UNITY_EDITOR

} /* } Labsim.apollon.gameplay.entity */