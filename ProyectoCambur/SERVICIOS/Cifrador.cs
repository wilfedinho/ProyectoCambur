using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SERVICIOS
{
    // Provee las dos operaciones criptograficas que usa todo el sistema:
    // - EncriptarIrreversible: hash SHA-256 (contrasenas, digito verificador)
    // - EncriptarReversible / DesencriptarReversible: AES (informacion clinica sensible)
    //
    // La clave AES es unica para todo el sistema y se persiste en App_Data/cifrador.key.
    // IMPORTANTE: ese archivo es la unica forma de recuperar los datos clinicos encriptados.
    // Si se pierde, TODA la informacion cifrada en la base queda irrecuperable. Hay que
    // incluirlo en el backup del sistema (fuera del control de versiones).
    public class Cifrador
    {
        private static Cifrador Instancia;
        public static Cifrador GestorCifrador
        {
            get
            {
                if (Instancia == null)
                {
                    Instancia = new Cifrador();
                }
                return Instancia;
            }
        }

        private readonly byte[] key;
        private const string NombreArchivoClave = "cifrador.key";

        private Cifrador()
        {
            string ruta = ObtenerRutaArchivoClave();

            if (File.Exists(ruta))
            {
                key = File.ReadAllBytes(ruta);
            }
            else
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.GenerateKey();
                    key = aesAlg.Key;
                }

                string carpeta = Path.GetDirectoryName(ruta);
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }
                File.WriteAllBytes(ruta, key);
            }
        }

        private string ObtenerRutaArchivoClave()
        {
            // En un request web normal usamos App_Data (protegida: IIS no sirve ese contenido por HTTP).
            // El fallback es solo para el caso de que esta clase se use fuera de un request HTTP
            // (por ejemplo, una tarea programada mas adelante).
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath("~/App_Data/" + NombreArchivoClave);
            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", NombreArchivoClave);
        }

        public string EncriptarIrreversible(string textoEncriptar)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(textoEncriptar);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        public string EncriptarReversible(string textoEncriptar)
        {
            if (textoEncriptar == null) return null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.GenerateIV();

                using (ICryptoTransform encriptador = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // El IV no es secreto: se antepone al texto cifrado para poder desencriptar despues.
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encriptador, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(textoEncriptar);
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string DesencriptarReversible(string textoDesencriptar)
        {
            if (textoDesencriptar == null) return null;

            byte[] datosCompletos = Convert.FromBase64String(textoDesencriptar);

            using (Aes aesAlg = Aes.Create())
            {
                int tamanioIV = aesAlg.BlockSize / 8;
                byte[] iv = new byte[tamanioIV];
                byte[] cifrado = new byte[datosCompletos.Length - tamanioIV];

                Array.Copy(datosCompletos, 0, iv, 0, tamanioIV);
                Array.Copy(datosCompletos, tamanioIV, cifrado, 0, cifrado.Length);

                aesAlg.Key = key;
                aesAlg.IV = iv;

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                using (MemoryStream msDecrypt = new MemoryStream(cifrado))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}