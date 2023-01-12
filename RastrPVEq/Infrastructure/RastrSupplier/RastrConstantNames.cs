namespace RastrPVEq.Infrastructure.RastrSupplier
{
    /// <summary>
    /// RastrConstantNames class
    /// </summary>
    public static class RastrConstantNames
    {
        #region Table Nodes
        public const string NodeTable = "node";
        public const string NodeNumberColumn = "ny";
        public const string NodeNameColumn = "name";
        public const string NodeRatedVoltageColumn = "uhom";
        public const string NodeDistrictNumberColumn = "na";
        public const string NodeTerritoryNumberColumn = "npa";
        #endregion

        #region Table Branches
        public const string BranchTable = "vetv";
        public const string BranchTypeColumn = "tip";
        public const string BranchStartNodeColumn = "ip";
        public const string BranchEndNodeColumn = "iq";
        public const string BranchNameColumn = "name";
        public const string BranchResistanceColumn = "r";
        public const string BranchInductanceColumn = "x";
        public const string BranchCapacitanceColumn = "b";
        public const string BranchTransformationRatioColumn = "ktr";
        public const string BranchDistrictNumberColumn = "na";
        public const string BranchTerritoryNumberColumn = "npa";
        public const string BranchAdmissibleCurrentColumn = "i_dop";
        public const string BranchEquipmentAdmissibleCurrentColumn = "i_dop_ob";
        #endregion

        #region Table Generators
        public const string GeneratorTable = "Generator";
        public const string GeneratorNumberColumn = "Num";
        public const string GeneratorNameColumn = "Name";
        public const string GeneratorNodeNumberColumn = "Node";
        public const string GeneratorMaxActivePowerColumn = "Pmax";
        public const string GeneratorPQDiagramNumberColumn = "NumPQ";
        #endregion
    }
}
