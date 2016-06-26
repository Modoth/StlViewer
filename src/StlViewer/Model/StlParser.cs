using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;

namespace StlViewer.Model
{
    /// <summary>
    ///     Parse stl data form data
    /// </summary>
    public class StlParser
    {
        /// <summary>
        ///     Parse form stream data
        /// </summary>
        /// <param name="stream">data stream</param>
        /// <returns>null:parse failed</returns>
        public StlModel Parse(Stream stream)
        {
            try
            {
                using (var sr = new StreamReader(stream))
                {
                    string lineData;
                    var triangles = new List<Vector3D<Point3D>>();
                    var max = new Vector3D();
                    var min = new Vector3D();
                    var triangle = new Vector3D<Point3D>();
                    var trianglePointIdx = 0;
                    while ((lineData = sr.ReadLine()) != null)
                    {
                        lineData = lineData.Trim();

                        if (lineData.IndexOf("facet normal ", StringComparison.Ordinal) >= 0)
                        {
                            triangle = new Vector3D<Point3D>();
                        }
                        if (lineData.IndexOf("vertex ", StringComparison.Ordinal) < 0)
                        {
                            continue;
                        }
                        var lineTokens = lineData.Split(' ');
                        if (triangle == null)
                        {
                            return null;
                        }
                        var px = double.Parse(lineTokens[1]);
                        var py = double.Parse(lineTokens[2]);
                        var pz = double.Parse(lineTokens[3]);
                        var point = new Point3D(px, py, pz);
                        switch (trianglePointIdx)
                        {
                            case 0:
                                triangle.X = point;
                                trianglePointIdx++;
                                break;
                            case 1:
                                triangle.Y = point;
                                trianglePointIdx++;
                                break;
                            case 2:
                                triangle.Z = point;
                                triangles.Add(triangle);
                                triangle = null;
                                trianglePointIdx = 0;
                                break;
                        }
                        max.X = Math.Max(max.X, px);
                        min.X = Math.Min(min.X, px);
                        max.Y = Math.Max(max.Y, py);
                        min.Y = Math.Min(min.Y, py);
                        max.Z = Math.Max(max.Z, pz);
                        min.Z = Math.Min(min.Z, pz);
                    }
                    return new StlModel(triangles, max, min);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}