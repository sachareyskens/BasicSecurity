package be.pxl.crypter;


import org.apache.tomcat.util.codec.binary.Base64;

import javax.xml.bind.DatatypeConverter;
import java.math.BigInteger;
import java.security.*;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;
import java.security.interfaces.RSAPublicKey;
import java.security.spec.EncodedKeySpec;
import java.security.spec.PKCS8EncodedKeySpec;
import java.security.spec.X509EncodedKeySpec;

public class KeyPairGenerator {
    public KeyPair generateKeyPair() throws NoSuchAlgorithmException {
        SecureRandom random = new SecureRandom();
        java.security.KeyPairGenerator keyPairGenerator = java.security.KeyPairGenerator.getInstance("RSA");
        keyPairGenerator.initialize(2048, random);
        KeyPair kp = keyPairGenerator.generateKeyPair();
        return kp;

    }
    public KeyPair createKeyPair(byte[] encodedPrivateKey, byte[] encodedPublicKey) {
        try {
            EncodedKeySpec privateKeySpec = new PKCS8EncodedKeySpec(encodedPrivateKey);
            KeyFactory generator = KeyFactory.getInstance("RSA");
            PrivateKey privateKey = generator.generatePrivate(privateKeySpec);

            EncodedKeySpec publicKeySpec = new X509EncodedKeySpec(encodedPublicKey);
            PublicKey publicKey = generator.generatePublic(publicKeySpec);



            return new KeyPair(publicKey, privateKey);
        } catch (Exception e) {
            e.printStackTrace();
            throw new IllegalArgumentException("Failed to create KeyPair from provided encoded keys");

        }
    }

    public String bytes2String(byte[] bytes) {
        return Base64.encodeBase64String(bytes);
    }

    public byte[] String2bytes(String string) {
        return Base64.decodeBase64(string);
    }
}
