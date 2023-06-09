﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

namespace Peace
{
    public class RealTimeWorld : MonoBehaviour
    {
        public String configLocation = "";
        public GameObject tracking;

        private Collector _collector;
        private World _world;
        private Vector3 _position;
        private FirstPersonView _view;

        private bool _collecting;

        private RealTimeWorldStatistics _stats = new RealTimeWorldStatistics();

        private Queue<GameObject> _objectPool;
        private List<GameObject> _objectsUsed;

        private async void RunCollect()
        {
            _collecting = true;
            await _collector.CollectFirstPerson(_world, _view);
            UpdateFromCollector();
            _collecting = false;
        }

        private void UpdateFromCollector()
        {
            _stats.removed = 0;
            _stats.added = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = _objectsUsed.Count - 1; i >= 0; --i)
            {
                GameObject used = _objectsUsed[i];

                if (!_collector.HasNode(used.name))
                {
                    _objectsUsed.RemoveAt(i);
                    used.SetActive(false);
                    _objectPool.Enqueue(used);
                }
            }

            foreach (var nodeKey in _collector.GetNewNodes())
            {
                var node = _collector.GetNode(nodeKey);
                Mesh mesh = _collector.GetMesh(node.Mesh);

                if (mesh != null)
                {
                    GameObject child = AllocateObject(nodeKey);
                    child.transform.localPosition = new Vector3((float)node.posX, (float)node.posZ, (float)node.posY);
                    child.transform.localScale = new Vector3((float)node.scaleX, (float)node.scaleZ, (float)node.scaleY);
                    child.transform.localEulerAngles = new Vector3((float)node.rotX, (float)node.rotZ, (float)node.rotY);

                    MeshFilter meshFilter = child.GetComponent<MeshFilter>();
                    meshFilter.sharedMesh = mesh;

                    MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();

                    Material material = _collector.GetMaterial(node.Material);

                    if (material != null)
                    {
                        meshRenderer.material = material;
                    }
                    else
                    {
                        meshRenderer.material.shader = Shader.Find("Standard");
                    }

                    _objectsUsed.Add(child);
                }

                _stats.added++;
            }

            sw.Stop();
            _stats.updateTime = (float)sw.Elapsed.TotalMilliseconds;
            _stats.collectorStats = _collector.LastStats;
        }

        private GameObject AllocateObject(string name)
        {
            GameObject obj;

            if (_objectPool.Count != 0)
            {
                obj = _objectPool.Dequeue();
                obj.SetActive(true);
                obj.name = name;
            }
            else
            {
                obj = new GameObject(name);
                obj.transform.SetParent(transform);
                obj.AddComponent<MeshFilter>();
                obj.AddComponent<MeshRenderer>();
            }
            return obj;
        }

        // Start is called before the first frame update
        void Start()
        {
            _objectPool = new Queue<GameObject>();
            _objectsUsed = new List<GameObject>();

            if (configLocation == "")
            {
                _world = World.CreateDemo("");
            }
            else
            {
                _world = new World(configLocation);
            }

            _collector = new Collector();

            _view.eyeResolution = 700;
            _view.maxDistance = 10000;
        }

        void UpdatePosition(Vector3 position)
        {
            _position = position;
            _view.X = position.x;
            _view.Y = position.z;
            _view.Z = position.y;
        }

        void Update()
        {
            Vector3 newPos = tracking.transform.position;

            if (Vector3.Distance(newPos, _position) > 10 && !_collecting)
            {
                UpdatePosition(newPos);
                RunCollect();
            }
        }
    }

}
