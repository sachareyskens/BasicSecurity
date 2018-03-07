package be.pxl.service;


import be.pxl.entity.Message;
import be.pxl.repository.MessageRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class MessageService implements IMessageService{
    @Autowired
    private MessageRepository repo;
    @Override
    public Message find(int id) {
        return repo.findOne(id);
    }

    @Override
    public List<Message> All() {
        return repo.findAll();
    }

    @Override
    public void persist(Message message) {
        repo.save(message);
    }

    @Override
    public void update(Message message) {
        repo.save(message);
    }

    @Override
    public void delete(int id) {
        repo.delete(id);
    }

    @Override
    public List<Message> findByUsername(String username) {
        return repo.findByUsername(username);
    }

    @Override
    public int countAll() {
        return repo.countAll();
    }
}
