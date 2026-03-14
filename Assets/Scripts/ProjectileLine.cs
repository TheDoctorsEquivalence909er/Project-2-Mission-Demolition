using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProjectileLine : MonoBehaviour
{
    // List of all projectile lines in scene
    static List<ProjectileLine> PROJ_LINES = new List<ProjectileLine>();

    // How much older lines fade
    private const float DIM_MULT = 0.75f;

    private LineRenderer _line;
    private bool _drawing = true;
    private Projectile _projectile;

    void Start()
    {
        // Get LineRenderer
        _line = GetComponent<LineRenderer>();

        _line.positionCount = 1;
        _line.SetPosition(0, transform.position);

        // Find projectile in parent
        _projectile = GetComponentInParent<Projectile>();

        // Add this line to list
        ADD_LINE(this);
    }

    void FixedUpdate()
    {
        if (!_drawing) return;

        // Add new point to line
        _line.positionCount++;
        _line.SetPosition(_line.positionCount - 1, transform.position);

        // Stop drawing when projectile goes to sleep
        if (_projectile != null)
        {
            if (!_projectile.awake)   // ✅ FIXED LOGIC
            {
                _drawing = false;
                _projectile = null;
            }
        }
    }

    private void OnDestroy()
    {
        PROJ_LINES.Remove(this);
    }

    static void ADD_LINE(ProjectileLine newLine)
    {
        Color col;

        // Dim all existing lines
        foreach (ProjectileLine pl in PROJ_LINES)
        {
            if (pl == null) continue;

            col = pl._line.startColor;
            col *= DIM_MULT;

            pl._line.startColor = col;
            pl._line.endColor = col;
        }

        // Add new line
        PROJ_LINES.Add(newLine);
    }
}