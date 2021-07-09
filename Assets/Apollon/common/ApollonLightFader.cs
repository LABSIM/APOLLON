
public class ApollonLightFader 
    : UnityEngine.MonoBehaviour
{


    [UnityEngine.SerializeField]
    private System.Collections.Generic.List<UnityEngine.Light> Lights 
        = new System.Collections.Generic.List<UnityEngine.Light>();
        
    private enum FadeAction
    {
        None,
        FadeIn,
        FadeOut
    } 
    private enum FadeStatus
    {
        Pending,
        Running
    } 

    private class FadeRequest
    {
        public FadeAction m_fadeType = FadeAction.None;
        public float m_fadeDuration = 0.0f;
    }
    private FadeStatus m_fadeStatus = FadeStatus.Pending;
 
    private System.Collections.Generic.Queue<FadeRequest> m_requestQueue = new System.Collections.Generic.Queue<FadeRequest>();


    private void Update()
    {
        // skip if running or if no pending request
        if(this.m_fadeStatus == FadeStatus.Running || this.m_requestQueue.Count == 0)
            return;

        if (this.m_requestQueue.Peek().m_fadeType == FadeAction.FadeIn)
        {

            this.StartCoroutine(this.FadeIn());

        }
        else if (this.m_requestQueue.Peek().m_fadeType == FadeAction.FadeOut)
        {

            this.StartCoroutine(this.FadeOut());

        } /* if() */

    } /* Update() */
 
    // fade from transparent to opaque
    private System.Collections.IEnumerator FadeIn()
    {
        // mark as Running
        this.m_fadeStatus = FadeStatus.Running;
    
        // extract last one
        FadeRequest current_request = null;
        lock(this.m_requestQueue) { current_request = this.m_requestQueue.Peek(); }

        // loop over m_fadeDuration time
        for (float i = 0.0f; i <= current_request.m_fadeDuration; i += UnityEngine.Time.deltaTime)
        {

            // smoothly update intensity
            foreach(var light in this.Lights)
            {
                
                light.intensity 
                    = UnityEngine.Mathf.Clamp(
                        (1.0f - i / current_request.m_fadeDuration),
                        0.0f,
                        1.0f
                    );

            } /* foreach() */
            
            // yield
            yield return null;

        } /* for() */

        // finally, zeroing intensity
        foreach(var light in this.Lights)
        {
            
            light.intensity = 0.0f;

        } /* foreach() */

        // flush last one
        lock(this.m_requestQueue)
        {

            // flush last one       
            this.m_requestQueue.Dequeue();
        
        }
        
        // mark as Pending
        this.m_fadeStatus = FadeStatus.Pending;
 
    } /* FadeIn() */
 
    // fade from opaque to transparent
    private System.Collections.IEnumerator FadeOut()
    {

        // mark as Running
        this.m_fadeStatus = FadeStatus.Running;

        // extract last one
        FadeRequest current_request = null;
        lock(this.m_requestQueue) { current_request = this.m_requestQueue.Peek(); }

        // loop over m_fadeDuration time
        for (float i = 0.0f; i <= current_request.m_fadeDuration; i += UnityEngine.Time.deltaTime)
        {

            // smoothly update intensity
            foreach(var light in this.Lights)
            {
                
                light.intensity 
                    = UnityEngine.Mathf.Clamp(
                        (i / current_request.m_fadeDuration),
                        0.0f,
                        1.0f
                    );

            } /* foreach() */

            // yield
            yield return null;

        } /* for() */

        // finally, set max intensity
        foreach(var light in this.Lights)
        {
            
            light.intensity = 1.0f;

        } /* foreach() */
 
        lock(this.m_requestQueue)
        {

            // flush last one       
            this.m_requestQueue.Dequeue();
        
        }

        // mark as Pending
        this.m_fadeStatus = FadeStatus.Pending;

    } /* FadeOut() */
 
    public void RequestFadeIn(float duration_in_ms)
    {

        lock(this.m_requestQueue)
        {

            // add a request to queue
            this.m_requestQueue.Enqueue( 
                new FadeRequest 
                { 
                    m_fadeDuration = duration_in_ms / 1000.0f, 
                    m_fadeType = FadeAction.FadeIn 
                }
            );

        } /* lock */

    } /* RequestFadeIn() */

    public void RequestFadeOut(float duration_in_ms)
    {

        lock(this.m_requestQueue)
        {

            // add a request to queue
            this.m_requestQueue.Enqueue( 
                new FadeRequest 
                { 
                    m_fadeDuration = duration_in_ms / 1000.0f, 
                    m_fadeType = FadeAction.FadeOut 
                }
            );

        } /* lock*/

    } /* RequestFadeOut() */

} /* class ApollonLightFader */
