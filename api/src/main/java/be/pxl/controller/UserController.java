package be.pxl.controller;

import be.pxl.crypter.Crypter;
import be.pxl.crypter.KeyPairGenerator;
import be.pxl.entity.User;
import be.pxl.service.IUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.web.bind.annotation.*;

import java.security.*;
import java.util.List;
import java.util.UUID;
@RestController
@CrossOrigin
@RequestMapping(UserController.BASE_URL)
public class UserController {
    @Autowired
    private IUserService service;
    PasswordEncoder encoder = new BCryptPasswordEncoder(12);
    public static final String BASE_URL="/api/users";
    @Autowired
    Crypter crypter;


    @RequestMapping(value="/get/{username}", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public ResponseEntity<User> getUser(@PathVariable("username") String username) {
        HttpStatus status = HttpStatus.OK;

        User user = service.find(username);

        if (user==null) {
            status = HttpStatus.NOT_FOUND;
        }

        return new ResponseEntity<>(user, status);
    }

    @RequestMapping(value="/all", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public List<User> getAllUsers() {
        return service.All();
    }

    @RequestMapping(value ="/add", method = RequestMethod.POST, consumes = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public Object addUser(@RequestBody User user) throws NoSuchAlgorithmException {
        if (service.find(user.getUsername()) == null ) {
            user.setPassword(encoder.encode(user.getPassword()));
            KeyPairGenerator kpg = new KeyPairGenerator();
            KeyPair kp = kpg.generateKeyPair();
            user.setPubKey(kp.getPublic().getEncoded());
            user.setPrivKey(kp.getPrivate().getEncoded());
            service.persist(user);
            return user;
        } else {
            return false;
        }

    }

    @RequestMapping(value ="/update", method = RequestMethod.PUT,consumes = MediaType.APPLICATION_JSON_VALUE)
    public void updateUser(@RequestBody User user) { service.update(user);}

    @RequestMapping(value="/login", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    @ResponseBody
    public Object validateUser(@RequestParam(value="username") String username, @RequestParam(value = "password") String rawPassword) {
        User user = service.find(username);
        if (user == null) {
            return false;
        } else {
            if (encoder.matches(rawPassword, user.getPassword())) {
                String accesstoken = UUID.randomUUID().toString();
                user.setAccesToken(accesstoken);
                service.update(user);
                user.setPassword("HIDDEN");
                return user;
            } else {
                return false;
            }
        }
    }
    @RequestMapping(value="/validatetoken", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    @ResponseBody
    public Boolean validateToken(@RequestParam(value="username") String username, @RequestParam(value="token") String token) {
        User user = service.find(username);
        if (user != null) {
            if (user.getAccesToken().equals(token)) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    @RequestMapping(value="/logout", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    @ResponseBody
    public void logoutUser(@RequestParam(value="username") String username) {
        User user = service.find(username);
        user.setAccesToken("");
        service.update(user);
    }


}
