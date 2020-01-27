using UnityEngine;
using System.Collections;
/*
 * Mesh Guage
 * Authors: Julian Dixon
 * Last Update: July 21 2016
 * 
 * This class functions similarly to Mesh Graph, once again, there is no chance
 * of understanding this unless you understand how meshes work first. Read up on unity's
 * documentation on their Mesh class, Mesh Filters, and Mesh Renderers first
 * 
 * This was mostly for the paw cmh2o guage displayed on the upper left side of the anesthesia
 * monitor, but you can attach any graph manager class to it
 */

public class MeshGuage : MonoBehaviour {
    // set these in the editor
    public Color baseColor;
    public Color fillColor;
    public GraphManager gm;

    // size variables, set them in the editor
    public float guageInnerRadius;
    public float guageWidth;

    // center of the circle that we draw our guage with
    private Vector3 guageCenter;
    // the angle we need to fill to, and each increment we need to make filling it
    private float angle;
    private float step;

    private Mesh mesh; // the mesh we use to draw the graph
    private MeshFilter mf; // mesh filter for our mesh

    private int fillQuadCount;							// 	Total number of quads we are making - needs to be 1 or above
    private int fillVerticesCount;						//	The number of vertices used across the whole fill ie the length of the vertices array
    private int fillTriangleCount;						//	The number of triangles we need to make the fill
    private int fillNormalCount;						//	The number of normals we are using
    private int fillQuadCounter = 0;					//	Used for the purposes of drawing the plots over time
    private Vector3[] fillVertices;					//	Storage: Vertex positions
    private Vector3[] fillNormals;						//	Storage: Vertex normals
    private int[] fillTriangles;						//	Storage: Vertex triangles
    private Color[] fillColors;						//	Storage: Vertex colors

    // we don't really need previous sum here since we arent making a fade effect like the trace
    private float currentSum;

    // Use this for initialization
    void Start ()
    {
        // get our gameObject's position and use it as the center to create the mesh
        guageCenter = this.gameObject.transform.position;
        CreateMesh();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        GetValues();
        UpdateMesh();
	}

    void CreateMesh()
    {
        // initialize the mesh
        mesh = new Mesh();
        // get our mesh filter and set its mesh property
        mf = GetComponent<MeshFilter>();
        mf.mesh = mesh;

        fillQuadCount = (int)(50 * guageWidth);         // 50 times the width of the guage since we use FixedUpdate
        fillTriangleCount = fillQuadCount * 6;          // each triangle has 3 vertices * 2 triangle per quad
        fillVerticesCount = fillQuadCount * 4;          // 4 vertices per quad

        fillVertices = new Vector3[fillVerticesCount];	//	Where we store the vertices of the fill
        fillNormals = new Vector3[fillVerticesCount];		//	Where we store the normals (facing direction) of the vertices for the quads we are making
        fillTriangles = new int[fillTriangleCount];		//	Where we store the triangles that make up the quads we are making
        fillColors = new Color[fillVerticesCount];		//	Where we store the color of the vertices

        int index = 0;

        // the angle we start at, since we want a horseshoe type of shape, start at 11pi/6
        angle = (11f / 6f) * Mathf.PI;
        // basically 30 degrees divided by our number of quads
        step = ((2f * Mathf.PI) - ((2f *Mathf.PI)/3f)) / fillQuadCount;

        // set up the vertices of the circle
        for (int n = 0; n < fillQuadCount; n++)
        {
            // each quad has 4 vertices
            // bottom left
            float xblpos = (guageInnerRadius + guageWidth) * Mathf.Cos(angle) + guageCenter.x;
            float yblpos = (guageInnerRadius + guageWidth) * Mathf.Sin(angle) + guageCenter.y;
            // top left
            float xtlpos = (guageInnerRadius + guageWidth) * Mathf.Cos(angle+step) + guageCenter.x;
            float ytlpos = (guageInnerRadius + guageWidth) * Mathf.Sin(angle+step) + guageCenter.y;
            // bottom right
            float xbrpos = guageInnerRadius * Mathf.Cos(angle) + guageCenter.x;
            float ybrpos = guageInnerRadius * Mathf.Sin(angle) + guageCenter.y;
            // top right
            float xtrpos = guageInnerRadius * Mathf.Cos(angle+step) + guageCenter.x;
            float ytrpos = guageInnerRadius * Mathf.Sin(angle+step) + guageCenter.y;

            // increment the angle
            angle += step;

            fillVertices[index] = new Vector3(xblpos, yblpos, 0.0f);     // bottom left
            fillColors[index] = baseColor;

            fillVertices[index+1] = new Vector3(xtlpos, ytlpos, 0.0f);      // top left
            fillColors[index+1] = baseColor;

            fillVertices[index + 2] = new Vector3(xbrpos, ybrpos, 0.0f);      // bottom right
            fillColors[index + 2] = baseColor;

            fillVertices[index + 3] = new Vector3(xtrpos, ytrpos, 0.0f);      // top right
            fillColors[index +3] = baseColor;

            // increment 4 at a time for each vertex
            index += 4;
        }

        index = 0;

        for (int n = 0; n < fillQuadCount; n++)			//	How the triangles are described:	
        {
            fillTriangles[index] = n * 4;				// 	1 - - - - 3  	ie: 
            fillTriangles[index + 1] = (n * 4) + 1;		// 	| .		  |		left triangle has three points: 0, 1 and 2
            fillTriangles[index + 2] = (n * 4) + 2;		//	|   .	  |		right triangle has three points: 2, 1 and 3
            fillTriangles[index + 3] = (n * 4) + 2;		//	|	  .   |
            fillTriangles[index + 4] = (n * 4) + 1;		//	|	    . |		note:	
            fillTriangles[index + 5] = (n * 4) + 3;		//	0 - - - - 2		we describe these 'clockwise'

            index += 6;
        }

        //	Now we need to assign the arrays we have made to the mesh

        mesh.vertices = fillVertices;
        mesh.normals = fillNormals;
        mesh.triangles = fillTriangles;
        mesh.colors = fillColors;
        
        // sometimes the mesh will end up facing the wrong way, these calls get it facing the camera
        // unity apparently doesnt render the back of a mesh
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    void GetValues()
    {
        currentSum = gm.currentSum;
    }

    void UpdateMesh()
    {
        fillVertices = mesh.vertices;
        fillColors = mesh.colors;

        // get our fill angle and scale it for display
        float fillAngleRadians = Mathf.Deg2Rad * (currentSum*1.75f);

        // use the y position of the bottom left vertex of the quads we need to fill to
        float fillybl = (guageInnerRadius + guageWidth) * Mathf.Sin(fillAngleRadians) + guageCenter.y;

        for (int n = 0; n < fillQuadCount; n+=4)
        {
            // the y bottom left works for when we are filling less than 90 degrees
            // don't need to fill more than that yet so I havent done it
            if (fillAngleRadians <= Mathf.PI / 2)
            {
                if (fillVertices[n].y <= fillybl)
                {
                    fillColors[n] = fillColor;
                    fillColors[n+1] = fillColor;
                    fillColors[n+2] = fillColor;
                    fillColors[n+3] = fillColor;
                }
                else
                {
                    fillColors[n+1] = baseColor;
                    fillColors[n+2] = baseColor;
                    fillColors[n+3] = baseColor;
                    fillColors[n+4] = baseColor;
                }
            }
        }

        // update our colors
        mesh.colors = fillColors;
    }
}
