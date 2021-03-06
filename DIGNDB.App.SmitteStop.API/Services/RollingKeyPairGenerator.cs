﻿using DIGNDB.App.SmitteStop.API.Exceptions;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System;
using System.Linq;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class RollingKeyPairGenerator
    {
        private readonly byte[] _masterKeyBytes;

        public X9ECParameters ECParameters { get; }

        public RollingKeyPairGenerator(byte[] masterKeyBytes, X9ECParameters ecParameters)
        {
            _masterKeyBytes = masterKeyBytes;
            ECParameters = ecParameters;
        }

        public BigInteger GetPrivateKey(long keyId)
        {
            return GeneratePrivateKey(_masterKeyBytes, keyId, ECParameters);
        }

        public (BigInteger privateKey, ECPublicKeyParameters publicKey) GenerateKeyPairForInterval(long keyIntervalNumber)
        {
            var privateKey = GeneratePrivateKey(_masterKeyBytes, keyIntervalNumber, ECParameters);
            var publicKey = CalculatePublicKey(privateKey, ECParameters);
            return (privateKey, publicKey);
        }

        private static BigInteger GeneratePrivateKey(byte[] masterKeyBytes, long keyIntervalNumber, X9ECParameters ecParameters)
        {
            const int maxIterations = 1000;
            var keyByteSize = (int)Math.Ceiling(ecParameters.Curve.Order.BitCount / 8.0);
            var counter = 0;

            BigInteger privateKey;
            do
            {
                var privateKeyBytes = GeneratePrivateKeyBytes(keyByteSize, masterKeyBytes, keyIntervalNumber, counter);
                privateKey = new BigInteger(privateKeyBytes);
                counter++;
                if (counter > maxIterations)
                    throw new CannotMatchPrivateKeyToCurveException();
            } while (privateKey.CompareTo(ecParameters.Curve.Order) > 0);

            return privateKey;
        }

        private static byte[] GeneratePrivateKeyBytes(int numBytes, byte[] masterKeyBytes, long keyIntervalNumber, int counter)
        {
            var keyIntervalBytes = BitConverter.GetBytes(keyIntervalNumber);
            var counterBytes = BitConverter.GetBytes(counter);
            var ikm = masterKeyBytes;
            var salt = keyIntervalBytes.Concat(counterBytes).ToArray();
            var hkdf = new HkdfBytesGenerator(new Sha256Digest());
            var hParams = new HkdfParameters(ikm, salt, null);
            hkdf.Init(hParams);
            byte[] keyBytes = new byte[numBytes];
            hkdf.GenerateBytes(keyBytes, 0, numBytes);
            return keyBytes;
        }

        private static ECPublicKeyParameters CalculatePublicKey(BigInteger privateKey, X9ECParameters ecParameters)
        {
            var publicKeyPoint = ecParameters.G.Multiply(privateKey);
            var domainParams = new ECDomainParameters(ecParameters);
            return new ECPublicKeyParameters("ECDSA", publicKeyPoint, domainParams);
        }
    }
}
