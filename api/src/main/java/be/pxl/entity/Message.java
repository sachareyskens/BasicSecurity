package be.pxl.entity;

import javax.persistence.*;


@Entity
@Table(name = "messages")
public class Message {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private int id;
    private String message;
    private String sender;
    private String reciever;
    @Lob
    private byte[] encryptedSymm;
    @Lob
    private byte[] signature;
    private String validation;
    private String date;

    public String getDate() {
        return date;
    }

    public void setDate(String date) {
        this.date = date;
    }

    public String getValidation() {
        return validation;
    }

    public void setValidation(String validation) {
        this.validation = validation;
    }

    public byte[] getSignature() {
        return signature;
    }

    public void setSignature(byte[] signature) {
        this.signature = signature;
    }

    public byte[] getEncryptedSymm() {
        return encryptedSymm;
    }

    public void setEncryptedSymm(byte[] encryptedSymm) {
        this.encryptedSymm = encryptedSymm;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getSender() {
        return sender;
    }

    public void setSender(String sender) {
        this.sender = sender;
    }

    public String getReceiver() {
        return reciever;
    }

    public void setReceiver(String receiver) {
        this.reciever = receiver;
    }
}
