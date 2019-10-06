namespace VRTK.Examples
{
    using UnityEngine;
    using VRTK.Highlighters;

    public class stateTest : VRTK_InteractableObject
    {
        //private aa rotator;
        private GameObject myState;

        public void myInteract()
        {
            if (myState == null)
            {
                myState = Instantiate(gameObject, new Vector3(5, 5, 0), Quaternion.identity);
                Destroy(myState.GetComponent<stateTest>());
                myState.GetComponent<checkLabel>().setLabel("State");
                Destroy(myState.transform.GetChild(myState.transform.childCount - 1).gameObject);
                Destroy(myState.GetComponent<VRTK_OutlineObjectCopyHighlighter>());
                //myState.AddComponent<aa>();
                myState.AddComponent<aa>().visibility();
                myState.AddComponent<VRTK_InteractableObject>().isGrabbable = true;
                myState.GetComponent<MeshCollider>().convex = true;
                myHighlight();
            }
            else
            {
                //myState.GetComponent<aa>().visibility();
                Destroy(myState);
                myState = null;
                myHighlight();
            }
        }

        //public override void StartUsing(VRTK_InteractUse currentUsingObject = null)
        //{
        //    base.StartUsing(currentUsingObject);
        //    myState = Instantiate(gameObject, new Vector3(0, 5, 0), Quaternion.identity);
        //    myState.AddComponent<checkLabel>().setLabel("State");
        //    myState.AddComponent<aa>().visibility();
        //    //if (rotator == null)
        //    //    rotator = GameObject.Find("TheCube").GetComponent<aa>();
        //    //            rotator.Going = !rotator.Going;
        //    myHighlight();

        //}

        //public override void StopUsing(VRTK_InteractUse previousUsingObject = null, bool resetUsingObjectState = true)
        //{
        //    base.StopUsing(previousUsingObject, resetUsingObjectState);
        //    //if (rotator == null)
        //    //    rotator = GameObject.Find("TheCube").GetComponent<aa>();
        //    //            rotator.Going = !rotator.Going;
        //    myState.GetComponent<aa>().visibility();
        //    Destroy(myState);
        //    myState = null;
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
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}