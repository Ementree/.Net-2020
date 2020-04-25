using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._6.Models.ViewModels;

namespace DotNet2020.Domain._6.Services
{
    public class CapacityViewModelBuilder
    {
        private readonly List<Resource> _resources;
        private readonly List<ResourceCapacity> _resourceCapacities;

        public CapacityViewModelBuilder(List<Resource> resources, List<ResourceCapacity> resourceCapacities)
        {
            _resources = resources;
            _resourceCapacities = resourceCapacities;
        }

        public Dictionary<string, List<ViewModelCapacity>> Build()
        {
            var capacities = GroupByResource();
            var viewModelList = MakeViewModelList(capacities);
            var model = GroupCapacityByGroup(viewModelList);
            
            return model;
        }

        private List<ViewModelCapacity> MakeViewModelList(Dictionary<int, Dictionary<int, int>> capacities)
        {
            var viewModelList = new List<ViewModelCapacity>();
            foreach (var resource in _resources)
            {
                var capacity = new Dictionary<int, int>();
                if (capacities.ContainsKey(resource.Id))
                {
                    capacity = capacities[resource.Id];
                }

                viewModelList.Add(new ViewModelCapacity(
                    resource.Id,
                    resource.FirstName + ' ' + resource.LastName,
                    resource.ResourceGroupType.Type,
                    resource.ResourceGroupType.Group,
                    capacity));
            }

            return viewModelList;
        }

        private Dictionary<int, Dictionary<int, int>> GroupByResource()
        {
            var capacities = _resourceCapacities
                .GroupBy(res => res.Resource.Id)
                .Select(x => new
                {
                    resourceId = x.Key,
                    capacity = x.ToDictionary(y => y.Period.Start.Month, y => y.Capacity)
                })
                .ToDictionary(pair => pair.resourceId, pair => pair.capacity);
            
            return capacities;
        }

        private Dictionary<string, List<ViewModelCapacity>> GroupCapacityByGroup(List<ViewModelCapacity> viewModelList)
        {
            var model = new Dictionary<string, List<ViewModelCapacity>>();
            foreach (var viewModel in viewModelList)
            {
                if (model.ContainsKey(viewModel.Group))
                {
                    model[viewModel.Group].Add(viewModel);
                }
                else
                {
                    model.Add(viewModel.Group, new List<ViewModelCapacity>() {viewModel});
                }
            }

            return model;
        }
    }
}