namespace RastrPVEq.Infrastructure.RastrSupplier
{
    /// <summary>
    /// RastrConstantNames class
    /// </summary>
    public static class RastrConstantNames
    {
        #region Table "Узлы" for Узлы
        public const string NodeTable = "node";
        public const string NodeNumberColumn = "ny";
        public const string NodeNameColumn = "name";
        public const string NodeRatedVoltageColumn = "uhom";
        public const string NodeDistrictNumberColumn = "na";
        public const string NodeTerritoryNumberColumn = "npa";
        #endregion

        #region Table "Ветви" for Branch
        public const string BranchTable = "vetv";
        public const string BranchTypeColumn = "tip";
        public const string BranchStatusColumn = "sta";
        public const string BranchStartNodeColumn = "ip";
        public const string BranchEndNodeColumn = "iq";
        public const string BranchNameColumn = "name";
        public const string BranchResistanceColumn = "r";
        public const string BranchInductanceColumn = "x";
        public const string BranchCapacitanceColumn = "b";
        public const string BranchTransformationRatioColumn = "ktr";
        public const string BranchDistrictNumberColumn = "na";
        public const string BranchTerritoryNumberColumn = "npa";
        public const string BranchAdmissableCurrentColumn = "i_dop";
        public const string BranchEquipmentAdmissableCurrentColumn = "i_dop_ob";
        #endregion

        #region Table "PQ диаграммы" for AdjustmentRange
        public const string AdjustmentRangeTable = "graphik2";
        public const string AdjustmentRangeNumberColumn = "Num";
        public const string AdjustmentRangeActivePowerColumn = "P";
        public const string AdjustmentRangeMinimumReactivePowerColumn = "Qmin";
        public const string AdjustmentRangeMaximumReactivePowerColumn = "Qmax";
        #endregion

        #region Table "Генераторы УР" for Generator
        public const string GeneratorTable = "Generator";
        public const string GeneratorStatusColumn = "sta";
        public const string GeneratorNumberColumn = "Num";
        public const string GeneratorNameColumn = "Name";
        public const string GeneratorNodeNumberColumn = "Node";
        public const string GeneratorMaxActivePowerColumn = "Pmax";
        public const string GeneratorPQDiagramNumberColumn = "NumPQ";
        #endregion
    }
}
