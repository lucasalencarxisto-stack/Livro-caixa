
using System;
using System.Security.Cryptography;

namespace LivroCaixa.Classes
{
    public class Autenticacao
    {
        private const int Iteracoes = 600000;
        private const int TamanhoHash = 32;
        private const int TamanhoSalt = 16;

        public byte[] GerarSalt()
        {
            byte[] salt = new byte[TamanhoSalt];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        public byte[] GerarHash(string senha, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                senha,
                salt,
                Iteracoes,
                HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(TamanhoHash);
            }
        }

        public bool VerificarSenha(
            string senha,
            byte[] salt,
            byte[] hashArmazenado)
        {
            if (senha == null ||
                salt == null ||
                hashArmazenado == null ||
                salt.Length != TamanhoSalt ||
                hashArmazenado.Length != TamanhoHash)
            {
                return false;
            }

            byte[] hashDigitado = GerarHash(senha, salt);

            int diferenca = 0;

            for (int i = 0; i < TamanhoHash; i++)
            {
                diferenca |= hashDigitado[i] ^ hashArmazenado[i];
            }

            return diferenca == 0;
        }
    }
}
