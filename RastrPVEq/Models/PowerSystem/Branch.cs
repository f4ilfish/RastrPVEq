namespace RastrPVEq.Models.PowerSystem
{
    /// <summary>
    /// Branch class
    /// </summary>
    public class Branch
    {
        /// <summary>
        /// Index
        /// </summary>
        private int _index;

        /// <summary>
        /// Gets index
        /// </summary>
        public int Index
        {
            get => _index;
            private set
            {
                ValueValidation.IsZeroOrPositive(value);
                _index = value;
            }
        }

        /// <summary>
        /// Gets branch type
        /// </summary>
        public BranchType BranchType { get; private set; }

        /// <summary>
        /// Gets or sets branch start node
        /// </summary>
        public Node? BranchStartNode { get; set; }

        /// <summary>
        /// Gets or sets branch end node
        /// </summary>
        public Node? BranchEndNode { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        private string _name;

        /// <summary>
        /// Gets name
        /// </summary>
        public string Name
        {
            get => _name;
            private set
            {
                ValueValidation.IsNotNullOrEmptyString(value);
                _name = value;
            }
        }

        /// <summary>
        /// Resistance
        /// </summary>
        private double _resistance;

        /// <summary>
        /// Gets resistance
        /// </summary>
        public double Resistance
        {
            get => _resistance;
            private set
            {
                ValueValidation.IsNotNaN(value);
                ValueValidation.IsZeroOrPositive(value);
                _resistance = value;
            }
        }

        /// <summary>
        /// Inductance
        /// </summary>
        private double _inductance;

        /// <summary>
        /// Gets inductance
        /// </summary>
        public double Inductance
        {
            get => _inductance;
            private set
            {
                ValueValidation.IsNotNaN(value);
                _inductance = value;
            }
        }

        /// <summary>
        /// Capacitance
        /// </summary>
        private double _capacitance;

        /// <summary>
        /// Gets capacitance
        /// </summary>
        public double Capacitance
        {
            get => _capacitance;
            private set
            {
                ValueValidation.IsNotNaN(value);
                _capacitance = value;
            }
        }

        /// <summary>
        /// Transformation ratio
        /// </summary>
        private double _transformationRatio;

        /// <summary>
        /// Gets transformation ratio
        /// </summary>
        public double TransformationRatio
        {
            get => _transformationRatio;
            private set
            {
                ValueValidation.IsNotNaN(value);
                ValueValidation.IsZeroOrPositive(value);
                ValueValidation.IsOneOrLess(value);
                _transformationRatio = value;
            }
        }

        /// <summary>
        /// District number
        /// </summary>
        private int _districtNumber;
        
        /// <summary>
        /// Gets district number
        /// </summary>
        public int DistrictNumber
        {
            get => _districtNumber;
            private set
            {
                ValueValidation.IsZeroOrPositive(value);
                _districtNumber = value;
            }
        }

        /// <summary>
        /// Territory number
        /// </summary>
        private int _territoryNumber;

        /// <summary>
        /// Gets territory number
        /// </summary>
        public int TerritoryNumber
        {
            get => _territoryNumber;
            private set
            {
                ValueValidation.IsZeroOrPositive(value);
                _territoryNumber = value;
            }
        }

        /// <summary>
        /// Admissible  current
        /// </summary>
        private double _admissibleCurrent;

        /// <summary>
        /// Gets admissible current
        /// </summary>
        public double AdmissibleCurrent
        {
            get => _admissibleCurrent;
            private set
            {
                ValueValidation.IsNotNaN(value);
                _admissibleCurrent = value;
            }
        }

        /// <summary>
        /// Equipment admissilbe  current
        /// </summary>
        private double _equipmentAdmissibleCurrent;

        /// <summary>
        /// Gets equipment admissalbe  current
        /// </summary>
        public double EquipmentAdmissibleCurrent
        {
            get => _equipmentAdmissibleCurrent;
            private set
            {
                ValueValidation.IsNotNaN(value);
                _equipmentAdmissibleCurrent = value;
            }
        }

        /// <summary>
        /// Branch class instance constructor (for downloading)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="branchType"></param>
        /// <param name="name"></param>
        /// <param name="resistance"></param>
        /// <param name="inductance"></param>
        /// <param name="capacitance"></param>
        /// <param name="transformationRatio"></param>
        /// <param name="districtNumber"></param>
        /// <param name="territoryNumber"></param>
        /// <param name="admissibleCurrent"></param>
        /// <param name="equipmentAdmissibleCurrent"></param>
        public Branch(int index, 
                      BranchType branchType, 
                      string name, 
                      double resistance, 
                      double inductance, 
                      double capacitance, 
                      double transformationRatio, 
                      int districtNumber, 
                      int territoryNumber, 
                      double admissibleCurrent, 
                      double equipmentAdmissibleCurrent)
        {
            Index = index;
            BranchType = branchType;
            Name = name;
            Resistance = resistance;
            Inductance = inductance;
            Capacitance = capacitance;
            TransformationRatio = transformationRatio;
            DistrictNumber = districtNumber;
            TerritoryNumber = territoryNumber;
            AdmissibleCurrent = admissibleCurrent;
            EquipmentAdmissibleCurrent = equipmentAdmissibleCurrent;
        }

        /// <summary>
        /// Branch class instance constructor (for equivalent)
        /// </summary>
        /// <param name="branchType"></param>
        /// <param name="name"></param>
        /// <param name="resistance"></param>
        /// <param name="inductance"></param>
        /// <param name="capacitance"></param>
        /// <param name="transformationRatio"></param>
        /// <param name="districtNumber"></param>
        /// <param name="territoryNumber"></param>
        /// <param name="admissibleCurrent"></param>
        /// <param name="equipmentAdmissibleCurrent"></param>
        public Branch(BranchType branchType,
                      string name,
                      double resistance,
                      double inductance,
                      double capacitance,
                      double transformationRatio,
                      int districtNumber,
                      int territoryNumber,
                      double admissibleCurrent,
                      double equipmentAdmissibleCurrent)
        {
            BranchType = branchType;
            Name = name;
            Resistance = resistance;
            Inductance = inductance;
            Capacitance = capacitance;
            TransformationRatio = transformationRatio;
            DistrictNumber = districtNumber;
            TerritoryNumber = territoryNumber;
            AdmissibleCurrent = admissibleCurrent;
            EquipmentAdmissibleCurrent = equipmentAdmissibleCurrent;
        }
    }
}
