using System;
using System.IO;

namespace PropertyGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var archivoEntrada = @"C:\GIT\PropertyGen\campos.txt";
            var archivoSalida = @"C:\GIT\PropertyGen\salida.txt";

            File.Delete(archivoSalida);
            using (var sr = new StreamReader(archivoEntrada))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    var lineaPropiedad = linea.Split(";");
                    var nombre = lineaPropiedad[0];
                    var tipo = lineaPropiedad[1];
                    var longitud = lineaPropiedad[2] ?? "";
                    var nombrePrivado = $"_{char.ToLower(nombre[0]) + nombre.Substring(1)}";
                    var propf = GenerarPropFull(nombre, nombrePrivado, tipo, longitud);

                    Console.WriteLine(propf);
                    File.AppendAllText(archivoSalida, propf);
                }
            }
        }

        private static string GenerarPropFull(string propName, string propPrivateName, string propType, string propLongitud)
        {
            var propf = $"private {propType} {propPrivateName};\n\n";

            propf += $"public {propType} {propName}\n{{\n";
            propf += $"\tget {{ return {propPrivateName}; }}\n";
            propf += $"\tset\n\t{{\n\t";
            propf += $"\tvar longitud = {propLongitud};\n\n";
            propf += $"\t\t{propPrivateName} = value\n";
            propf += $"\t\t\t.PadRight(longitud)\n";
            propf += $"\t\t\t.Substring(0, longitud);\n";
            propf += $"\t}}\n}}\n\n";

            return propf;
        }
    }
}
