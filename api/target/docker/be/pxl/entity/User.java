package be.pxl.entity;

import javax.persistence.*;

@Entity
@Table(name="users")
public class User {
    @Id
    private String username;
    private String password;
    @Lob
    byte[] pubKey;
    @Lob
    byte[] privKey;
    private Boolean active;
    private String accesstoken;

    public User() {
    }

    public byte[] getPubKey() {
        return pubKey;
    }

    public void setPubKey(byte[] pubKey) {
        this.pubKey = pubKey;
    }

    public byte[] getPrivKey() {
        return privKey;
    }

    public void setPrivKey(byte[] privKey) {
        this.privKey = privKey;
    }



    public String getAccesToken() {
        return accesstoken;
    }

    public void setAccesToken(String accesToken) {
        this.accesstoken = accesToken;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public Boolean getActive() {
        return active;
    }

    public void setActive(Boolean active) {
        this.active = active;
    }

}
