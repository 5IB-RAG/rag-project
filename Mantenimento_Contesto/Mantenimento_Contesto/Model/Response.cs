using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimento_Contesto.Model
{
    public class Response
    {
        public string Object { get; set; }
        public List<EmbeddingData> Data { get; set; }
        public string Model { get; set; }
        public EmbeddingUsage Usage { get; set; }
    }
}
