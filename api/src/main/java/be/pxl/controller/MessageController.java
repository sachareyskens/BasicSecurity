package be.pxl.controller;

import be.pxl.crypter.Crypter;
import be.pxl.entity.Message;
import be.pxl.entity.User;
import be.pxl.service.IMessageService;
import be.pxl.service.IUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@CrossOrigin
@RequestMapping(MessageController.BASE_URL)
public class MessageController {
    @Autowired
    private IMessageService service;
    @Autowired
    Crypter crypter;



    public static final String BASE_URL = "api/messages";


    @RequestMapping(value= "/get/{id}", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public ResponseEntity<Message> getMessage(@PathVariable("id") int key) {
        HttpStatus status = HttpStatus.OK;
        Message message = service.find(key);

        return new ResponseEntity<>(message, status);
    }

    @RequestMapping(value= "/all", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public List<Message> AllMessages() {
        return service.All();
    }
    @RequestMapping(value="/add", method = RequestMethod.POST, consumes = MediaType.APPLICATION_JSON_VALUE)
    public void addMessage(@RequestBody Message message) {

        service.persist(crypter.encryptMessage(message));
    }
    @RequestMapping(value="/update", method = RequestMethod.POST, consumes = MediaType.APPLICATION_JSON_VALUE)
    public void updateMessage(@RequestBody Message message) {

        service.update(message);
    }

    @RequestMapping(value = "/delete/{id}", method = RequestMethod.DELETE)
    public void deleteMessage(@PathVariable("id") int id) {
        service.delete(id);
    }

    @RequestMapping(value= "/decrypt/{id}", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public ResponseEntity<Message> decrypt(@PathVariable("id") int key, @RequestParam("loggedIn") String username) {
        HttpStatus status = HttpStatus.OK;
        try {
            Message message = service.find(key);
            Message messagee = crypter.decryptMessage(message, username);
            byte[] hidden = new byte[1];
            messagee.setEncryptedSymm(hidden);
            service.delete(messagee.getId());
            return new ResponseEntity<>(messagee, status);
        } catch (Exception e) {
            return new ResponseEntity<>(new Message(), HttpStatus.NOT_FOUND);
        }

    }
    @RequestMapping(value= "/showall}", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public List<Message> showAllByUsername(@RequestParam("username") String username) {
        return service.findByUsername(username);
    }




}
