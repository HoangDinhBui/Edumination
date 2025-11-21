using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class BandScaleDTO
    {
        public long Id { get; set; }
        public long PaperId { get; set; }
        public string Skill { get; set; }
        public int RawScoreMin { get; set; }
        public int RawScoreMax { get; set; }
        public decimal BandScore { get; set; }

        public bool IsInRange(int rawScore)
        {
            return rawScore >= RawScoreMin && rawScore <= RawScoreMax;
        }

        public override string ToString()
        {
            return $"{Skill}: {RawScoreMin}-{RawScoreMax} → Band {BandScore}";
        }
    }
}
