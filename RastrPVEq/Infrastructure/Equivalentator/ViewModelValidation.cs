using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RastrPVEq.Models;
using RastrPVEq.Models.RastrWin3;
using RastrPVEq.Models.Topology;
using RastrPVEq.ViewModels;

namespace RastrPVEq.Infrastructure.Equivalentator
{
    public static class ViewModelValidation
    {
        /// <summary>
        /// Check is has equivalence branches duplicates
        /// </summary>
        /// <param name="equivalenceBranches"></param>
        /// <returns></returns>
        public static bool IsHasEquivalenceBranchesDuplicates(EquivalenceGroupViewModel equivalenceGroup)
        {
            var equivalenceBranches = equivalenceGroup.EquivalenceBranches;

            var totalBranches = equivalenceBranches.Count();
            var unqueBranches = equivalenceBranches.Distinct().Count();

            return totalBranches != unqueBranches;
        }

        /// <summary>
        /// Check one generators rated voltage level
        /// </summary>
        /// <returns></returns>
        public static bool IsOneGeneratorsRatedVoltageLevel(List<Generator> generatorsOfEquivalenceGroup)
        {
            var generatorsRatedVoltages = new List<double>();

            foreach (var generator in generatorsOfEquivalenceGroup)
            {
                var ratedVoltage = generator.GeneratorNode.RatedVoltage;

                generatorsRatedVoltages.Add(ratedVoltage);
            }

            var uniqueGeneratorsRatedVoltages = generatorsRatedVoltages.Distinct().Count();

            return uniqueGeneratorsRatedVoltages == 1;
        }
    }
}
