using Assets.LazerPath2D.Scripts.GamePlay.LaserView;
using Assets.LazerPath2D.Scripts.GamePlay.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.Node.DetectorNode
{
    [Serializable]
    public class NodesDetector
    {
        public event Action UpdatedDetoctor;

        [SerializeField] private List<NodesDetectorData> _nodesDetectorDates = new();

        private LayerMask _receivingNodeLayer;
        private float _maxRayLength = 10f;

        private IDetectorNode _rayDetectorNode;
        private IReflectionNodeCalculator _reflectionDirectionNodeCalculator;
        private HandlerDetectorDataForLaserViewPoints _handlerDetectorDataForLaserViewPoints;
        private List<LaserVisualizer> _laserVisualizeres;

        private IReadOnlyList<INode> _allNodesInScene;
        private List<EmiterRay> _emitersRay = new();
        private List<INode> _activeNodes;

        public NodesDetector(
            IReadOnlyList<INode> allNodesInScene,
            List<LaserVisualizer> laserVisualizeres,
            IReflectionNodeCalculator reflectionDirectionNodeCalculator,
            IDetectorNode rayTracerToDetectNode,
            HandlerDetectorDataForLaserViewPoints handlerDetectorDataForLaserViewPoints,
            LayerMask receivingNodeLayer,
            float maxRayLength)
        {
            _receivingNodeLayer = receivingNodeLayer;
            _maxRayLength = maxRayLength;

            _rayDetectorNode = rayTracerToDetectNode;
            _reflectionDirectionNodeCalculator = reflectionDirectionNodeCalculator;
            _handlerDetectorDataForLaserViewPoints = handlerDetectorDataForLaserViewPoints;

            _laserVisualizeres = laserVisualizeres;

            _allNodesInScene = allNodesInScene;

            foreach (INode node in _allNodesInScene)
            {
                if (node is EmiterRay emiterRay)
                    _emitersRay.Add(emiterRay);
            }

            // создаём "nodesDetectorData" по кол-ву равным кол-ву эммитеров на сцене
            // и добавляем в список "_nodesDetectorDates"

            if (_emitersRay.Count != _laserVisualizeres.Count)
                throw new ArgumentException($"Внимание, Кол-во излучателей {_emitersRay.Count}, отличается от кол-ва визуализаторов {_laserVisualizeres.Count}");

            for (int i = 0; i < _emitersRay.Count; i++)
            {
                LaserVisualizer laserVisualizer = _laserVisualizeres[i];
                EmiterRay emiterRay = _emitersRay[i];

                NodesDetectorData nodesDetectorData = new NodesDetectorData(emiterRay, laserVisualizer);
                _nodesDetectorDates.Add(nodesDetectorData);
            }
        }

        public IReadOnlyList<NodesDetectorData> NodesDetectorDates => _nodesDetectorDates;

        public void ToTryRemoveNodesInDetectingListAfterThis(INode node)
        {
            foreach (NodesDetectorData nodesDetectorData in _nodesDetectorDates)
                nodesDetectorData.TryRemoveElementsInDetectingNodesAfter(node);
        }

        public void ToClearDetectingNodesInData(EmiterRay emiterRay)
        {
            foreach (NodesDetectorData nodesDetectorData in _nodesDetectorDates)
            {
                if (nodesDetectorData.EmiterRay == emiterRay)
                    nodesDetectorData.ToClearDetectingNodesList();
            }
        }

        public void ToDecectNode()
        {
            //int dataIteration = 0;

            foreach (NodesDetectorData nodesDetectorData in _nodesDetectorDates)
            {
                nodesDetectorData.ToClearData();

                INode nodeDetector = null;

                bool isRereflection = false;

                for (int i = 0; i < nodesDetectorData.DetectingNodes.Count + 1; i++)
                {

                    Vector3 currentDirection = Vector3.zero;

                    // детектим первая итерация                   
                    if (nodeDetector == null)
                    {
                        nodeDetector = nodesDetectorData.EmiterRay;
                        currentDirection = nodeDetector.transform.up;
                    }
                    else
                    {
                        if (nodeDetector is IReflectableNode refleReflectableNode)
                        {
                            Vector3 receivedDirection = nodesDetectorData.ReceivedLaserDirection;
                            currentDirection = ToGetDirectionReflection(refleReflectableNode, receivedDirection);

                            nodesDetectorData.DirectionReflection = currentDirection;

                            // обработка переотражения
                            float scalarTwoVectors = Vector3.Dot(receivedDirection.normalized, currentDirection.normalized);

                            if (scalarTwoVectors <= -1)
                            {
                                isRereflection = true;
                                break;
                            }
                        }
                    }

                    INode detectingNode = _rayDetectorNode.ToDetectNodeByRayCast(nodeDetector.transform.position, nodeDetector.Collider, currentDirection, _maxRayLength, _receivingNodeLayer);

                    if (detectingNode == null)
                    {
                        break;
                    }

                    if (detectingNode is IDetectableNode)
                    {
                        if (detectingNode is StarNode starNode)
                        {
                            //если детектим звезду которая уже активированна 
                            if (nodesDetectorData.DetectingNodes.Contains(starNode))
                                break;


                            starNode.ToActive();

                            nodesDetectorData.DetectingNodes.Add(starNode);

                            detectingNode = _rayDetectorNode.ToDetectNodeByRayCast(starNode.transform.position, starNode.Collider, currentDirection, _maxRayLength, _receivingNodeLayer);

                            // если в конце пути лазера только звезда 
                            if (detectingNode == null)
                                break;
                        }

                        if (detectingNode is EmiterRay emiterRay)
                        {
                            break;
                        }

                        nodesDetectorData.DetectingNodes.Add(detectingNode);

                        if (detectingNode is ReceiverRay receiverRay)
                        {

                            break;
                        }

                        if (detectingNode is IReflectableNode reflectablerNode)
                        {
                            nodesDetectorData.ReceivedLaserDirection = (detectingNode.transform.position - nodeDetector.transform.position).normalized;

                            nodeDetector = detectingNode;
                        }
                    }
                }

                ToUpdateDrawLaserView(nodesDetectorData, default, isRereflection);
            }

            UpdateActiveNodesList();
            SetActiveOrDeactiveDetectingNodes();

            UpdatedDetoctor?.Invoke();
        }

        public void UpdateActiveNodesList()
        {
            _activeNodes = new();

            // добавили в лист все ноды которые попали под луч
            foreach (NodesDetectorData nodesDetectorData in _nodesDetectorDates)
            {
                foreach (INode node in nodesDetectorData.DetectingNodes)
                {
                    if (_activeNodes.Contains(node) == false)
                    {
                        _activeNodes.Add(node);
                    }
                }
            }
        }

        public void SetActiveOrDeactiveDetectingNodes()
        {
            foreach (INode node in _allNodesInScene)
            {
                if (_activeNodes.Contains(node))
                    node.ToActive();
                else
                    node.ToDeActive();
            }
        }

        public void ToUpdateDrawLaserView(NodesDetectorData nodesDetectorData, bool nodeIsRotating = false, bool isRereflection = false)
        {
            _handlerDetectorDataForLaserViewPoints.ToHandleData(nodesDetectorData, nodeIsRotating, isRereflection);
        }

        private Vector3 ToGetDirectionReflection(IReflectableNode node, Vector3 receivedDirection)
        {
            return _reflectionDirectionNodeCalculator.ToСalculateReflection(node, receivedDirection);
        }
    }

    [Serializable]
    public class NodesDetectorData
    {
        public Vector3 DirectionReflection;

        [field: SerializeField] public EmiterRay EmiterRay { get; private set; }
        [field: SerializeField] public LaserVisualizer LaserVisualizer { get; private set; }

        public List<INode> DetectingNodes = new();
        public Vector3 ReceivedLaserDirection;

        public List<Vector3> LaserViewPoints = new();// для отслеживания в инспекторе, потом удалить

        public NodesDetectorData(EmiterRay emiterRay, LaserVisualizer laserVisualizer)
        {
            EmiterRay = emiterRay;
            LaserVisualizer = laserVisualizer;
        }

        // потом создать методы для добавление и удаления элементов в "DetectingNode" и "LaserViewPoints" 
        public void ToClearData()
        {
            DetectingNodes.Clear();
            LaserViewPoints.Clear();
            ReceivedLaserDirection = Vector3.zero;
        }

        public void TryRemoveElementsInDetectingNodesAfter(INode node)
        {
            if (DetectingNodes.Contains(node))
            {
                int indexOfElement = DetectingNodes.IndexOf(node);// если не найдет индекс то вернёт -1

                if (indexOfElement != -1 && indexOfElement != DetectingNodes.Count - 1)
                {
                    int amountElements = DetectingNodes.Count - indexOfElement - 1;

                    DetectingNodes.RemoveRange(indexOfElement + 1, amountElements);
                }
            }
        }

        public void ToClearDetectingNodesList() => DetectingNodes.Clear();
    }
}





