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
        this.polygon = polygon; // The cylinder has a polygonal cross-section, so the splay does as well
        this.splayData = splayData; // The points that define the curve that begins parallel to the plane and ends parallel to the cylinder's sides
        this.baseVertexRadius = baseVertexRadius; // The radius of the cylinder before any splaying
        this.zEnd = zEnd;
    }

    /* 
    This function will form the splay at the end of the cylinder, starting at z=zEnd and splaying into z<zEnd.
    The splay will be formed by stacking polygons of increasing radii as z decreases,
    with specific radii and z coordinates calculated from "splayData" to ensure a parabolic splay.
    */
    public void BuildMeshData()
    {
        var prevVertexRadius = baseVertexRadius; // Initialize the radius of the "previous" polygon to be the radius of the cylinder.
        var prevZ = zEnd; // The original cylinder goes from z=zEnd+length to z=zEnd, so the splay will start at z=zEnd and splay into z<zEnd

        var totSplayLength = splayData.GetTotalChangeInY(); // The splay length in the z direction
        for (int splayLevel = 1; splayLevel <= this.splayData.numDivisions; splayLevel++)
        {
            var splayPt = splayData.xyChanges[splayLevel]; // Get the amount of change in x and y (actually, x and z here) at this splay point in the curved splay

            // The splay points contain the total x and y changes from the end of the cylinder to the end of the "splayLevel".
            // Therefore, the radius at the end of this splayLevel is equal to splayPt.x plus the radius at the end of the cylinder (baseVertexRadius)
            float nextVertexRadius = baseVertexRadius + splayPt.x; // nextVertexRadius will generally be larger than prevVertexRadius as we stack larger and larger polygons at lower z-coordinates in the splay

            // The next Z coordinate is the z coordinate at the end of the cylinder (which is zEnd) plus splayPt.y
            var nextZ = zEnd - splayPt.y;

            // Generate the vertices and triangles for the mesh between these two polygons of different radii and different z-coordinates
            PolygonCylinder.StackPolygons(meshData, polygon, totSplayLength, prevVertexRadius, nextVertexRadius, prevZ, nextZ);

            // Stack the next polygon upon the polygon just stacked
            prevVertexRadius = nextVertexRadius;
            prevZ = nextZ;
        }
    }
}
