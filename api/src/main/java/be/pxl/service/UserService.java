package be.pxl.service;

import be.pxl.entity.User;
import be.pxl.repository.UserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
@Service
public class UserService implements IUserService {
    @Autowired
    private UserRepository repo;
    @Override
    public User find(String id) {
        return repo.findOne(id);
    }

    @Override
    public List<User> All() {
        return repo.findAll();
    }

    @Override
    public void persist(User user) {
        repo.save(user);
    }

    @Override
    public void update(User user) {
        repo.save(user);
    }

    @Override
    public void delete(String id) {
        repo.delete(id);
    }

    @Override
    public User findLoggedIn(String token) {
        return repo.findLoggedIn(token);
    }
}
