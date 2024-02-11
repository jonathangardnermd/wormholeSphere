// using UnityEngine;
// using System.IO;


public class PolygonalCylinderSplay
{
    private Polygon polygon;
    private SplayData splayData;
    private float baseVertexRadius;
    private float zEnd;
    public MeshData meshData;

    public PolygonalCylinderSplay(Polygon polygon, float baseVertexRadius, SplayData splayData, float zEnd)
    {
        meshData = new();
        this.polygon = polygon; // the data 
        this.splayData = splayData; // the pts that define the curve that begins parallel to the cylinder's sides and ends parallel to the plane
        this.baseVertexRadius = baseVertexRadius; // the radius of the cylinder before any splaying
        this.zEnd = zEnd;
    }

    /* 
    This function will form the splay at the end of the cylinder, starting at z=0 and splaying into z>0.
    The splay will be formed by stacking polygons of increasing radii as z grows larger, and the 
    specific radii and z coords chosen will come from the "splayData", which calculates the radii
    and z coords so that the splay is parabolic.
    */
    public void BuildMeshData()
    {
        var prevVertexRadius = baseVertexRadius; // initialize the radius of the "previous" polygon to be the radius of the cylinder.
        var prevZ = zEnd; // the original cylinder goes from z=-length to z=0, so the splay will start at z=0 and splay into z>0

        float nextVertexRadius = -1f; // nextVertexRadius will generally be larger than prevVertexRadius as we stack larger and larger polygons at higher z-coords in the splay
        var totSplayLength = splayData.GetTotalChangeInY(); // the splay length in the z direction
        for (int splayLevel = 1; splayLevel <= this.splayData.numDivisions; splayLevel++)
        {
            var splayPt = splayData.xyChanges[splayLevel]; // get the amount of change in x and y (actually, x and z here) at this splay pt in the curved splay

            // the splay pts contain the total x and y changes from the end of cylinder to the end of the "splayLevel"
            // therefore, the radius at the end of this splayLevel is equal to splayPt.x plus the radius at the end of the cylinder (baseVertexRadius)
            nextVertexRadius = baseVertexRadius + splayPt.x;

            // and the nextZ coord is splayPt.y plus the z coordinate at the end of the cylinder (which is 0)
            var nextZ = zEnd - splayPt.y;

            // generate the vertices and triangles for the mesh between these two polygons of different radii and different z-coords
            PolygonCylinder.StackPolygons(meshData, polygon, totSplayLength, prevVertexRadius, nextVertexRadius, prevZ, nextZ);

            // stack the next polygon upon the polygon just stacked
            prevVertexRadius = nextVertexRadius;
            prevZ = nextZ;
        }
    }
}


