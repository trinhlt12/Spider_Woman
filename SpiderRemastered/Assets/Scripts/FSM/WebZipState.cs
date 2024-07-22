namespace SFRemastered
{
    public class WebZipState : StateBase
    {
        //Detecting edges:
        //Use between 3 and 5 line traces performed on every tick of the game
        //first, use a line trace directly from the camera
        //then use the normal of the hit object to send out another trace
        //either from directly above or in front of the first hit
        //Then compare the normals of both hit surfaces to see if they're different enough
        //to indicate an edge
        //If an edge was found, project the player's collision capsule onto the zip point
        //to make sure the player can fit on the ledge.
        //Check to see whether each point is directly in view and which point is closet
        //to the players line of sight.
        //Finally, the system compares the distance from the edge points is found
        //to the characters line of sight, to see which is nearest and then draws the hood
        //widget on this point.
        
        //cast a raycast -> if it hit an object -> detect the shape of that object 
        //to identify its sides -> draw a sphere gizmo on hit -> rotate the camera to check
        //whether that gizmo move along the side of object or not
    }
}