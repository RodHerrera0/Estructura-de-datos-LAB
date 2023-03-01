using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;

namespace ConsoleApp1
{
    class Program
    {


        public class InputLab
        {
            public Dictionary<string, bool>[] input1 { get; set; }
            public string[] input2 { get; set; }
        }

        public class Apartamento
        {
            public int numero { get; set; }
            public string amenidadesNombre { get; set; }
            public bool amenidades { get; set; }
        }

        public static void Main()
        {


            List<int> requisitos = new List<int>();
            Dictionary<string, int> solicitud = new Dictionary<string, int>();
            Dictionary<string, int> places = new Dictionary<string, int>();
            Dictionary<string, int> apartamentos = new Dictionary<string, int>();
            Dictionary<string, bool> apartamentosTemp = new Dictionary<string, bool>();
            Dictionary<int, int> distancia = new Dictionary<int, int>();
            Dictionary<int, int> distanciaMax = new Dictionary<int, int>();
            Dictionary<int, int> valoresFinales = new Dictionary<int, int>();
            Dictionary<int, int> valoresMinimosFinales = new Dictionary<int, int>();
            Dictionary<int, int> valoresMinimosFinalesMin = new Dictionary<int, int>();
            Dictionary<int, int> distanciaMin = new Dictionary<int, int>();

            string path = @"C:\Users\herre\OneDrive\Escritorio\Primer Ciclo 2023\EStructuras\Lab\Lab 1 (corregido)\output.jsonl";

            int contador = 0;
            string text = File.ReadAllText(@"C:\Users\herre\OneDrive\Escritorio\Primer Ciclo 2023\EStructuras\Lab\Lab 1 (corregido)\input_challenge.jsonl");
            string[] words = text.Split('\n');
            for (int i = 0; i < words.Length - 1; i++)
            {
                InputLab input = JsonSerializer.Deserialize<InputLab>(words[i])!;
                int apartment = 0;
                foreach (Dictionary<string, bool> map in input.input1)
                {
                    foreach (KeyValuePair<string, bool> entry in map)
                    {
                        if (!places.ContainsKey(entry.Key))
                        {
                            places.Add(entry.Key, contador);
                            contador++;
                        }
                    }
                    apartamentos.Add($"apartment: {apartment}", apartment);
                    apartment++;
                }

                foreach (var item in apartamentos)
                {
                }
                foreach (var item in places)
                {
                    Console.WriteLine(item.Key + " " + item.Value);
                }

                bool[,] matriz = new bool[apartamentos.Count, places.Count];

                for (int x = 0; x < apartamentos.Count; x++)
                {
                    for (int j = 0; j < places.Count; j++)
                    {
                        matriz[x, j] = false;
                    }
                }
                int contador1 = 0;

                foreach (Dictionary<string, bool> map in input.input1)
                {

                    foreach (KeyValuePair<string, bool> entry in map)
                    {

                        string key = entry.Key;
                        int value = places[key];
                        if (entry.Value == true)
                        {
                            matriz[contador1, value] = true;
                        }
                    }
                    contador1++;

                }

                for (int f = 0; f < apartamentos.Count; f++)
                {
                    for (int c = 0; c < places.Count; c++)
                    {
                        Console.Write(string.Format("{0}\t", matriz[f, c]));
                    }

                    Console.WriteLine("");
                }




                foreach (string requirement in input.input2)
                {

                    Console.WriteLine($"requirement: {requirement}");
                }

                foreach (var item1 in places)
                {
                    foreach (var item2 in input.input2)
                    {
                        if (item1.Key == item2)
                        {
                            solicitud.Add(item2, item1.Value);
                        }

                    }
                }


                foreach (var item in solicitud)
                {

                    Console.WriteLine($"requisitos a evaluar: {item.Key} , {item.Value}");



                }

                int countApartments = apartamentos.Count;
                int countRequirements = solicitud.Count;
                int[,] distances = new int[countApartments, countRequirements];

                Console.WriteLine(matriz.ToString());

                if (validarSolicitudes(solicitud, input.input2, apartamentos, matriz))
                {
                    Console.WriteLine("si se puede");
                    for (int a = 0; a < solicitud.Count; a++)
                    {
                        for (int r = 0; r < apartamentos.Count; r++)
                        {
                            if (WalkToDown(solicitud, apartamentos, matriz, r, a) == WalkToUp(solicitud, apartamentos, matriz, r, a))
                            {

                                distances[r, a] = WalkToDown(solicitud, apartamentos, matriz, r, a);
                            }
                            else if (WalkToDown(solicitud, apartamentos, matriz, r, a) < WalkToUp(solicitud, apartamentos, matriz, r, a))
                            {

                                distances[r, a] = WalkToDown(solicitud, apartamentos, matriz, r, a);


                            }
                            else if (WalkToDown(solicitud, apartamentos, matriz, r, a) > WalkToUp(solicitud, apartamentos, matriz, r, a))
                            {
                                distances[r, a] = WalkToUp(solicitud, apartamentos, matriz, r, a);
                            }

                        }
                    }

                    Console.WriteLine("matriz para max");
                    for (int f = 0; f < apartamentos.Count; f++)
                    {
                        for (int c = 0; c < solicitud.Count; c++)
                        {
                            Console.Write(string.Format("{0}\t", distances[f, c]));
                        }

                        Console.WriteLine("");
                    }



                    int sumatoria = 0;
                    int maxValue = 0;
                    int minValue = 100;
                    if (solicitud.Count.Equals(0))
                    {
                        sumatoria = -1;
                    }
                    else
                    {
                        sumatoria = 0;
                    }

                    for (int f = 0; f < apartamentos.Count; f++)
                    {
                        for (int c = 0; c < solicitud.Count; c++)
                        {

                            Console.Write(string.Format("{0}\t", distances[f, c]));
                            sumatoria = sumatoria + distances[f, c];

                            if (maxValue < distances[f, c])
                            {
                                maxValue = distances[f, c];
                            }

                            if (distances[f, c] < minValue)
                            {
                                minValue = distances[f, c];
                            }

                        }
                        Console.Write(" ");
                        Console.Write(sumatoria);
                        Console.Write(" ");
                        Console.Write("valor maximo: " + maxValue);
                        Console.Write(" ");
                        Console.Write("valor minimo: " + minValue);


                        distancia.Add(f, sumatoria);

                        distanciaMax.Add(f, maxValue);
                        distanciaMin.Add(f, minValue);
                        Console.WriteLine("");
                        sumatoria = 0;
                        maxValue = 0;
                        minValue = 100;
                    }



                    var s = 0;
                    var min = Int32.MaxValue;

                    var s2 = 0;
                    var min2 = Int32.MaxValue;

                    var s3 = 0;
                    var min3 = Int32.MaxValue;


                    foreach (var item in distancia)
                    {
                        if (item.Value < min)
                        {
                            s = item.Key;
                            min = item.Value;
                        }


                    }

                    Console.WriteLine(min.ToString());
                    StreamWriter sw = File.AppendText(path);

                    if (min >= 0 && min < 200)
                    {
                        var matchingKeys = distancia.Where(kvp => kvp.Value == min)
                                .Select(kvp => kvp.Key);

                        foreach (var item in matchingKeys)
                        {

                            Console.WriteLine("matching de apt: " + item);

                            for (int l = 0; l < distanciaMax.Count; l++)
                            {
                                if (item == distanciaMax.ElementAt(l).Key)
                                {
                                    Console.WriteLine("valor distancia maxima por match: " + distanciaMax.ElementAt(l).Value);


                                    valoresFinales.Add(item, distanciaMax.ElementAt(l).Value);

                                    foreach (var item2 in valoresFinales)
                                    {
                                        if (item2.Value < min2)
                                        {
                                            s2 = item2.Key;
                                            min2 = item2.Value;
                                        }
                                    }
                                }
                            }
                        }

                        var matchingKeys2 = valoresFinales.Where(kvp => kvp.Value == min2)
                                   .Select(kvp => kvp.Key);



                        foreach (var item in matchingKeys2)
                        {
                            valoresMinimosFinalesMin.Add(item, distanciaMin[item]);
                        }


                        foreach (var item in valoresMinimosFinalesMin)
                        {
                            Console.WriteLine("aca son los finales ya con minimos: " + item);
                            if (item.Value <= min3)
                            {
                                s3 = item.Key;
                                min3 = item.Value;
                            }
                        }

                        var matchingKeys3 = valoresMinimosFinalesMin.Where(kvp => kvp.Value == min3)
                                   .Select(kvp => kvp.Key);



                        Console.WriteLine("por favor que funcione" + " " + "[" + String.Join(", ", matchingKeys3) + "]");
                        sw.WriteLine("[" + String.Join(", ", matchingKeys3) + "]");
                        sw.Close();
                        valoresFinales.Clear();
                        foreach (var item in valoresFinales)
                        {
                            Console.WriteLine("valores finales: " + item.Key + item.Value);
                        }



                        Console.WriteLine("[" + String.Join(", ", matchingKeys) + "]");
                    }
                    else
                    {
                        sw.WriteLine("[]");
                        sw.Close();
                        Console.WriteLine("[]");
                    }

                }
                else
                {
                    StreamWriter sw = File.AppendText(path);
                    sw.WriteLine("[]");
                    sw.Close();
                    Console.WriteLine("[]");
                    Console.WriteLine("try better next time");
                }
                solicitud.Clear();
                apartamentos.Clear();
                distancia.Clear();
                distanciaMax.Clear();
                distanciaMin.Clear();
                valoresMinimosFinalesMin.Clear();
            }
        }
        public static bool validarSolicitudes(Dictionary<string, int> solicitudValidate, string[] placesValidate, Dictionary<string, int> apartamentosValidate, bool[,] matrizValidate)
        {
            if (solicitudValidate.Count > 0 && solicitudValidate.Count == placesValidate.Length)
            {
                foreach (var item in solicitudValidate)
                {
                    for (int g = 0; g < apartamentosValidate.Count; g++)
                    {
                        if (matrizValidate[g, item.Value] == false && g == apartamentosValidate.Count - 1)
                        {
                            return false;
                        }
                        else if (matrizValidate[g, item.Value] == true)
                        {
                            break;
                        }


                    }

                }
                return true;

            }
            else
            {
                return false;
            }

        }

