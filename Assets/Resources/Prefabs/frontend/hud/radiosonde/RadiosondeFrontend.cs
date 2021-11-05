
public class RadiosondeFrontend
    : UnityEngine.MonoBehaviour
{

    public void SetValue(float value)
    {

        if(this.GetComponent<UnityEngine.UI.Slider>() != null) 
        {
            
            this.GetComponent<UnityEngine.UI.Slider>().value = value;

        }
        else if(this.GetComponent<UnityEngine.UI.Scrollbar>() != null)
        {

            this.GetComponent<UnityEngine.UI.Scrollbar>().value = UnityEngine.Mathf.Clamp(value, 0.0f, 40.0f) / 40.0f ;

        }
    
    }

}
