using UnityEngine;

namespace Materials
{
    // Define the attribute to specify custom row and column labels
    public class MatrixLabelsAttribute : PropertyAttribute
    {
        public readonly string[] Rows;
        public MatrixLabelsAttribute(params string[] rows) => Rows = rows;
    }
}
