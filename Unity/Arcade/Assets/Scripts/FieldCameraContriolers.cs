using UnityEngine;

public abstract class FieldCameraController {
    protected const float velocity = 100f;

    public abstract void HandleInput (Transform transform);
}

public class SphereCameraController : FieldCameraController {
    private readonly Vector3 center;
    private readonly float minRadius;

    public SphereCameraController (Vector3 center, float minRadius) {
        this.center = center;
        this.minRadius = minRadius;

        if (minRadius < 0) throw new System.Exception("minRadius can't be below zero");
    }
    
    public override void HandleInput (Transform transform) {
        float radius = (transform.position - center).magnitude;

        var movementVector = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.up * Input.GetAxisRaw("Vertical");
        movementVector = velocity * Time.deltaTime * (radius / 100 * movementVector.normalized + 3 * (Input.mouseScrollDelta.y * Vector3.forward));

        transform.position += transform.right * movementVector.x + transform.forward * movementVector.z + transform.up * movementVector.y;
        transform.forward = center - transform.position;

        if (radius < minRadius) transform.position = transform.position.normalized * minRadius;
    }
}

public class PlaneCameraController : FieldCameraController {
    private Vector3 normal;

    public PlaneCameraController (Vector3 normal) {
        this.normal = normal.normalized;

        if (this.normal.magnitude == 0) throw new System.Exception("normal must not be null");
    }

    public override void HandleInput (Transform transform) {
        transform.forward = normal;

        var movementVector = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.up * Input.GetAxisRaw("Vertical");
        movementVector = velocity * Time.deltaTime * (movementVector.normalized + 3 * (Input.mouseScrollDelta.y * Vector3.forward));

        transform.position += transform.right * movementVector.x + transform.forward * movementVector.z + transform.up * movementVector.y;
        if (Vector3.Dot(transform.position, normal) > -10) transform.position = -10 * normal;
    }
}