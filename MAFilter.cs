using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Tesis_Maestria
{
    class MAFilter
    {
        private int[] vector;

        private int filterLength;
        public int FilterLength
        {
            get { return this.filterLength; }
            set
            {
                this.filterLength = value;
                this.vector = new int[value];
            }
        }

        private int newDatum;
        public int NewDatum
        {
            set
            {
                this.newDatum = value;
                UpdateVector();
                VectorMean();
            }
        }

        private int filteredDatum;
        public int FilteredDatum
        {
            get
            {
                return this.filteredDatum;
            }
        }

        private void UpdateVector()
        {
            for (int i = this.filterLength - 1; i > 0; i--)
            {
                vector[i] = vector[i - 1];
            }
            vector[0] = this.newDatum;
        }

        private void VectorMean()
        {
            int sum = 0;
            for (int i = 0; i < this.filterLength; i++)
            {
                sum += this.vector[i];
            }
            this.filteredDatum = (int)(sum / this.filterLength);
        }
    }
}
