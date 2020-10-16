using System;
using System.IO;

namespace PropertyGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var archivoEntrada = @"C:\GIT\PropertyGen\PersonasFULL.txt";
            //var archivoEntrada = @"C:\GIT\PropertyGen\RelacionesFULL.txt";
            //var archivoEntrada = @"C:\GIT\PropertyGen\TransaccionesFULL.txt";
            var archivoSalida = @"C:\GIT\PropertyGen\salida.txt";

            File.Delete(archivoSalida);
            using (var sr = new StreamReader(archivoEntrada))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    var lineaPropiedad = linea.Split(";");
                    var nombre = lineaPropiedad[0];
                    var longitud = lineaPropiedad[1] ?? "";
                    var decimales = lineaPropiedad[2];
                    var tipo = lineaPropiedad[3];
                    var nombrePrivado = $"_{char.ToLower(nombre[0]) + nombre.Substring(1)}";
                    var propf = GenerarPropFull(nombre, nombrePrivado, tipo, longitud, decimales);

                    Console.WriteLine(propf);
                    File.AppendAllText(archivoSalida, propf);
                }
            }
        }

        private static string GenerarPropFull(string propName, string propPrivateName, string propType, string propLongitud, string propDecimal)
        {
            var propf = $"private string {propPrivateName};\n\n";

            propf += $"public string {propName}\n{{\n";
            propf += $"\tget {{ return {propPrivateName}; }}\n";
            propf += $"\tset\n\t{{\n\t";

            if (propType == "string")
            {
                propf += $"\tvar longitud = {propLongitud};\n\n";
                propf += $"\t\t{propPrivateName} = value\n";
                propf += $"\t\t\t.PadRight(longitud)\n";
                propf += $"\t\t\t.Substring(0, longitud);\n";
                propf += $"\t}}\n}}\n\n";
            }
            if (propType == "int")
            {
                var longitud = Convert.ToInt32(propLongitud);
                if (propDecimal != "0") longitud = Convert.ToInt32(propDecimal) + 1;
                propf += $"\tvar longitud = {longitud};\n\n";
                propf += $"\t\t{propPrivateName} = value\n";
                propf += $"\t\t\t.PadLeft(longitud, '0')\n";
                propf += $"\t\t\t.Substring(0, longitud);\n";
                propf += $"\t}}\n}}\n\n";
            }

            return propf;
        }
    }
}
