using UnityEngine;
using UnityEngine.Splines;
using System.IO;
using System.Text;

public class SplineExporter : MonoBehaviour
{
    public SplineContainer splineContainer;

    void Start()
    {
        ExportSplineToJson("Assets/splineData.json");
    }

    void ExportSplineToJson(string path)
    {
        var spline = splineContainer.Spline;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("{");
        sb.AppendLine("  \"points\": [");

        for (int i = 0; i < spline.Count; i++)
        {
            BezierKnot knot = spline[i];
            sb.AppendLine("    {");
            sb.AppendLine($"      \"position\": [{knot.Position.x}, {knot.Position.y}, {knot.Position.z}],");
            sb.AppendLine($"      \"tangentIn\": [{knot.TangentIn.x}, {knot.TangentIn.y}, {knot.TangentIn.z}],");
            sb.AppendLine($"      \"tangentOut\": [{knot.TangentOut.x}, {knot.TangentOut.y}, {knot.TangentOut.z}]");
            sb.Append(i < spline.Count - 1 ? "    }," : "    }");
            sb.AppendLine();
        }

        sb.AppendLine("  ]");
        sb.AppendLine("}");

        File.WriteAllText(path, sb.ToString());
        Debug.Log("Spline exported to JSON.");
    }
}
