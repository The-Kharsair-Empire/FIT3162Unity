namespace VRTK.Examples
{
    using UnityEngine;
    using VRTK.Highlighters;

    public class second : VRTK_InteractableObject
    {
        public aa rotator;
        private VRTK_ControllerReference controllerReference;
        private float impactMagnifier = 120f;
        private float controllerSpeed = 0f;
        private Rigidbody rigi;

        public void myInteract()
        {
            //if (rotator == null)
            //    rotator = GameObject.Find("TheCube").GetComponent<aa>();
            //rotator.Going = !rotator.Going;
            rotator.visibility();
            myHighlight();

        }

        //public override void StartUsing(VRTK_InteractUse currentUsingObject = null)
        //{

        //    base.StartUsing(currentUsingObject);
        //    if (rotator == null)
        //        rotator = GameObject.Find("TheCube").GetComponent<aa>();
        //    //            rotator.Going = !rotator.Going;
        //    rotator.visibility();
        //    myHighlight();

        //}

        //public override void StopUsing(VRTK_InteractUse previousUsingObject = null, bool resetUsingObjectState = true)
        //{
        //    base.StopUsing(previousUsingObject, resetUsingObjectState);
        //    if (rotator == null)
        //        rotator = GameObject.Find("TheCube").GetComponent<aa>();
        //    //            rotator.Going = !rotator.Going;
        //    rotator.visibility();
        //    myHighlight();
        //}

        protected void myHighlight()
        {
            VRTK_BaseHighlighter highligher = transform.GetComponentInChildren<VRTK_BaseHighlighter>();
            highligher.Initialise();
            highligher.Highlight(Color.yellow);
        }

        protected void Start()
        {
            rotator = GameObject.Find("TheCube").GetComponent<aa>();
            rigi = gameObject.GetComponent<Rigidbody>();
        }

        protected override void Update()
        {
            base.Update();
            controllerSpeed = VRTK_DeviceFinder.GetControllerVelocity(controllerReference).magnitude * impactMagnifier;
            if (controllerSpeed > 30)
            {

                rigi.useGravity = true;
                rigi.isKinematic = false;
                Destroy(gameObject, 2);
            }
        }

        public void monitorVelocity(VRTK_ControllerReference c)
        {
            controllerReference = c;
        }

        public void grabReleased()
        {
            controllerReference = null;
        }
    }
}