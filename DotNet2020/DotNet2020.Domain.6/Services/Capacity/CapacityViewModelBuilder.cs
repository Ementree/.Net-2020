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
        private readonly int _year;
        private readonly bool _withAbsence;
        

        public CapacityViewModelBuilder(List<Resource> resources, List<ResourceCapacity> resourceCapacities, int year, bool withAbsence)
        {
            _resources = resources;
            _resourceCapacities = resourceCapacities;
            _year = year;
            _withAbsence = withAbsence;
        }

        public CapacityViewModel Build()
        {
            var capacities = GroupByResource();
            var viewModelList = MakeViewModelList(capacities);
            var data = GroupCapacityByGroup(viewModelList);
            var model = new CapacityViewModel(data, MonthGeneratorService.GetAllMonths(_year), _withAbsence);
            
            return model;
        }

        private List<CapacityViewModelData> MakeViewModelList(Dictionary<int, Dictionary<int, double>> capacities)
        {
            var viewModelList = new List<CapacityViewModelData>();
            foreach (var resource in _resources)
            {
                var capacity = new Dictionary<int, double>();
                if (capacities.ContainsKey(resource.Id))
                {
                    capacity = capacities[resource.Id];
                }

                viewModelList.Add(new CapacityViewModelData(
                    resource.Id,
                    resource.Employee.FirstName + ' ' + resource.Employee.LastName,
                    resource.ResourceGroupType.Type,
                    resource.ResourceGroupType.Group,
                    capacity));
            }

            return viewModelList;
        }

        private Dictionary<int, Dictionary<int, double>> GroupByResource()
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

        private Dictionary<string, List<CapacityViewModelData>> GroupCapacityByGroup(List<CapacityViewModelData> viewModelList)
        {
            var model = new Dictionary<string, List<CapacityViewModelData>>();
            foreach (var viewModel in viewModelList)
            {
                if (model.ContainsKey(viewModel.Group))
                {
                    model[viewModel.Group].Add(viewModel);
                }
                else
                {
                    model.Add(viewModel.Group, new List<CapacityViewModelData>() {viewModel});
                }
            }

            return model;
        }
    }
}