using BotAssets.Models;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application1.Caches
{
    public static class LocationCache
    {
        private static ConcurrentDictionary<Int16, Location> Locations = new ConcurrentDictionary<Int16, Location>();
        private static ConcurrentDictionary<Int16, Branch> Branches = new ConcurrentDictionary<Int16, Branch>();

        public static void InitCache()
        {
            if (!Locations.Any())
            {
                var locations = new List<Location>()
                { 
                    new Location()
                    {
                        LocationId = 1,
                        Description = "Breakroom",
                        Branch = new Branch()
                        {
                            BranchId = 1,
                            City = "Cincinnati",
                            State = "OH"
                        }
                    },
                    new Location()
                    {
                        LocationId = 2,
                        Description = "Training Room",
                        Branch = new Branch()
                        {
                            BranchId = 1,
                            City = "Cincinnati",
                            State = "OH"
                        }
                    },
                    new Location()
                    {
                        LocationId = 3,
                        Description = "Breakroom",
                        Branch = new Branch()
                        {
                            BranchId = 2,
                            City = "Columbus",
                            State = "OH"
                        }
                    },
                    new Location()
                    {
                        LocationId = 4,
                        Description = "Training Room",
                        Branch = new Branch()
                        {
                            BranchId = 2,
                            City = "Columbus",
                            State = "OH"
                        }
                    },
                    new Location()
                    {
                        LocationId = 5,
                        Description = "Breakroom",
                        Branch = new Branch()
                        {
                            BranchId = 3,
                            City = "Memphis",
                            State = "TN"
                        }
                    },
                    new Location()
                    {
                        LocationId = 6,
                        Description = "Training Room",
                        Branch = new Branch()
                        {
                            BranchId = 3,
                            City = "Memphis",
                            State = "TN"
                        }
                    }
                };


                foreach(var location in locations)
                {
                    Locations.TryAdd(location.LocationId, location);
                    Branches.TryAdd(location.Branch.BranchId, location.Branch);
                }
            }
        }

        public static Branch GetBranchById(Int16 branchId) =>
            Branches[branchId];

        public static Branch GetBranchByCityState(string city, string state) =>
            Branches.Values.FirstOrDefault(b => b.City.Equals(city, StringComparison.InvariantCultureIgnoreCase) &&
                                                b.State.Equals(state, StringComparison.InvariantCultureIgnoreCase));
        
        public static IEnumerable<Branch> GetAllBranches() =>
            Branches.Values;

        public static IEnumerable<Branch> GetBranchesByCity(string city) =>
            Branches.Values.Where(b => b.City.Equals(city, StringComparison.InvariantCultureIgnoreCase));

        public static IEnumerable<Branch> GetBranchesByState(string state) =>
            Branches.Values.Where(b => b.State.Equals(state, StringComparison.InvariantCultureIgnoreCase));

        public static IEnumerable<Branch> FindBranches(IEnumerable<string> input)
        {
            var stateBranches = input.SelectMany(i => GetBranchesByState(i)).Select(b => b.BranchId);
            var cityBranches = input.SelectMany(i => GetBranchesByCity(i)).Select(b => b.BranchId);

            var all = stateBranches.Union(cityBranches);
            var common = stateBranches.Intersect(cityBranches);

            return common.Count() == 1 
                ? new List<Branch>() { GetBranchById(common.ElementAt(0)) }
                : all.Select(i => GetBranchById(i));
        }
        
        public static IEnumerable<Location> GetAllLocations() =>
            Locations.Values;

        public static IEnumerable<Location> GetLocationsByBranch(Int16 branchId) =>
            Locations.Values.Where(location => location.Branch.BranchId == branchId);
    }
}