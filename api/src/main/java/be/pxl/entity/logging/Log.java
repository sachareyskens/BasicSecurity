package be.pxl.entity.logging;

import javax.persistence.*;
import java.sql.Timestamp;

@Entity
@Table(name="logs")
public class Log {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private int id;

    private String log;

    private Timestamp date;

    public Log() {}

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getLog() {
        return log;
    }

    public void setLog(String log) {
        this.log = log;
        this.date = new Timestamp(System.currentTimeMillis());
    }

    public Timestamp getDate() {
        return date;
    }

    public void setDate(Timestamp date) {
        this.date = date;
    }

    public Log(String log) {
        this.log = log;
        this.date = new Timestamp(System.currentTimeMillis());
    }
}
