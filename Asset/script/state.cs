//namespace VRTK.Examples
//{
//    using UnityEngine;
//    using VRTK.Highlighters;

//    public class state : VRTK_InteractableObject
//    {
//        public aa rotator;

//        public override void StartUsing(VRTK_InteractUse currentUsingObject = null)
//        {
            
//            base.StartUsing(currentUsingObject);
//            //if (rotator == null)
//            //    rotator = GameObject.Find("TheCube").GetComponent<aa>();
////            rotator.Going = !rotator.Going;
//            rotator.visibility();
//            myHighlight();

//        }

//        public override void StopUsing(VRTK_InteractUse previousUsingObject = null, bool resetUsingObjectState = true)
//        {
//            base.StopUsing(previousUsingObject, resetUsingObjectState);
//            //if (rotator == null)
//            //    rotator = GameObject.Find("TheCube").GetComponent<aa>();
////            rotator.Going = !rotator.Going;
//            rotator.visibility();
//            myHighlight();
//        }

//        protected void myHighlight()
//        {
//            VRTK_BaseHighlighter highligher = transform.GetComponentInChildren<VRTK_BaseHighlighter>();
//            highligher.Initialise();
//            highligher.Highlight(Color.yellow);
//        }

//        protected void Start()
//        {
//            rotator = transform.Find("state").GetComponent<aa>();
//        }

//        protected override void Update()
//        {
//            base.Update();
//        }
//    }
//}