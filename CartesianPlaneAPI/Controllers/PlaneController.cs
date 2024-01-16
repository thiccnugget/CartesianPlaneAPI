using CartesianPlaneAPI.Classes;
using CartesianPlaneAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace CartesianPlaneAPI.Controllers
{
    //[Route("[controller]")]
    [ApiController]
    public class PlaneController : Controller
    {
        //Set of unique points in the space
        //private static HashSet<Point> points = new HashSet<Point>();
        private static HashSet<Point> points = GenerateRandomPoints(100);

        #region GET

        [HttpGet("Space")]
        public async Task<ActionResult> GetSpacePoints()
        {
            return StatusCode(200, new APIResponse()
            {
                msg = $"{points.Count()} points in this space",
                data = points
            });
        }


        [HttpGet("Lines/{n}")]
        public async Task<ActionResult> GetSpaceLines(int n)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var data = GetLines(n);
            stopwatch.Stop();

            return StatusCode(200, new APIResponse()
            {
                msg = $"Lines passing through {n} points. Completed in {stopwatch.ElapsedMilliseconds} milliseconds",
                data = data
            });
        }

            #endregion

        #region DELETE

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

        [HttpPost("Point")]
        public async Task<ActionResult> CreatePoint(CartesianPlaneDTO point)
        {
            var status = points.Add(new Point(point.x, point.y));
            return status == true
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
        /// Returns a list of sets of points representing a line segment 
        /// </summary>
        /// <param name="minPoints">Minimum number of points the segment must pass through</param>
        /// <returns></returns>
        HashSet<HashSet<Point>> GetLines(int minPoints)
        {

            HashSet<HashSet<Point>> segments = new HashSet<HashSet<Point>>();

            foreach (var p1 in points)
            {
                foreach (var p2 in points.Reverse())
                {
                    if (p1.Equals(p2)) continue;

                    HashSet<Point> segment = new HashSet<Point> { p1, p2 };
                    double slope = 0;
                    double yIntercept = 0;

                    try
                    {
                        // Calculate the equation of the line
                        slope = (double)(p2.y - p1.y) / (p2.x - p1.x);
                        yIntercept = p1.y - slope * p1.x;
                    }
                    catch
                    {
                        continue;
                    }
                    
                    //Check how many / which points satisfy the equation
                    foreach (var p in points)
                    {
                        if (!p.Equals(p1) && !p.Equals(p2) && IsOnLine(p, slope, yIntercept))
                        {
                            segment.Add(p);
                        }
                    }

                    if (segment.Count >= minPoints && !segments.Any(s => s.SetEquals(segment)))
                    {
                        segments.Add(segment);
                    }
                                        
                }
            }

            return segments;
        }

        bool IsOnLine(Point p, double slope, double yIntercept)
        {
            return Math.Abs(p.y - (slope * p.x + yIntercept)) < double.Epsilon;
        }


        static HashSet<Point> GenerateRandomPoints(int count)
        {
            Random random = new Random();
            HashSet<Point> points = new HashSet<Point>();

            for (int i = 0; i < count; i++)
            {
                double x = Math.Round(random.NextDouble() * 100, 1); // Adjust the range as needed
                double y = Math.Round(random.NextDouble() * 100, 1); // Adjust the range as needed

                points.Add(new Point(x, y));
            }

            List<Point> t = points.ToList();
            t.Sort();

            return t.ToHashSet();
        }

        #endregion
    }
}

