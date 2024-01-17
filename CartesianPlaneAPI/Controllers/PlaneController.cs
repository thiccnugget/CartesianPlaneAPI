using CartesianPlaneAPI.Classes;
using CartesianPlaneAPI.Models;
using Microsoft.AspNetCore.Mvc;


namespace CartesianPlaneAPI.Controllers
{
    [Route("/")]
    [ApiController]
    public class PlaneController : Controller
    {
        //Set of unique, sorted points in the space
        private static SortedSet<Point> points = new SortedSet<Point>();

        #region GET


        /// <summary>
        /// Returns all points in the space
        /// </summary>
        /// <returns>A SortedSet of points</returns>
        [HttpGet("Space")]
        public async Task<ActionResult> GetSpacePoints()
        {
            return StatusCode(200, new APIResponse()
            {
                msg = $"{points.Count()} points in this space",
                data = points
            });
        }


        /// <summary>
        /// Returns all line segments passing through <paramref name="n"/> points
        /// </summary>
        /// <param name="n">Minimum number of points the segment must pass through</param>
        /// <returns>An HashSet of segments, represented by a SortedSet of points</returns>
        [HttpGet("Lines/{n?}")]
        public async Task<ActionResult> GetSpaceLines(int n)
        {
            return StatusCode(200, new APIResponse()
            {
                msg = $"Lines passing through {n} points.",
                data = GetLines(n)
            });
        }

            #endregion

        #region DELETE

        /// <summary>
        /// Deletes all points from the space
        /// </summary>
        [HttpDelete("Space")]
        public async Task<ActionResult> DeleteSpacePoints()
            {
                points.Clear();
                return StatusCode(200, new APIResponse()
                {
                    msg = "All points in space deleted"
                });
            }

            #endregion

        #region POST
        /// <summary>
        /// Creates a point in the space
        /// </summary>
        /// <param name="point">object with x and y numeric coordinates</param>
        /// <returns></returns>
        [HttpPost("Point")]
        public async Task<ActionResult> CreatePoint(CartesianPlaneDTO point)
        {
            bool added = points.Add(new Point(point.x, point.y));
            return added == true
                ? StatusCode(201, new APIResponse()
                {
                    msg = $"Point {point.x},{point.y} created"
                })
                : StatusCode(409, new APIResponse()
                {
                    msg = $"Point {point.x}, {point.y} already exists on the plane"
                });
        }

        #endregion



        #region UTILS
        /// <summary>
        /// Returns a set of sets of points each one representing a line segment 
        /// </summary>
        /// <param name="minPoints">Minimum number of points the segment must pass through</param>
        /// <returns></returns>
        HashSet<SortedSet<Point>> GetLines(int minPoints)
        {
            //Set of segments to return
            HashSet<SortedSet<Point>> segments = new HashSet<SortedSet<Point>>();


            //Cycle each point on the plane starting from the lowest X coordinate
            foreach(Point p1 in points)
            {
                //Cycle each point on the plane starting from the highest X coordinate
                foreach (Point p2 in points.Reverse())
                {
                    //check next point if points p1 and p2 are the same
                    if (p1.Equals(p2)) continue;

                    double slope;
                    double yIntercept;

                    try
                    {
                        //Calculate the equation of the line
                        //This may throw a DivisionByZero exception
                        slope = (double)(p2.y - p1.y) / (p2.x - p1.x);
                        yIntercept = p1.y - slope * p1.x;
                    }
                    catch
                    {
                        //Possible division by zero, skip and check next point
                        continue;
                    }

                    //Create the segment and add points p1 and p2
                    SortedSet<Point> segment = new SortedSet<Point> { p1, p2 };

                    //Add all filtered points that are not equal to p1 and p2 and lay on the same line to the segment
                    segment.UnionWith(points.Where(p => !p.Equals(p1) && !p.Equals(p2) && IsOnSameLine(p, slope, yIntercept)));
                    
                    //Add this segment to the set to be returned if the quantity of points it passes through is equal or greater than specified
                    //Also check if the current segment has not already been added to the segments set
                    if (segment.Count >= minPoints && !segments.Any(s => s.SetEquals(segment)))
                    {
                        segments.Add(segment);
                    }
                    
                }
            }

            return segments;
        }

        /// <summary>
        /// Check if point lays on the same line
        /// Must satisfy the equation: y = mx + q
        /// </summary>
        /// <param name="p"> point to check </param>
        /// <param name="slope"> line slope </param>
        /// <param name="yIntercept"> point where the line intercepts the y axis </param>
        /// <returns>true if the point lays on the same line, else returns false</returns>
        bool IsOnSameLine(Point p, double slope, double yIntercept) => p.y == (slope * p.x + yIntercept);


        //Use this method to test and populate the plane with random points, Case test only
        static SortedSet<Point> GenerateRandomPoints(int count)
        {
            Random random = new Random();
            SortedSet<Point> points = new SortedSet<Point>();

            while(points.Count() < count)
            {
                double x = Math.Round(random.NextDouble() * 100, 0)/2;
                double y = Math.Round(random.NextDouble() * 100, 0)/2;
                points.Add(new Point(x, y));
            }

            return points;
        }

        #endregion
    }
}

