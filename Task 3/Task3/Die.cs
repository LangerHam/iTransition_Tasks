using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    public sealed record Die(int[] Faces)
    {
        public string Name => string.Join(",", Faces);
        public int Roll(int index) => Faces[index];
        public override string ToString() => $"[{Name}]";
    }
}
