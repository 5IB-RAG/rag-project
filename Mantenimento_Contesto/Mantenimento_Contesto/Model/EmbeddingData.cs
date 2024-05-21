using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimento_Contesto.Model
{
    public class EmbeddingData
    {
        public string Object { get; set; }
        public int Index { get; set; }
        public List<float> Embedding { get; set; }
    }
}