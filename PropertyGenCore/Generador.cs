using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyGenCore
{
    public class Generador
    {
        private readonly string _template;

        public Generador(string template)
        {
            _template = template;
        }

        public void GenerarArchivo(string rutaArchivoEntrada)
        {
            var archivoSalida = @"C:\GIT\PropertyGen\salida.txt";

            File.Delete(archivoSalida);
            using (var sr = new StreamReader(rutaArchivoEntrada))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    var lineaPropiedad = linea.Split(";");
                    var nombre = lineaPropiedad[0];
                    var longitud = lineaPropiedad[1] ?? "";
                    var decimales = lineaPropiedad[2];
                    var tipo = lineaPropiedad[3];
                    var propf = GenerarPropGetFull(nombre, tipo, longitud, decimales);

                    Console.WriteLine(propf);
                    File.AppendAllText(archivoSalida, propf);
                }
            }
        }

        public string GenerarPropGetFull(string propName, string propType, string propLongitud, string propDecimal)
        {
            var propf = _template.Replace("MyProperty", propName);
            propf = _template.Replace("myVar", "_" + char.ToLower(propName[0]) + propName.Substring(1));
            propf = _template.Replace("myType", propType);



            return propf;
        }

        public static string GenerarPropSetFull(string propName, string propPrivateName, string propType, string propLongitud, string propDecimal)
        {
            var propf = "";

            if (propType == "string")
            {
                propf = $"private string {propPrivateName} = \" \";\n\n";

                propf += $"public string {propName}\n{{\n";
                propf += $"\tget {{ return {propPrivateName}; }}\n";
                propf += $"\tset\n\t{{\n\t";

                propf += $"\tvar longitud = {propLongitud};\n\n";
                propf += $"\t\t{propPrivateName} = value\n";
                propf += $"\t\t\t.PadRight(longitud)\n";
                propf += $"\t\t\t.Substring(0, longitud)\n";
                propf += $"\t\t\t.Trim();\n";
                propf += $"\t}}\n}}\n\n";
            }
            if (propType == "int")
            {
                propf = $"private string {propPrivateName} = \"0\";\n\n";

                propf += $"public string {propName}\n{{\n";
                propf += $"\tget {{ return {propPrivateName}; }}\n";
                propf += $"\tset\n\t{{\n\t";

                var longitud = Convert.ToInt32(propLongitud);
                if (propDecimal != "0") longitud = Convert.ToInt32(propDecimal) + 1;
                propf += $"\tvar longitud = {longitud};\n\n";
                propf += $"\t\t{propPrivateName} = value\n";
                propf += $"\t\t\t.PadLeft(longitud, '0')\n";
                propf += $"\t\t\t.Substring(0, longitud)\n";
                propf += $"\t\t\t.Trim();\n";
                propf += $"\t}}\n}}\n\n";
            }

            return propf;
        }
    }
}
