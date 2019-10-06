namespace VRTK.Examples
{
    using UnityEngine;
    using VRTK.Highlighters;


    public class aa : MonoBehaviour
    {
        //public bool Going = false;
        private VRTK_ControllerReference controllerReference;
        private float impactMagnifier = 120f;
        private float controllerSpeed = 0f;
        private Rigidbody rigi;

        public void Start()
        {
            rigi = gameObject.GetComponent<Rigidbody>();
            
        }

        public void Update()
        {
            //Debug.Log(gameObject.GetComponent<Rigidbody>().velocity);
            //controllerSpeed = VRTK_DeviceFinder.GetControllerVelocity(controllerReference).magnitude * impactMagnifier;
            //if (controllerSpeed > 60)
            //{
            //    rigi.isKinematic = false;
            //    //rigi.useGravity = true;
            //    //Destroy(gameObject, 10);
            //}


            //if (Going)
            //{
            //    if (transform.localScale[0] <= 0.1F)
            //    {
            //        transform.localScale += new Vector3(0.1F, 0.1F, 0.1F);
            //    }
            //    else
            //    {
            //        Going = false;
            //    }
            //}
        }

        public void visibility()
        {
            transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
            //    Renderer rend = gameObject.GetComponent<Renderer>();
            //    //rend.enabled = !rend.enabled;
            //    Going = rend.enabled;
            //    transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        }

        //public void monitorVelocity(VRTK_ControllerReference c)
        //{
        //    controllerReference = c;
        //}

        //public void grabReleased()
        //{
        //    controllerReference = null;
        //}
    }
}