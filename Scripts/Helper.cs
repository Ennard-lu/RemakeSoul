using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Helper {
    public static string PascalToSnakeCase(string pascalString) {
        StringBuilder snakeCaseString = new();
        for (int i = 0; i < pascalString.Length; i++) {
            char currentChar = pascalString[i];
            if (char.IsUpper(currentChar) && i > 0)
                snakeCaseString.Append('_');
            snakeCaseString.Append(char.ToLower(currentChar));
        }
        return snakeCaseString.ToString();
    }
}
