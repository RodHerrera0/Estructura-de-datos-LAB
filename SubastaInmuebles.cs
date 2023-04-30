using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SubastaInmuebles
{
    class Cliente
    {
        public string dpi { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime birthDate { get; set; }
        public string job { get; set; }
        public string placeJob { get; set; }
        public int salary { get; set; }
        public string signature { get; set; }

        public Cliente(string dpi, string firstName, string lastName, DateTime birthDate, string job, string placeJob, int salary)
        {
            this.dpi = dpi;
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
            this.job = job;
            this.placeJob = placeJob;
            this.salary = salary;
            this.signature = GenerarFirma();
        }

        private string GenerarFirma()
        {
            string firma = $"{dpi}-{firstName}-{lastName}-{birthDate.ToString("yyyyMMddTHHmmss.fff")}-{job}-{placeJob}-{salary}";
            // Aquí se puede utilizar cualquier función de hash para generar la firma
            // En este caso, se utiliza SHA256 para simplificar
            byte[] bytesFirma = System.Text.Encoding.UTF8.GetBytes(firma);
            System.Security.Cryptography.SHA256Managed sha256 = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256.ComputeHash(bytesFirma);
            return Convert.ToBase64String(hash);
        }
    }
    class Subasta
    {
        public string propiedad { get; set; }
        public List<Cliente> clientes { get; set; }
        public int rechazos { get; set; }

        public Subasta(string propiedad, List<Cliente> clientes, int rechazos)
        {
            this.propiedad = propiedad;
            this.clientes = clientes;
            this.rechazos = rechazos;
        }


    }
         class Program
    {
        static void Main(string[] args)
        {
            // Lectura del archivo de clientes
            List<Cliente> clientes = new List<Cliente>();
            using (StreamReader archivoClientes = new StreamReader("clientes.json"))
            {
                string linea;
                while ((linea = archivoClientes.ReadLine()) != null)
                {
                    Cliente cliente = JsonConvert.DeserializeObject<Cliente>(linea);
                    clientes.Add(cliente);
                }
            }

            // Creación del árbol AVL de clientes
            AVLTree<string, Cliente> arbolClientes = new AVLTree<string, Cliente>();
            foreach (Cliente cliente in clientes)
            {
                arbolClientes.Insert(cliente.dpi, cliente);
            }

            // Lectura del archivo de subastas
            List<Subasta> subastas = new List<Subasta>();
            using (StreamReader archivoSubastas = new StreamReader("subastas.json"))
            {
                string linea;
                while ((linea = archivoSubastas.ReadLine()) != null)
                {
                    Subasta subasta = JsonConvert.DeserializeObject<Subasta>(linea);
                    subastas.Add(subasta);
                }
            }
            // Procesamiento de las subastas
            foreach (Subasta subasta in subastas)
            {
                Console.WriteLine($"Subasta de la propiedad {subasta.propiedad}:");
                bool aceptada = false;

                foreach (Cliente cliente in subasta.clientes)
                {
                    Console.WriteLine($"Cliente: {cliente.firstName} {cliente.lastName}");

                    // Verificación de la firma del cliente
                    byte[] bytesFirma = System.Text.Encoding.UTF8.GetBytes($"{cliente.dpi}-{cliente.signature}");
                    System.Security.Cryptography.SHA256Managed sha256 = new System.Security.Cryptography.SHA256Managed();
                    byte[] hash = sha256.ComputeHash(bytesFirma);
                    string firmaCalculada = Convert.ToBase64String(hash);
                    if (firmaCalculada != cliente.signature)
                    {
                        Console.WriteLine("La firma del cliente es inválida");
                        continue;
                    }

                    // Verificación del árbol AVL
                    Cliente clienteAVL = arbolClientes.Search(cliente.dpi);
                    if (clienteAVL == null)
                    {
                        Console.WriteLine("El cliente no está registrado en el sistema");
                        continue;
                    }

                    // Verificación de salario mínimo
                    if (cliente.salary < subasta.propiedad.Length * 10000)
                    {
                        Console.WriteLine("El salario del cliente es insuficiente para participar en la subasta");
                        continue;
                    }

                    Console.WriteLine("El cliente es elegible para participar en la subasta");
                    aceptada = true;
                    break;
                }

                if (aceptada)
                {
                    Console.WriteLine("La subasta ha sido aceptada");
                }
                else
                {
                    subasta.rechazos++;
                    Console.WriteLine($"La subasta ha sido rechazada, número de rechazos: {subasta.rechazos}");
                }

                Console.WriteLine();
            }
        }
    }
    // Escritura del archivo de subastas
    using (StreamReader archivoSubastas = new StreamReader(" input_auctions_example_lab_3.json"))
    {
        foreach (Subasta subasta in subastas)
        {
            string linea = JsonConvert.SerializeObject(subasta);
            archivoSubastas.WriteLine(linea);
        }
    }
    // Escritura del archivo de clientes
    using (StreamReader archivoClientes = new StreamReader(" input_customer_example_lab_3.json"))
    {
        foreach (Cliente cliente in arbolClientes.Values)
        {
            string linea = JsonConvert.SerializeObject(cliente);
            archivoClientes.WriteLine(linea);
        }
    }


}
