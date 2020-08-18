
// avoid namespace pollution
namespace Labsim.apollon.frontend.gui
{

    public class ApollonResponseGUIBehaviour : UnityEngine.MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        void OnEnable()
        {
            gameObject.GetComponent<Hover.InterfaceModules.Panel.HoverpanelRowTransitioner>().OnRowSwitched(Hover.InterfaceModules.Panel.HoverpanelRowSwitchingInfo.RowEntryType.SlideFromBottom);
        }

        void OnDisable()
        {
            gameObject.GetComponent<Hover.InterfaceModules.Panel.HoverpanelRowTransitioner>().TransitionProgress = 0.0f;
        }

        public void OnItemSelected(Hover.Core.Items.Types.IItemDataSelectable item)
        {

            // follow call !
            (experiment.ApollonExperimentManager.Instance.Profile as experiment.profile.ApollonAgencyAndTBWExperimentProfile).DoRespondToUserResponse(item.Id);

        } /* OnItemSelected() */

    } /* public class ApollonResponseGUIBehaviour */

} /* } Labsim.apollon.frontend.gui */