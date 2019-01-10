using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerVervoer
{
    public class SortingService
    {
        /// <summary>
        /// Return a sorted list of containers by placementheight
        /// </summary>
        public List<Container> SortContainers(List<Container> containers)
        {
            return containers.OrderBy(container => container.PlacementHeight).ToList();
        }
    }
}
