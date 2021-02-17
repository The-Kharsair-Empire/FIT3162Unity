using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////////////////////////////////////////////////////////////
//FileName: Arc.cs
//FileType: visual C# Source File

//Description: The data class that stores all the information of a path, including origin, destination and volume.
//Last modified on: 23/10/19
//////////////////////////////////////////////////////////////////////////////////////////

namespace ArcTemplate
{
	public class Arc
	{
		public string o; //Origin slic
		public string d; //Destination slic
		public float vol; //volume in float

        public Arc(string o, string d, float vol)
		{
			this.o = o;
			this.d = d;
			this.vol = vol;
		}

	}

}