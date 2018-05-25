package be.pxl.service;

import be.pxl.entity.Message;
import org.springframework.beans.factory.annotation.Configurable;

import java.util.List;

@Configurable
public interface IMessageService {
    Message find(int id);
    List<Message> All();
    void persist(Message message);
    void update(Message message);
    void delete(int id);
    List<Message> findByUsername(String username);
    int countAll();

    List<Message> findBySenderAndReciever(String sender, String reciever);
}