        public static int WalkToDown(Dictionary<string, int> solicitud, Dictionary<string, int> apartamentos, bool[,] matriz, int contador, int contador2)
        {

            int distanciaRecorrida = 0;


            for (int i = contador2; i < solicitud.Count; i++)
            {
                for (int f = contador; f < apartamentos.Count; f++)
                {
                    if (matriz[f, solicitud.ElementAt(i).Value].Equals(true))
                    {
                        return distanciaRecorrida;
                    }
                    else if (matriz[f, solicitud.ElementAt(i).Value].Equals(false) && f == apartamentos.Count - 1)
                    {
                        return 1000;
                    }
                    else
                    {

                        distanciaRecorrida++;

                    }
                }
            }

            return 1000;
        }



        public static int WalkToUp(Dictionary<string, int> solicitud, Dictionary<string, int> apartamentos, bool[,] matriz, int contador, int contador2)
        {


            int distanciaRecorrida = 0;

            for (int i = contador2; i < solicitud.Count; i++)
            {

                for (int f = contador; f >= 0; f--)
                {

                    if (matriz[f, solicitud.ElementAt(i).Value].Equals(true))
                    {

                        return distanciaRecorrida;
                    }
                    else if (matriz[f, solicitud.ElementAt(i).Value].Equals(false) && f == 0)
                    {
                        return 1000;
                    }
                    else
                    {
                        distanciaRecorrida++;


                    }
                }

            }
            return 1000;
        }




    }
}
