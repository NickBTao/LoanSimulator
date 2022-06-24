//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace LoanSimulatorV2
{
    class Loan
    {
        public double Principal { get; set; }
        public int Years { get; set; }

        public Loan()
        {

        }
        public Loan(double principal, int years)
        {
            Principal = principal;
            Years = years;
        }

        public override string ToString()
        {
            return "Principal: $" + Principal + " - Years: " + Years;
        }

        public override bool Equals(object obj)
        {
            return obj is Loan loan &&
                   Principal == loan.Principal &&
                   Years == loan.Years;
        }

        public override int GetHashCode()
        {
            int hashCode = -1659439730;
            hashCode = hashCode * -1521134295 + Principal.GetHashCode();
            hashCode = hashCode * -1521134295 + Years.GetHashCode();
            return hashCode;
        }
    }
}
