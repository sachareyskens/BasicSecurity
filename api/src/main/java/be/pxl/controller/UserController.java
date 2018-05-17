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

import java.security.KeyPair;
import java.security.NoSuchAlgorithmException;
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

    // Get a single user.
    @RequestMapping(value="/get/{username}", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public ResponseEntity<User> getUser(@PathVariable("username") String username) {
        HttpStatus status = HttpStatus.OK;

        User user = service.find(username);

        if (user==null) {
            status = HttpStatus.NOT_FOUND;
        } else {

        }

        return new ResponseEntity<>(user, status);
    }
    // Show all users.
    @RequestMapping(value="/all", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public List<User> getAllUsers() {
        return service.All();
    }
    // Add a user and gen a keypair for him.
    @RequestMapping(value ="/add", method = RequestMethod.POST, consumes = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ResponseEntity<Boolean> addUser(@RequestBody User user) throws NoSuchAlgorithmException {
        HttpStatus status = HttpStatus.OK;
        if (service.find(user.getUsername()) == null ) {
            user.setPassword(encoder.encode(user.getPassword()));
            KeyPairGenerator kpg = new KeyPairGenerator();
            KeyPair kp = kpg.generateKeyPair();
            user.setPubKey(kp.getPublic().getEncoded());
            user.setPrivKey(kp.getPrivate().getEncoded());
            service.persist(user);
            return new ResponseEntity<Boolean>(true, status);
        } else {
            return new ResponseEntity<Boolean>(false, status);
        }

    }
    // Update a user.
    @RequestMapping(value ="/update", method = RequestMethod.PUT,consumes = MediaType.APPLICATION_JSON_VALUE)
    public void updateUser(@RequestBody User user) { service.update(user);}
    // Let a user login.
    @RequestMapping(value="/login", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    @ResponseBody
    public ResponseEntity<User> validateUser(@RequestParam(value="username") String username, @RequestParam(value = "password") String rawPassword) {
        HttpStatus status = HttpStatus.OK;
        User user = service.find(username);
        if (user == null) {
            return new ResponseEntity<>(new User(), status);
        } else {
            if (encoder.matches(rawPassword, user.getPassword())) {
                String accesstoken = UUID.randomUUID().toString();
                user.setAccesToken(accesstoken);
                service.update(user);
                user.setPassword("HIDDEN");
                return new ResponseEntity<>(user, status);
            } else {
                return new ResponseEntity<>(new User(), status);
            }
        }
    }
    // Validate token of login, in order to do SSO.
    @RequestMapping(value="/validatetoken", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    @ResponseBody
    public ResponseEntity<Boolean> validateToken(@RequestParam(value="token") String token) {
        HttpStatus status = HttpStatus.OK;
        User user = service.findLoggedIn(token);
        if (user != null) {
            if (token.equals(user.getAccesToken())) {
                return new ResponseEntity<Boolean>(true, status);
            } else {
                return new ResponseEntity<Boolean>(false, status);
            }
        } else {
            return new ResponseEntity<Boolean>(false, status);
        }
    }
    // Logout and remove all tokens.
    @RequestMapping(value="/logout", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    @ResponseBody
    public boolean logoutUser(@RequestParam(value="username") String username) {
        User user = service.find(username);
        user.setAccesToken("");
        service.update(user);
        return true;
    }
    // Show all names of all users.
    @RequestMapping(value="/names", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public List<String> findAllNames() {
        return service.getAllNames();
    }
}
