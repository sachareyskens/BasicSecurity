package be.pxl.crypter;

import be.pxl.entity.Message;
import be.pxl.service.IUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import javax.crypto.*;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;
import java.security.*;
import java.util.Arrays;

@Component
public class Crypter {

    @Autowired
    private IUserService service;
    public SecretKey aesKey;
    byte[] iv = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    KeyPairGenerator kpg = new KeyPairGenerator();

    private byte[] signature = new byte[500];

    public Crypter() {
        try {
            createAESKey();
        } catch (NoSuchPaddingException e) {

        } catch (NoSuchAlgorithmException e) {

        } catch (InvalidKeyException e) {

        }
    }


    public  void createAESKey() throws NoSuchPaddingException, NoSuchAlgorithmException, InvalidKeyException {
        KeyGenerator keyGen = KeyGenerator.getInstance("AES");
        keyGen.init(256);
        aesKey = keyGen.generateKey();
    }

    public Message encryptMessage(Message message) {
        IvParameterSpec ivspec = new IvParameterSpec(iv);
        KeyPair recieverPair =  kpg.createKeyPair(service.find(message.getReceiver()).getPrivKey(), service.find(message.getReceiver()).getPubKey());
        KeyPair senderPair = kpg.createKeyPair(service.find(message.getSender()).getPrivKey(), service.find(message.getSender()).getPubKey());


        Cipher aesCipher;
        byte[] encrypted;
        try {

             //Sign hash with private key sender
            MessageDigest messageDigest = MessageDigest.getInstance("SHA-256");
            messageDigest.update(message.getMessage().getBytes());
            signature = messageDigest.digest();
            Cipher cipher = Cipher.getInstance("RSA");
            cipher.init(Cipher.ENCRYPT_MODE, senderPair.getPrivate());
            byte[] cipherText = cipher.doFinal(signature);
             //Encrypt the message with AES.
            aesCipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
            aesCipher.init(Cipher.ENCRYPT_MODE, aesKey, ivspec);
            encrypted = aesCipher.doFinal(message.getMessage().getBytes());
            // Encrypt the AES key with public key from reciever
            Cipher rsaCipher = Cipher.getInstance("RSA");
            rsaCipher.init(Cipher.ENCRYPT_MODE, recieverPair.getPublic());
            byte[] encodedKey = rsaCipher.doFinal(aesKey.getEncoded());


            // Set variables
            message.setEncryptedSymm(encodedKey);
            message.setMessage(kpg.bytes2String(encrypted));
            message.setSignature(cipherText);

        } catch (NoSuchAlgorithmException e) {

            message.setValidation("Error encrypting. Contact provider");
        } catch (NoSuchPaddingException e) {

            message.setValidation("Error encrypting. Contact provider");
        } catch (InvalidKeyException e) {

            message.setValidation("Error encrypting. Contact provider");
        } catch (BadPaddingException e) {

            message.setValidation("Error encrypting. Contact provider");
        } catch (IllegalBlockSizeException e) {

            message.setValidation("Error encrypting. Contact provider");
        } catch (InvalidAlgorithmParameterException e) {

            message.setValidation("Error encrypting. Contact provider");
        }

        return message;
    }

    public Message decryptMessage(Message message, String username) {
        IvParameterSpec ivspec = new IvParameterSpec(iv);
        KeyPairGenerator kpg = new KeyPairGenerator();


        try {
            KeyPair recieverPair = kpg.createKeyPair(service.find(username).getPrivKey(), service.find(username).getPubKey());
            KeyPair senderPair = kpg.createKeyPair(service.find(message.getSender()).getPrivKey(), service.find(message.getSender()).getPubKey());

            // decrypt AES key
            Cipher rsaCipher = Cipher.getInstance("RSA");
            rsaCipher.init(Cipher.DECRYPT_MODE, recieverPair.getPrivate());
            byte[] aeskey = rsaCipher.doFinal(message.getEncryptedSymm());
            SecretKeySpec aesKey = new SecretKeySpec(aeskey, "AES");

            // decrypt text
            Cipher aesCipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
            aesCipher.init(Cipher.DECRYPT_MODE, aesKey, ivspec);
            byte[] plainText = aesCipher.doFinal(kpg.String2bytes(message.getMessage()));
            // Encrypt text for hash
            MessageDigest messageDigest = MessageDigest.getInstance("SHA-256");
            messageDigest.update(plainText);
            byte[] checker = messageDigest.digest();
           //  Check hash to see who sent message
            Cipher shaCipher = Cipher.getInstance("RSA");
            shaCipher.init(Cipher.DECRYPT_MODE, senderPair.getPublic());
            byte[] hash = shaCipher.doFinal(message.getSignature());

            if (checkHash(checker, hash)) {

                message.setValidation("OK, HASHES ARE EQUAL");
            } else {
                message.setValidation("FALSE, NOT RIGHT HASH");
            }
            message.setMessage(new String(plainText));
        } catch (NoSuchAlgorithmException e) {

            message.setValidation("Error decrypting. Contact provider");
        } catch (NoSuchPaddingException e) {

            message.setValidation("Error decrypting. Contact provider");
        } catch (InvalidKeyException e) {

            message.setValidation("Error decrypting. Contact provider");
        } catch (BadPaddingException e) {

            message.setValidation("Error decrypting. Contact provider");
        } catch (IllegalBlockSizeException e) {

            message.setValidation("Error decrypting. Contact provider");
//        } catch (UnsupportedEncodingException e) {
//
        } catch (InvalidAlgorithmParameterException e) {

            message.setValidation("Error decrypting. Contact provider");
        }catch (NullPointerException e) {

            message.setValidation("Error decrypting. Contact provider");
        }
        return message;
    }

    public SecretKey getAesKey() {
        return aesKey;
    }

    public boolean checkHash(byte[] c, byte[] tc) {
        if (Arrays.equals(c, tc)) {
            return true;
        } else {
            return false;
        }
    }




}
