using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject player;
    public float speed;
    [SerializeField] Transform target;
    //判定是否可以行走(插件) ,追擊速率(仍需要製作,先保留變數) , 製作可設定追擊目標


    [SerializeField]
    private Transform[] waypoints;
    //排列行走目標
    [SerializeField]
    private float moveSpeed = 1f;
    //正常行走速率
    private int waypointIndex = 0;

    public Vector2 deltaSpeed;
    public float delay = 0.5f; //waitingtime
    bool waiting = false;

    public float viewRadius;
    public float offset = 0;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    float warningValue = 0;

    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        // StartCoroutine(FindTargetsWithDelay(2f));
    }

    private void Update()
    {
        FindVisibleTargets();
        CheckLookingPlayer();
        if (!waiting)
            Move();
        if (Vector2.Distance(transform.position, player.transform.position) < 0.35)
        {
            Application.LoadLevel("Caught");
        }
    }

    private void Move()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        print($"可見：{visibleTargets.Count}  警戒值：{ warningValue }");
        if (warningValue >= .9f)
        {
            GetComponentInChildren<MeshRenderer>().material.color = new Color(1, 0, 1, .5f);

            agent.speed = 2;
            agent.SetDestination(player.transform.position);

            deltaSpeed = agent.desiredVelocity;
        }
        else
        {
            agent.SetDestination(waypoints[waypointIndex].transform.position);

            deltaSpeed = Vector2.MoveTowards(transform.position,
                waypoints[waypointIndex].transform.position,
               moveSpeed * Time.deltaTime) - (Vector2)transform.position;

            transform.position = Vector2.MoveTowards(transform.position,
                waypoints[waypointIndex].transform.position,
               moveSpeed * Time.deltaTime);

            if ((transform.localPosition - waypoints[waypointIndex].transform.localPosition).magnitude <= .0001f)
            {
                deltaSpeed = new Vector2(-Mathf.Sin(Mathf.Deg2Rad * waypoints[waypointIndex].localEulerAngles.z), Mathf.Cos(Mathf.Deg2Rad * waypoints[waypointIndex].localEulerAngles.z));
                if (!waiting)
                    StartCoroutine(Delay());
            }
        }

    }

    void CheckLookingPlayer()
    {
        if (visibleTargets.Count > 0)
        {
            warningValue = Mathf.Clamp01(warningValue += 1f * Time.deltaTime);
        }
        else
        {
            warningValue = Mathf.Clamp01(warningValue -= 1f * Time.deltaTime);
        }

        if (transform.Find("Warning Bar") != null)
        {
            UnityEngine.UI.Image bar = transform.Find("Warning Bar").GetComponentInChildren<UnityEngine.UI.Image>();
            bar.fillAmount = warningValue;
        }
    }

    IEnumerator Delay()
    {
        waiting = true;
        float t = 0f;
        while (t < delay && warningValue < .9f)
        {
            t += Time.deltaTime;
            yield return null;
        }

        waiting = false;
        if (warningValue >= .9f)
        {
            yield break;
        }

        if (waypointIndex == waypoints.Length - 1)
        {
            waypointIndex = 0;
        }
        else
        {
            waypointIndex++;
        }
    }

    void LateUpdate()
    {
        UpdateFaceTo();
        DrawFieldOfView();
    }

    void UpdateFaceTo()
    {
        offset = -Vector2.SignedAngle(Vector2.up, GetComponent<FieldOfView>().deltaSpeed.normalized);
    }

    void FindVisibleTargets()
    {

        if (visibleTargets.Count > 0)
        {
            GetComponentInChildren<MeshRenderer>().material.color = new Color(1, 0, 1, .5f);
        }
        else
        {
            GetComponentInChildren<MeshRenderer>().material.color = new Color(.9f, .9f, .1f, .4f); //這邊調viewcone顏色
        }

        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            dirToTarget.z = 0;

            float angle = GetAngle(new Vector3(transform.forward.x, transform.forward.y, 0), dirToTarget);
            angle += angle > 0 ? -180 : 180;
            angle += offset;

            if (Mathf.Abs(angle) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    // 該方法接收 a 點與 b 點, a 點為物件位置, b 點為目標位置
    public float GetAngle(Vector2 a, Vector2 b)
    {
        // 這邊需要過濾掉位置相同的問題
        if (a.x == b.x && a.y >= b.y) return 0;

        b -= a;
        float angle = Mathf.Acos(-b.y / b.magnitude) * (180 / Mathf.PI);

        return (b.x < 0 ? -angle : angle);
    }


    private void OnDrawGizmos()
    {

    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }


    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }


    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin((angleInDegrees + offset) * Mathf.Deg2Rad), Mathf.Cos((angleInDegrees + offset) * Mathf.Deg2Rad), 0); //
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

}