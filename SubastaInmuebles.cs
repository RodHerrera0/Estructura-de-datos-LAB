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
}
