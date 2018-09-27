using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessBackground
{
    public class DTOData
    {
        public bool IsLog { get; set; }
        public bool IsStop { get; set; }
        public List<DTODataDetail> ListDetails { get; set; }
    }
}
