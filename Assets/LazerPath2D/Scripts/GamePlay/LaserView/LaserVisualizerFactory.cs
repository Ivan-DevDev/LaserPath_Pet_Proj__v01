using System.Collections.Generic;
using UnityEngine;

namespace Assets.LazerPath2D.Scripts.GamePlay.LaserView
{
    public class LaserVisualizerFactory
    {
        public List<LaserVisualizer> CreateLaserVisualizerList( int amountLaserEmiters)
        {
            List<LaserVisualizer> laserVisualizers = new ();

            for (int i = 0; i < amountLaserEmiters; i++)
            {
                LaserVisualizer laserVisualizerPrefab = Resources.Load<LaserVisualizer>("GamePlay/ObjectsInScene/LaserView/LaserVisualiser");

                LaserVisualizer laserVisualizer = UnityEngine.Object.Instantiate(laserVisualizerPrefab);
                laserVisualizer.Initialize();

                laserVisualizers.Add(laserVisualizer);
            }

            return laserVisualizers;
        }
    }
}
