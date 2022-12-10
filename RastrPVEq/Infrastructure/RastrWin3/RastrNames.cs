namespace RastrPVEq.Infrastructure.RastrWin3
{
    /// <summary>
    /// RastrNames class
    /// </summary>
    public static class RastrNames
    {
        #region Table "Узлы" for Узлы
        public const string NodeTable = "node";

        public const string NodeStatusColumn = "sta";
        public const string NodeNumberColumn = "ny";
        public const string NodeNameColumn = "name";
        public const string NodeRatedVoltageColumn = "uhom";
        #endregion

        #region Table "Ветви" for Branch
        public const string BranchTable = "vetv";

        public const string BranchStatusColumn = "sta";
        public const string BranchTypeColumn = "tip";
        public const string BranchStartNodeColumn = "ip";
        public const string BranchEndNodeColumn = "iq";
        public const string BranchNameColumn = "name";
        public const string BranchResistanceColumn = "r";
        public const string BranchInductanceColumn = "x";
        public const string BranchTranformationRatioColumn = "ktr";
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
