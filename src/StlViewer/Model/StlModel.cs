using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace StlViewer.Model
{
    /// <summary>
    ///     StlModel
    /// </summary>
    public class StlModel
    {
        /// <summary>
        ///     Create StlModel Instance
        /// </summary>
        /// <param name="triangles">triangles</param>
        /// <param name="max">max values</param>
        /// <param name="min">min values</param>
        public StlModel(IEnumerable<Vector3D<Point3D>> triangles, Vector3D max, Vector3D min)
        {
            Triangles = triangles;
            Max = max;
            Min = min;
        }

        /// <summary>
        ///     Max Values
        /// </summary>
        public Vector3D Max { get; }

        /// <summary>
        ///     Min Values
        /// </summary>
        public Vector3D Min { get; }

        /// <summary>
        ///     Triangles
        /// </summary>
        public IEnumerable<Vector3D<Point3D>> Triangles { get; }
    }
}