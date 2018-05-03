package be.pxl.service;

import be.pxl.entity.User;
import org.springframework.beans.factory.annotation.Configurable;

import java.util.List;

@Configurable
public interface IUserService {
    User find(String id);
    List<User> All();
    void persist(User user);
    void update(User user);
    void delete(String id);
    User findLoggedIn(String token);
    int countAll();

    List<String> getAllNames();

}
