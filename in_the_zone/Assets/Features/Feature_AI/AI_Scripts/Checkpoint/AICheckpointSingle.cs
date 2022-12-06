using UnityEngine;

    public class AICheckpointSingle : MonoBehaviour
    {
        private AITrackCheckpoints _trackCheckpoints;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out AICheckpointManager component))
            {
                _trackCheckpoints.PlayerThroughCheckpoint(this, other.transform.parent);
            }
        }

        public void SetTrackCheckpoints(AITrackCheckpoints trackCheckpoints)
        {
            this._trackCheckpoints = trackCheckpoints;
        }
    }
